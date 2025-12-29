using System;
using System.Collections;
using HalvaStudio.Save;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenesisStudio
{
    public class CameraMotor : MonoBehaviour
    {
        [field: SerializeField] public CinemachineCamera camera { get; private set; }
        [field: NonSerialized] public CharacterMotor Body;
        [SerializeField] private float sensivity;
        public float Sensivity
        {
            get => sensivity;
            set
            {
                sensivity = value;
                if (sensivity <= 0)
                {
                    sensivity = 0;
                }
                SaveManager.Instance.saveData.sensivity = sensivity;
            }
        } 
        private bool _rightShoulder;
        
        public Ray ray => new Ray(camera.transform.position, camera.transform.forward);
        public bool canRotate;
        
        private float xRotation;
        private Coroutine _changeFOVCoroutine;

        public void Initialize()
        {
            Sensivity = SaveManager.Instance.saveData.sensivity;
            canRotate = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        
        
        private void Update()
        {
            if (!canRotate) return;
            LookAround();
        }

        private void LookAround()
        {
            var input = InputManager.Instance.LookInput * Sensivity * Time.deltaTime;

            xRotation -= input.y;
            xRotation = Mathf.Clamp(xRotation, -70f, 70f);

            camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            Body.transform.Rotate(Vector3.up * input.x);
        }


        public void ChangeFOV(float fov)
        {
            if (_changeFOVCoroutine != null)
            {
                StopCoroutine(_changeFOVCoroutine);
            }

            _changeFOVCoroutine = StartCoroutine(ChangeCameraFOV(fov));
        }

        private IEnumerator ChangeCameraFOV(float to)
        {
            const float threshold = 0.01f; 
            const float speed = 10f;      

            while (Mathf.Abs(camera.Lens.FieldOfView - to) > threshold)
            {
                camera.Lens.FieldOfView = Mathf.Lerp(
                    camera.Lens.FieldOfView,
                    to,
                    Time.deltaTime * speed
                );
                yield return null;
            }

            camera.Lens.FieldOfView = to; 
            _changeFOVCoroutine = null;
        }

    }
}