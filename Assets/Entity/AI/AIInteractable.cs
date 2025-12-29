using System;
using System.Collections;
using UnityEngine;

namespace GenesisStudio
{
    public class AIInteractable : MonoBehaviour
    {
        [field: SerializeField] protected AIInteracableObjectSettingsSo settings;
        [field: SerializeField] protected Transform _interactMarker;
        [field: NonSerialized] public bool interacted;
        protected AIBrain _user;
        
        protected Coroutine _performCoroutine;
        public Vector3 InteractPoint => _interactMarker ? _interactMarker.position : transform.position;

        public virtual void Lock(AIBrain brain)
        {
            if(_user != null)
            {
                Debug.LogError($"Trying to register {_user}");
                return;
            }
            _user = brain;
            AIInteractManager.Instance.RegisterInteractable(this, _user);
            
            _performCoroutine ??= StartCoroutine(Perform(brain, machine =>
            {
                machine.ChangeState(machine.idleState);
                Debug.Log("Perofrmig Canceled!");
            }));
        }

        protected virtual IEnumerator Perform(AIBrain brain, Action<StateMachine> onComplete = null) // se va chema la interact state
        {
            Debug.Log($"Inetracted from {brain}");
            
            yield return new WaitForSeconds(5f);
            onComplete?.Invoke(_user.stateMachine);
            Unlock();
        }

        public virtual void Unlock()
        {
            if (_user)
            {
                AIInteractManager.Instance.DeregisterInteractable(this, _user);
                _user = null;
            }
            else
            {
                Debug.LogError($"trying to deregister {this}");
            }
        }
    }
}