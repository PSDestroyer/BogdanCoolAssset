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
                if (_isTPS)
                {
                    _inputAxisController.Controllers[0].Input.Gain = value;
                    _inputAxisController.Controllers[1].Input.Gain = -value;
                    return;
                }
                
                if (value <= 0)
                {
                    SaveManager.Instance.saveData.sensivity = 0;
                    return;
                }
                SaveManager.Instance.saveData.sensivity = value;
            }
        } 

        private CinemachineInputAxisController _inputAxisController;
        private CinemachineRotationComposer _rotationComposer;
        private bool _rightShoulder;
        
        public Ray ray => new Ray(camera.transform.position, camera.transform.forward);
        public bool canRotate { get; set; }
        private bool _isTPS;
        
        private float xRotation;
        private Coroutine _changeFOVCoroutine;

        public void Initialize()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _isTPS = Body.IsTPS;

            if (_isTPS)
            {
                _inputAxisController = GetComponent<CinemachineInputAxisController>();
                _rotationComposer = GetComponent<CinemachineRotationComposer>();
                _rightShoulder = true;
                Sensivity = SaveManager.Instance.saveData.sensivity == 0 ? 25f :  SaveManager.Instance.saveData.sensivity;
            }
        }

        
        
        private void Update()
        {
            if (!canRotate) return;
            LookAround();
        }

        private void LookAround()
        {
            if(_isTPS) {return;}
            
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
        
        public void ChangeShoulder(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                _rightShoulder = !_rightShoulder;
                _rotationComposer.TargetOffset.x = _rightShoulder ? .5f : -0.5f;
            }
        }

    }
}