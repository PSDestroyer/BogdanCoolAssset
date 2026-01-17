using System;
using UnityEngine;

namespace GenesisStudio
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrabableObject : MonoBehaviour, IGrabable
    {
        private Rigidbody _rigidbody;
        private Transform _hands;
        
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>(); 
        }

        public void Grab(Transform hands)
        {
            transform.parent = hands;
            _hands = hands;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            _rigidbody.isKinematic = true;
        }

        public void Release()
        {
            transform.parent = null;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(_hands.forward * 2f, ForceMode.Impulse);
            _hands = null;
        }
    }
}