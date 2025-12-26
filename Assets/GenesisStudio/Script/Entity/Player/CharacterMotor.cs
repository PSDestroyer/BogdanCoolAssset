using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.Windows.Speech;

namespace GenesisStudio
{
    public class CharacterMotor : Brain
    {
        [SerializeField] protected CameraMotor _cameraMotor;
        public CameraMotor CameraMotor => _cameraMotor;
        public CharacterController Controller { get; private set; }

        [Header("TPS Settings")] 
        [field: SerializeField] public bool IsTPS { get; private set; }
        
        
        
        [Header("FPS Settings")]
        
        [SerializeField, Range(60f, 120f)] private float NormalFov;
        [SerializeField, Range(60f, 120f)] private float SpeedFov;

        private float _groundCheckDistance;
        private float _jumpForce;
        private float _gravity;
        private Vector2 _moveInput;
        private Vector3 _velocity;
        private int _index;
        private bool _isGrounded;
        private bool _wantsToJump;
        private bool _canMove;
        private bool _canRotate;

        protected bool _isMoving;
        protected InputManager _input;
        protected float _interactDistance;

        protected override void Initialize()
        {
            _input.Subscribe(Needs.Player_Sprint, Sprint);
            _input.Subscribe(Needs.Player_Select, ChangeItem);
            _cameraMotor.Body = this;
            _gravity = -5f;
            _groundCheckDistance = 1.1f;
            _jumpForce = 1.2f;
            _interactDistance = 3f;
            _canMove = true;  
            _cameraMotor.Initialize();
            
            if (IsTPS)
            {
                _input.Subscribe(Needs.Use, _cameraMotor.ChangeShoulder);
            }
        }

        public void Sprint(InputAction.CallbackContext ctx)
        {
            if (!_isMoving) return;
            if (ctx.performed)
            {
                Speed = 8f;
                ChangeFOV(SpeedFov);
            }
             if (ctx.canceled)
            {
                Speed = 5f;
                ChangeFOV(NormalFov); 
            }
        }



        private void ChangeFOV(float value)
        {
            CameraMotor.ChangeFOV(value);
        }

        private void Update()
        {
            _moveInput = _input.MoveInput;
            _isMoving = _moveInput != Vector2.zero;
            var dir = transform.right * _moveInput.x + transform.forward * _moveInput.y + new Vector3(0, -2, 0);

            _isGrounded = Physics.Raycast(transform.position, -transform.up, out var hit, _groundCheckDistance);
            _wantsToJump = _input.isJumping;

            if (_isGrounded && _velocity.y < 0)
                _velocity.y = -2f;

            if (_wantsToJump && _isGrounded)
                _velocity.y = Mathf.Sqrt(_jumpForce * -2 * _gravity);

            _velocity.y += _gravity * Time.deltaTime;

            Move(dir + _velocity);

            if (IsTPS)
            {
                if (_isMoving)
                {
                    transform.Rotate(Vector3.up * _input.LookInput.x * _cameraMotor.Sensivity * Time.deltaTime);
            
                    Quaternion cameraRotation = _cameraMotor.transform.rotation;
                    cameraRotation.x = 0;
                    cameraRotation.z = 0;
            
                    transform.rotation = Quaternion.Lerp(transform.rotation, cameraRotation, 0.1f);
                }
            }

        }

        private void Awake()
        {
            _cameraMotor.Body = this;
            _input = InputManager.Instance;
            Controller = GetComponent<CharacterController>();
        }
        

        public override void Move(Vector3 direction)
        {
            if(!_canMove) return;
            Controller.Move(direction * Speed * Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawRay(transform.position, -transform.up * _groundCheckDistance);
        }

        public override void SetPath(Path path)
        {
            if (path == null)
                throw new ArgumentNullException("Path is null");

            StartCoroutine(SetPoints(path));
        }
        public IEnumerator SetPoints(Path path)
        {
            Queue<Transform> pointQueue = new Queue<Transform>(path.points);
            while (pointQueue.Count > 0)
            {
                var target = pointQueue.Dequeue();
                var indicator = target.AddIndicator(GameManager.Instance.indicator_height, GameManager.Instance.indicator_color);

                while (Vector3.Distance(transform.position, target.position) > GameManager.Instance.indicator_arriveRange)
                {
                    yield return null;
                }
                if (path.points.IndexOf(target) == path.points.Count - 1)
                {
                    path.onLastPointArrivedForPlayer?.Invoke();
                }

                Destroy(indicator);
            }
        }

        public void Input_UseItem(InputAction.CallbackContext ctx)
        {
            if(ctx.started)
                UseItem(GetSelectedItem());
        }

        public void UseItem(Inventory.ItemInfo info)
        {
            if (info == null) return;
            var data = info.Data;
            if (!data.IsUsable) return;
            switch (data.ItemType)
            {
                case ItemType.Heal:
                    Health.Heatlh += data.HealAmount;
                    break;
                case ItemType.Key:
                    if(Physics.Raycast(CameraMotor.camera.transform.position, CameraMotor.camera.transform.forward, out var hit, _interactDistance))
                    {
                        if(hit.transform.gameObject.TryGetComponent(out Door door))
                        {
                            door.TryUnlock(data);
                        }
                    }
                    break;
                case ItemType.Placeable:
                    break;
            }

            if(data.RemoveAfterUse)
                Inventory.RemoveItem(data);
            Debug.Log($"Use {info.Data.ItemName}");
        }

        public void ChangeItem(InputAction.CallbackContext ctx)
        {
            if (Inventory.CurrentItems != 0)
            {
                _index += (int)ctx.ReadValue<Vector2>().x;
                int index = _index % Inventory.CurrentItems;
                if(index < 0) index = 0;
                if(index > Inventory.CurrentItems) index = Inventory.CurrentItems;
                SelectItem(index);
                Debug.Log($"Selected Item {index}");
            }
        }


        public override void AddItem(ItemData item)
        {
            Inventory.AddItem(item);
            if (!QuestManager.Instance.IsActiveQuest) return;
            if(QuestManager.Instance.ActiveQuest is CollectableQuest cq)
                cq.OnItemCollected(item);
        }

        public override void AddItem(ItemData item, int amount)
        {
            Inventory.AddItems(item, amount);
        }

        public override void OnGiveItem(ItemData item)
        {
            
        }


        public override void RemoveItem(ItemData item)
        {
            Inventory.RemoveItem(item);
            if (!QuestManager.Instance.IsActiveQuest) return;
            if (QuestManager.Instance.ActiveQuest is DeliverQuest dq)
                dq.OnItemDelivered(item);
        }

        public override void RemoveItem(ItemData item, int amount)
        {
            Inventory.RemoveItems(item, amount);
            
        }

        public override bool HasItem(ItemData item)
        {
            return Inventory.HasItem(item); 
        }


        public override void SelectItem(int index)
        {
            Inventory.SelectAt(index);
        }

        public override void SelectItem(ItemData data)
        {
             Inventory.Select(data);
        }

        public void Controls(bool value)
        {
            _canMove = value;
            CameraMotor.canRotate = value;
        }

        
    }
}
