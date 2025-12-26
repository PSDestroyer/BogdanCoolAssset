using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;


namespace GenesisStudio.Shooter
{
    public class ShooterMotor : MonoBehaviour
    {
        private Player _player;
        private AIBrain _aiBrain;
        private bool _isAI;
        
        [Header("IK")]
        [SerializeField] private TwoBoneIKConstraint leftHandIKConstraint;
        [SerializeField] private Transform leftHandTarget;
        [SerializeField] private TwoBoneIKConstraint rightHandIKConstraint;
        [SerializeField] private Transform rightHandTarget;

        private Transform _leftTarget, _rightTarget;
        
        private Weapon _activeWeapon;
        private Animator _animator;
        
        public void Initialize(Brain motor)
        {
            
            if (motor == null)
                throw new Exception($"There is no brain on {gameObject.name}");

            if (motor.Contains<BodyAnimation>(out var bodyAnimation))
            {
                _animator = bodyAnimation.Animator;
            }

            switch (motor)
            {
                case Player player:
                    _player = player;
                    break;
                case AIBrain aiBrain:
                    _aiBrain = aiBrain;
                    break;
            }

            _isAI = _aiBrain != null;
        }

        private void Update()
        {
            if (!_isAI)
            {
                if(_leftTarget != null)
                    leftHandTarget.position = _leftTarget.position;
                
                if(_rightTarget != null)
                    rightHandTarget.position = _rightTarget.position;
            }
        }

        public void Equip(Weapon item)
        {
            item.transform.parent = _player.hands.RightHand;
            item.transform.position = _player.hands.RightHand.position;
            item.transform.rotation = _player.hands.RightHand.rotation;
            
            leftHandIKConstraint.weight = 1;
            rightHandIKConstraint.weight = 1;
            
            _leftTarget = item.LeftHandPosition;
            _rightTarget = item.RightHandPosition;
            _animator.SetBool("Pistol", true);
            _activeWeapon = item;
        }

        public void Unequip()
        {
            leftHandIKConstraint.weight = 0;
            rightHandIKConstraint.weight = 0;
        }
        
    }
}