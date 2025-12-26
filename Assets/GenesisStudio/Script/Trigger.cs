using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GenesisStudio
{
    public enum TriggerType
    {
        Once,
        MultipleTimes
    }
    [RequireComponent(typeof(BoxCollider))]
    public class Trigger : MonoBehaviour
    {
        public GameObject TargetObject;
        
        public TriggerType triggerType;
        [Space(5f)]
        public UnityEvent onTriggerEnter;
        [Space(5f)]
        public UnityEvent onTriggerStay;
        [Space(5f)]
        public UnityEvent onTriggerExit;

        private bool triggered;
        private void OnTriggerEnter(Collider other)
        {
            ExecuteEvent(onTriggerEnter, other);
        }
        private void OnTriggerStay(Collider other)
        {
            ExecuteEvent(onTriggerStay, other);
        }
        private void OnTriggerExit(Collider other)
        {
            ExecuteEvent(onTriggerExit, other);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;   
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }


        private void ExecuteEvent(UnityEvent @event, Collider other)
        {
            if (TargetObject && other.gameObject != TargetObject) return;
            switch (triggerType)
            {
                case TriggerType.Once:
                    if(triggered) break;
                    @event?.Invoke();
                    triggered = true;
                    break;
                case TriggerType.MultipleTimes:
                    @event?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
}