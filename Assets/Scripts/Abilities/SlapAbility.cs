using System.Collections;
using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;

public class SlapAbility : Ability
{
    protected override void Initialize()
    {
        InputManager.Instance.Subscribe(Needs.Fire, Use);
    }

    protected override IEnumerator C_Use()
    {
        _animator.SetTrigger("Slap");
        yield return null;
    }
}
