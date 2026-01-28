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
        hitBox = _controller.hitBox;
        // InputManager.Instance.Subscribe(Needs.Fire, Use);Use
        hitBox.damage = Damage;
    }

    protected override IEnumerator Action()
    {
        _animator.SetTrigger("Slap");
        yield return null;
    }
}
