using System;
using GenesisStudio;
using UnityEngine;
using UnityEngine.InputSystem;
using PlatformCharacterController;

[RequireComponent(typeof(MovementCharacterController))]
public class GrabLogic : MonoBehaviour
{
    public Transform hands;

    private MovementCharacterController _player;
    private IGrabable _current;
    
    private void Start()
    {
        _player = GetComponent<MovementCharacterController>();
        InputManager.Instance.Subscribe(Needs.Interact, CheckForGrab);
    }

    public void CheckForGrab(InputAction.CallbackContext context)
    {
        if(context.canceled)
        {
            if (_current != null)
            {
                ReleaseCurrent();
                return;
            }


            if (Physics.Raycast(_player.Ray, out RaycastHit hit, 4f))
            {
                if (hit.collider.TryGetComponent(out IGrabable grabable))
                {
                    Grab(grabable);
                }
            }
        }
    }

    private void ReleaseCurrent()
    {
        _current.Release();
        _current = null;
    }

    private void Grab(IGrabable grabable)
    {
        _current = grabable;
        _current.Grab(hands);
    }
    
    
}