using System.Threading.Tasks;
using UnityEngine;

namespace GenesisStudio
{
    public class PatrolState : State
    {
        private float _waitTimer;

        private int _waypointIndex = 0;
        private Path _path;
        private bool _canInteract;
        private Transform _interactablePoint;
        private Transform _destination;
        private AIInteractable _currentInteractable;

        public override void Enter()
        {
            _canInteract = false;
            _brain.isBusy = false;
            _currentInteractable = null;
            _path = _brain.currentPath;
            _brain.Agent.isStopped = false;
            _brain.Agent.stoppingDistance = 0.2f;
        }

        public override void Perform()
        {
            PatrolCycle();
            CheckForDoors(_brain.ray);
        }

        private void CheckForDoors(Ray ray)
        {
            float timer = 0;
            timer += Time.deltaTime;
            
            if(timer > 2)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 2f))
                {
                    if (hit.transform.gameObject.TryGetComponent(out Door door))
                    {
                        door.Interact(_brain);
                        timer = 0;
                        return;
                    }
                }
            }
        }

        public override void Exit()
        {
            
        }

        private void PatrolCycle()
        {
            if (_path == null || _path.points.Count == 0 || _brain.Agent.pathPending) return;
            _destination = _path.points[_waypointIndex];
            

            if (_brain.Agent.remainingDistance < _brain.Agent.stoppingDistance)
            {
                _waitTimer += Time.deltaTime;
                if (_waitTimer > 1) 
                {
                    if (_canInteract)
                    {
                        _brain.isBusy = true;
                        _brain.Agent.isStopped = true;
                        _currentInteractable.Lock(_brain);
                        _stateMachine.ChangeState(_stateMachine.interactState);
                        _currentInteractable.interacted = true;
                        return;
                    }

                    if (_waypointIndex < _path.points.Count - 1)
                    {
                        _waypointIndex++;
                    }
                    else
                    {
                        _waypointIndex = 0;
                    }

                    //daca urmatorul este interactable
                    if (_destination.CanAIInteractWithPoint())
                    {
                        _currentInteractable = _destination.GetComponent<AIInteractable>();
                        _canInteract = true;
                        _brain.Agent.stoppingDistance = 1f;
                        
                        _brain.Move(_currentInteractable.InteractPoint);
                        _waitTimer = 0;
                        return;
                    }

                    _canInteract = false;
                    _brain.Agent.stoppingDistance = 0.2f;
                    _brain.Move(_destination.position);
                    _waitTimer = 0;
                    Debug.Log($"Can interact {_canInteract} ");
                }
            }
        }
    }
}
