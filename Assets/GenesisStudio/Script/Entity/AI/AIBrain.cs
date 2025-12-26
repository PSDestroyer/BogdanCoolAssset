using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GenesisStudio
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class AIBrain : Brain
    {
        [field: SerializeField] public Path currentPath { get; protected set; }
        [field: NonSerialized] public bool isBusy;
        
        protected NavMeshAgent _agent;
        protected StateMachine _stateMachine;
        protected Animator _animator;
        public Ray ray => new Ray(transform.position, transform.forward);
        public NavMeshAgent Agent => _agent;
        public StateMachine stateMachine => _stateMachine;

        // TODO: Implement a sight to detect objects in angle of view.
        public float angleOfView = 120f;
        public float origin = 1f;
        public float sightDistance = 10f;
        float halfAngleRad => Mathf.Deg2Rad * (angleOfView / 2f);
        float radius => Mathf.Tan(halfAngleRad) * sightDistance / 1.5f;
        public bool ReachedDestination => !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance && (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
        public bool ReachedTarget(Transform target)
        {
            
            return this.transform.IsNearThePoint(target);
        }

        private float _normalSpeed = 1.5f;
        private float _sprintSpeed = 3.5f;

        public void Sprint(bool value)
        {
            Speed = value ? _sprintSpeed : _normalSpeed;
            _agent.speed = Speed;
        }

        public override void Move(Vector3 destination)
        {
            if(!_agent.hasPath)
            {
                _agent.ResetPath();
                _agent.SetDestination(destination);
            }
        }

        public void MoveInstantly(Vector3 destination)
        {
            _agent.SetDestination(destination);
        }

        protected override void Initialize()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            
            Speed = _normalSpeed;
            _agent.speed = Speed;

            if (TryGetComponent(out StateMachine machine))
            {
                _stateMachine = machine;
                _stateMachine.Initialize(_stateMachine.idleState);
            }
        }

        private void Update()
        {
            _animator.SetFloat("Speed", _agent.velocity.magnitude, .4f, Time.deltaTime);
        }

        public bool IsInSight(Transform target)
        {
            bool result = false;
            Vector3 origin = transform.position + Vector3.up * this.origin;
            Collider[] hitColliders = Physics.OverlapSphere(origin + transform.forward * sightDistance, radius);
            foreach (var collider in hitColliders)
            {
                if(collider.transform == target)
                {
                    result = true; break;
                }
            }
            return result;
        }

        public void OnDrawGizmosSelected()
        {
            Vector3 origin = transform.position + Vector3.up * this.origin;
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(origin, Quaternion.Euler(0, angleOfView / 2, 0) * transform.forward * sightDistance);
            Gizmos.DrawRay(origin, Quaternion.Euler(0, -angleOfView / 2, 0) * transform.forward * sightDistance);
            
            Gizmos.DrawWireSphere(origin + transform.forward * sightDistance, radius);
        }

        public override void SetPath(Path newPath)
        {
            currentPath = newPath;
            if(newPath == null)
            {
                return;
            }
            stateMachine.ChangeState(new FollowThePathState(newPath));
        }

        public override void AddItem(ItemData item)
        {
            Inventory.AddItem(item);
            if (!QuestManager.Instance.IsActiveQuest) return;
            if (QuestManager.Instance.ActiveQuest is DeliverQuest dq)
            {
                Debug.Log("Delivered");
                dq.OnItemDelivered(item);
            }
        }

        public override void RemoveItem(ItemData item)
        {
            Inventory.RemoveItem(item);
        }

        public override bool HasItem(ItemData item)
        {
            throw new NotImplementedException();
        }

        public override Inventory.ItemInfo GetSelectedItem()
        {
            throw new NotImplementedException();
        }

        public override void SelectItem(int index)
        {
            throw new NotImplementedException();
        }

        public override void SelectItem(ItemData data)
        {
            throw new NotImplementedException();
        }

        public override bool CanAdd(Item item)
        {
            throw new NotImplementedException();
        }

        public override void AddItem(ItemData item, int amount)
        {
            Inventory.AddItems(item, amount);
            for (int i = 0; i < amount; i++)
            {
                if (!QuestManager.Instance.IsActiveQuest) return;
                if (QuestManager.Instance.ActiveQuest is DeliverQuest dq)
                {
                    dq.OnItemDelivered(item);
                }
            }
        }

        public override void RemoveItem(ItemData item, int amount)
        {
            throw new NotImplementedException();
        }
    }
}