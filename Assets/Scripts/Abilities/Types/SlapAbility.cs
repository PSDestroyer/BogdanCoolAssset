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
        hitBox.active = false; 
    }

    protected override IEnumerator C_Use()
    {
        hitBox.active = true;
        _animator.SetTrigger("Slap");
        yield return new WaitForSeconds(.7f);
        hitBox.active = false;
    }
}
