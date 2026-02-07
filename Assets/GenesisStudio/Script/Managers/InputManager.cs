using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenesisStudio
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : Singleton<InputManager>
    {
        public class GlobalActions
        {
            InputActionMap _actionMap;
            private readonly Dictionary<string, Action<InputAction.CallbackContext>> _subscriptions
                = new Dictionary<string, Action<InputAction.CallbackContext>>();

            public GlobalActions(InputActionMap actionMap)
            {
                _actionMap = actionMap;
                
                actionMap.Enable();
            }

            public void Subscribe(string actionName, Action<InputAction.CallbackContext> callback)
            {
                InputAction action = _actionMap.FindAction(actionName);
                string key = Key(action);
                
                if (_subscriptions.ContainsKey(actionName))
                    Unsubscribe(actionName);
                
                _subscriptions[key] = callback;
                
                action.performed += callback;
                action.canceled += callback;
            }
            
            public void Unsubscribe(string actionName)
            {
                if (!_subscriptions.TryGetValue(actionName, out var callback))
                    return;
                
                InputAction action = _actionMap.FindAction(actionName);
                string key = Key(action);

                if (action != null)
                {
                    action.performed -= callback;
                    action.canceled -= callback;
                }

                _subscriptions.Remove(key);
            }
        
            string Key(InputAction action) => action.id.ToString();
        }
        
        #region Inspector

        [field: SerializeField] public InputActionAsset DefaultInputActionAsset { get; private set; }
        [field: SerializeField] public PlayerInput playerInput { get; private set; }

        #endregion

        #region Properties

        public GlobalActions Global { get; private set; }
        
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
            
            Subscribe(Needs.Move, Move);
            Subscribe(Needs.Look, Look);
            Subscribe("Jump", Jump);
            
            Global = new GlobalActions(playerInput.actions.FindActionMap("Global"));
            // Subscribe(Needs.Player_Sprint, Sprint);
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

        #endregion

        #region Public Methods

        public void ChangeMap(string map)
        {
            foreach (var actionMap in playerInput.actions.actionMaps)
            {
                if (actionMap.name == "Global") continue;
                actionMap.Disable();
            }
            
            playerInput.actions.FindActionMap(map)?.Enable();
            
            Cursor.lockState = map == Needs.UIMap 
                ? CursorLockMode.Confined 
                : CursorLockMode.Locked;
        }

        public void Subscribe(string actionName, Action<InputAction.CallbackContext> callback)
        {
            InputAction action = playerInput.actions.FindAction(actionName);
            if (action == null)
            {
                Debug.LogWarning($"InputManager: Action '{actionName}' not found!");
                return;
            }

            string key = Key(action);

            if (_subscriptions.ContainsKey(actionName))
                Unsubscribe(actionName); // Prevent duplicate subscriptions

            _subscriptions[key] = callback;
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

            _subscriptions.Remove(Key(action));
        }
        
        string Key(InputAction action) => action.id.ToString();

        private void OnDisable()
        {
            UnsubscribeAll();
        }
        
        private void UnsubscribeAll()
        {
            foreach (var pair in _subscriptions)
            {
                string actionId = pair.Key;
                var callback = pair.Value;

                InputAction action = FindActionById(actionId);
                if (action == null)
                    continue;

                action.performed -= callback;
                action.canceled  -= callback;
            }

            _subscriptions.Clear();
        }
        
        private InputAction FindActionById(string id)
        {
            foreach (var map in playerInput.actions.actionMaps)
            {
                var action = map.FindAction(new Guid(id));
                if (action != null)
                    return action;
            }
            return null;
        }

        #endregion
        
        
    }
}
