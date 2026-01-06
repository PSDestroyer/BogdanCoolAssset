using System;
using System.Collections;
using System.Collections.Generic;
using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour , IDamageable
{
    [Header("Stats")] 
    private float _health;
    public float Health
    {
        get => _health;
        set
        {
            if (value < 0)
            {
                _health = 0;
                Die();
            }
            _health = value;
        }
    }

    public void ApplyDamage(float damage)
    {
        Health -= damage;
    }


    [Header("Patrol")] 
    [SerializeField]
    private Path path;
    [SerializeField] float waitAtPoint = 2f;
    [Range(0f, 1f)]
    [SerializeField] float randomActionChance = 0.3f;
    List<Transform> patrolPoints;

    [Header("Detection")]
    [SerializeField] float viewDistance = 12f;
    [SerializeField] float attackDistance = 2f;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LayerMask playerMask;

    [Header("Attack")]
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField, Range(2, 100f)] private float damage;

    NavMeshAgent agent;
    Animator _animator;
    Transform player;

    Queue<Transform> patrolQueue;
    Coroutine currentStateRoutine;

    // -------------------- UNITY --------------------

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        patrolPoints = path.points;
        patrolQueue = new Queue<Transform>(patrolPoints);
    }

    void Start()
    {
        player = GameManager.Instance.PlayerObject.transform;
        StartPatrol();
    }

    private void Update()
    {
        _animator.SetFloat("Speed", agent.velocity.magnitude);
    }
    

    // -------------------- STATE CONTROL --------------------

    void ChangeState(IEnumerator newState)
    {
        if (currentStateRoutine != null)
            StopCoroutine(currentStateRoutine);

        currentStateRoutine = StartCoroutine(newState);
    }

    // -------------------- PATROL --------------------

    void StartPatrol()
    {
        ChangeState(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        agent.isStopped = false;

        while (true)
        {
            
            Transform point = GetNextPatrolPoint();
            agent.SetDestination(point.position);

            // Wait until destination reached
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                if (CanSeePlayer(out _))
                {
                    StartChase();
                    yield break;
                }

                yield return null;
            }

            // Random idle action
            if (Random.value < randomActionChance)
                DoRandomPatrolAction();

            yield return new WaitForSeconds(waitAtPoint);
        }
    }

    Transform GetNextPatrolPoint()
    {
        Transform point = patrolQueue.Dequeue();
        patrolQueue.Enqueue(point);
        return point;
    }

    void DoRandomPatrolAction()
    {
        // animation / sound / look around
        Debug.Log("Enemy does random patrol action");
    }

    // -------------------- CHASE --------------------

    void StartChase()
    {
        ChangeState(ChaseRoutine());
    }

    IEnumerator ChaseRoutine()
    {
        agent.isStopped = false;

        while (true)
        {
            agent.SetDestination(player.position);

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackDistance)
            {
                StartAttack();
                yield break;
            }

            if (!CanSeePlayer(out _))
            {
                StartPatrol();
                yield break;
            }

            yield return new WaitForSeconds(0.1f); // cheaper than Update
        }
    }

    // -------------------- ATTACK --------------------

    void StartAttack()
    {
        ChangeState(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        agent.isStopped = true;

        while (true)
        {
            transform.LookAt(player);
            DoAttack();

            yield return new WaitForSeconds(attackCooldown);

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > attackDistance)
            {
                StartChase();
                yield break;
            }
        }
    }

    void DoAttack()
    {
        if(CanSeePlayer(out MovementCharacterController Player))
        {
            // Debug.Log("Enemy attacks player!");
            _animator.SetTrigger("Attack");
            Player.Health -= damage;
        }
    }

    // -------------------- SENSORS --------------------

    bool CanSeePlayer(out MovementCharacterController Player)
    {
        Player = null;
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 direction = (player.position - origin).normalized;
        float distance = Vector3.Distance(origin, player.position);

        if (distance > viewDistance)
            return false;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance, obstacleMask | playerMask))
        {
            return hit.transform.TryGetComponent(out Player);
        }

        return false;
    }


    public void Die()
    {
        Destroy(gameObject);
    }
    // -------------------- DEBUG --------------------

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
