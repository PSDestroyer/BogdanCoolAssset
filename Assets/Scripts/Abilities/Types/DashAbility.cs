using System.Collections;
using UnityEngine;

public class DashAbility : Ability
{
    bool canDash
    {
        get => _controller.CanDash;
        set => _controller.CanDash = value;
    }

    protected override void Initialize()
    {
        canDash = true;
        _controller.DashCooldown = Data.Cooldown;
        
    }

    protected override IEnumerator Action()
    {
        _controller.Dash();
        yield return null;
    }
}
