using System;
using System.Collections;
using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;

public class SlapAbility : Ability
{
    private HitBox hitBox;
    
    
    protected override void Initialize()
    {
        Debug.Break();
        hitBox = _controller.hitBox;
        // InputManager.Instance.Subscribe(Needs.Fire, Use);Use
        hitBox.damage = Damage;
    }

    protected override IEnumerator C_Use()
    {
        _animator.SetTrigger("Slap");
        yield return null;
    }
}
