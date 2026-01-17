using System.Collections;
using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;

public class SlapAbility : Ability
{
    public HitBox hitBox;
    
    
    protected override void Initialize()
    {
        InputManager.Instance.Subscribe(Needs.Fire, Use);
        hitBox.damage = Damage;
    }

    protected override IEnumerator C_Use()
    {
        _animator.SetTrigger("Slap");
        yield return null;
    }
}
