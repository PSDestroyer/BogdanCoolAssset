using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenesisStudio
{
    public class Compas : MonoBehaviour
    {
        
        [field: SerializeField] public RectTransform _compas;
        [field: SerializeField] public CameraMotor _cameraMotor;
        [field: SerializeField, Range(-400f, -10f)] private float size = -360f;
        
        private void Start()
        {
            _compas.SetLeft(size);
            _compas.SetRight(size);
        }

        private void LateUpdate()
        {
            Vector3 forwardVector = Vector3.ProjectOnPlane(_cameraMotor.camera.transform.forward, Vector3.up);
            
            float forwardSignedAngle = Vector3.SignedAngle(forwardVector, Vector3.forward, Vector3.up);
            
            float compassOffset = (forwardSignedAngle / 180f) * size;
            
            _compas.anchoredPosition = new Vector3(compassOffset, 0);
        }
    }
}