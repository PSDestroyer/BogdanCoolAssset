using System.Collections;
using Abilities;
using GenesisStudio;
using UnityEngine;
using UnityEngine.InputSystem;

public class BombAbility : Ability
{
    
    public float force;
    public float height;
    private bool _isAiming;   
    
    ShootController _shooter;
    
    protected override void Initialize()
    { 
        _shooter = _controller.shootController;
        
        _shooter.force = force;
        _shooter.height = height;
        
        // InputManager.Instance.Subscribe(Needs.Fire, Use);
    }

    public override void Use(InputAction.CallbackContext context)
    {
        _isAiming = context.performed;
        
        if(context.canceled)
            base.Use(context);
    }

    protected override IEnumerator C_Use()
    {
        _animator.SetTrigger("Throw");
        yield return null;
        
        
        
    }
}
