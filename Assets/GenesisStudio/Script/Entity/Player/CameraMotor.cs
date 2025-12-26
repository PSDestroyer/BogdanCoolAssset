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
        public float Sensivity
        {
            get => SaveManager.Instance.saveData.sensivity;
            set
            {
                if (value <= 0)
                {
                    SaveManager.Instance.saveData.sensivity = 0;
                    return;
                }
                SaveManager.Instance.saveData.sensivity = value;
            }
        } 
        private bool _rightShoulder;
        
        public Ray ray => new Ray(camera.transform.position, camera.transform.forward);
        public bool canRotate { get; set; }
        
        private float xRotation;
        private Coroutine _changeFOVCoroutine;

        public void Initialize()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        
        
        private void Update()
        {
            if (!canRotate) return;
            LookAround();
        }

        private void LookAround()
        {
            var input = InputManager.Instance.LookInput * Sensivity * 100 * Time.deltaTime;

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