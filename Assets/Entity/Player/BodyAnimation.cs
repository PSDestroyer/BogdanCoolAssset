using UnityEngine;
using UnityEngine.InputSystem;

namespace GenesisStudio
{
    public class BodyAnimation : MonoBehaviour
    {
        [SerializeField] Animator _bodyAnimator;
        InputManager _input;
        int velocityX;
        int velocityZ;
            
        public Animator Animator => _bodyAnimator;
        
        float speed => InputManager.Instance.isSprinting ? 1f : 0.5f;

        private void Awake()
        {
            _input = InputManager.Instance;
            velocityX = Animator.StringToHash("VelX");
            velocityZ = Animator.StringToHash("VelZ");
        }

        private void Update()
        {
            _bodyAnimator.SetFloat(velocityX, _input.MoveInput.x * speed, 0.2f, Time.deltaTime);
            _bodyAnimator.SetFloat(velocityZ, _input.MoveInput.y * speed, 0.2f, Time.deltaTime);
        }

    }
}