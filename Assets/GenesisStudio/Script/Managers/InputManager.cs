using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenesisStudio
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : Singleton<InputManager>
    {
        #region Inspector

        [field: SerializeField] public InputActionAsset DefaultInputActionAsset { get; private set; }
        [field: SerializeField] public PlayerInput playerInput { get; private set; }

        #endregion

        #region Properties

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        public bool isJumping;

        public bool isSprinting;

        #endregion

        #region Private Fields

        private readonly Dictionary<string, Action<InputAction.CallbackContext>> _subscriptions
            = new Dictionary<string, Action<InputAction.CallbackContext>>();

        #endregion

        #region Unity Methods

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.actions = DefaultInputActionAsset;
        }

        #endregion

        #region New Input Methods

        public void Move(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
        }

        public void Look(InputAction.CallbackContext ctx)
        {
            LookInput = ctx.ReadValue<Vector2>();
        }

        public void Jump(InputAction.CallbackContext ctx)
        {
            isJumping = ctx.performed;
        }

        public void Sprint(InputAction.CallbackContext ctx)
        {
            isSprinting = ctx.performed;
        }

        private void Start()
        {
            Subscribe(Needs.Move, Move);
            Subscribe(Needs.Look, Look);
            Subscribe("Jump", Jump);
            Subscribe(Needs.Player_Sprint, Sprint);
        }

        #endregion

        #region Public Methods

        public void ChangeMap(string map)
        {
            playerInput.SwitchCurrentActionMap(map);
            Cursor.lockState = map == Needs.UIMap ? CursorLockMode.Confined : CursorLockMode.Locked;
        }

        public void Subscribe(string actionName, Action<InputAction.CallbackContext> callback)
        {
            InputAction action = playerInput.actions.FindAction(actionName);
            if (action == null)
            {
                Debug.LogWarning($"InputManager: Action '{actionName}' not found!");
                return;
            }

            if (_subscriptions.ContainsKey(actionName))
                Unsubscribe(actionName); // Prevent duplicate subscriptions

            _subscriptions[actionName] = callback;
            action.performed += callback;
            action.canceled += callback;
        }

        public void Unsubscribe(string actionName)
        {
            if (!_subscriptions.TryGetValue(actionName, out var callback))
                return;

            InputAction action = playerInput.actions.FindAction(actionName);
            if (action != null)
            {
                action.performed -= callback;
                action.canceled -= callback;
            }

            _subscriptions.Remove(actionName);
        }

        public void EnableAll()
        {
            foreach (var map in playerInput.actions.actionMaps)
                map.Enable();
        }

        public void DisableAll()
        {
            foreach (var map in playerInput.actions.actionMaps)
                map.Disable();
        }

        #endregion
    }
}
