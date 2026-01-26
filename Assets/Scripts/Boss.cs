using System;
using System.Collections;
using System.Collections.Generic;
using GenesisStudio;
using PlatformCharacterController;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;



[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public abstract class Boss : MonoBehaviour, IDamageable
{
    [SerializeField] private BossData _bossData;

    [Header("Stats")]
    [field: SerializeField, Range(0, 100)] public float Health { get; set; }
    [field: SerializeField, Range(0, 100)] public float Damage, DamageIncreaseByPhase, PlayerCheckDistance;
    
    [field: SerializeField] HitBox[] hitBoxes;
    [SerializeField] private int phases;

    
    [Space(5f)] public Path patrolPath;
    public MovementCharacterController player;
    
    
    private BossState _state;

    private delegate IEnumerator BossAction();

    private BossAction _bossAction;
    private Coroutine _bossActionCoroutine;
    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;
    private List<Transform> _points => patrolPath.points;
    private float _maxHealth;
    private int _currentPhase;
    private Animator _animator;
    
    
    
    public enum BossState
    {
        Idle,
        Patrol,
        Chasing, 
        Attacking,
    }
    
    public BossState State
    {
        get => _state;
        set
        {
            if (_bossActionCoroutine != null)
            {
                StopCoroutine(_bossActionCoroutine);
                _bossActionCoroutine = null;
            }

            _agent.isStopped = false;

            _state = value;
            switch (_state)
            {
                case BossState.Idle:
                    _bossAction = null;
                    break;
                case BossState.Patrol:
                    _bossAction = Patrol;
                    break;
                case BossState.Chasing:
                    _bossAction = Chase;
                    break;
                case BossState.Attacking:
                    _bossAction = Attack;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            print($"<color=green>Changed State to {_state}</color>");
            if (_bossAction != null)
            {
                _bossActionCoroutine = StartCoroutine(_bossAction());
            }
        }
    }
    
    protected abstract List<Func<IEnumerator>> Combo(); // return => attack -> attack -> super attack -> go to pos -> custom method
    protected abstract IEnumerator AttackCoroutine();
    protected abstract IEnumerator HeavyAttack();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, PlayerCheckDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerCheckDistance/2f);
    }

    private void Start()
    {
        _bossData.Initialize(this);
        
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        gameObject.TryGetComponent(out _animator);
        
        
        foreach (var hitBox in hitBoxes)
        {
            hitBox.damage = Damage;
        }

        State = BossState.Patrol;
        _currentPhase = phases;
    }

    #region Animation
    
    //triggers
    protected void A_SetTrigger(int t)
    {
        _animator?.SetTrigger(t);
    }

    #endregion

    #region Helpers

    private void HitBoxes(bool value)
    {
        foreach (var hitBox in hitBoxes)
        {
            hitBox.active = value;
        }
    }
    private bool ReachedDestination(Transform target = null)
    {
        return transform.IsNearThePoint(target == null ? _agent.destination : target.position);
    }
    
    private bool SeePlayer()
    {
        print("Checking for player");
        bool result = transform.IsNearThePoint(player.transform, PlayerCheckDistance);
        if (result)
            State = BossState.Chasing;

        return result;
    }
    
    private float RandomTime()
    {
        return Random.Range(0.8f, 1.2f);
    }
    #endregion

    #region Behaviour

    public void ChangeState(BossState newState)
    {
        State = newState;
    }

    protected virtual IEnumerator Chase()
    {
        _agent.isStopped = false;
        while (Vector3.Distance(transform.position, player.transform.position) > 1f)
        {
            _agent.SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.2f);
            if (Vector3.Distance(transform.position, player.transform.position) < PlayerCheckDistance/2f)
            {
                State = BossState.Attacking;
                yield break;
            }
        }
    }
    
    protected virtual IEnumerator Patrol()
    {
        while (true)
        {
            var randomPoint = _points.GetRandomItemFromList();
            _agent.SetDestination(randomPoint.position);
            yield return new WaitUntil(() => ReachedDestination() || SeePlayer());
        }
    }
    
    protected virtual IEnumerator Attack()
    {   
        _agent.isStopped = true;
        var attackCombat = new Queue<Func<IEnumerator>>(Combo());
        while (attackCombat.Count > 0)
        {
            var current = attackCombat.Dequeue();
            yield return new WaitForSeconds(RandomTime());
            yield return StartCoroutine(current());
            if (Vector3.Distance(transform.position, player.transform.position) > PlayerCheckDistance)
            {
                State = BossState.Chasing;
                yield break;
            }
        }
        State = BossState.Chasing;
        
    }

    protected virtual IEnumerator NewPhase()
    {
        _currentPhase--;
        
        if (_currentPhase == 0)
            yield break;

        Damage *= Mathf.Pow(1f + DamageIncreaseByPhase/100f, _currentPhase);
        
        yield return null;
    }

    #endregion
    
    public void ApplyDamage(float damage)
    {
        Health -= damage;

        float healthPercent = Health * 100 / _maxHealth;
        float phaseCount = _currentPhase * _maxHealth / phases; 
        
        if(healthPercent <= phaseCount)
            StartCoroutine(NewPhase());
        
        if (Health <= 0)
            Die();
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
