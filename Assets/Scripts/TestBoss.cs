using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss : Boss
{
    protected override List<Func<IEnumerator>> Combo()
    {
        return new List<Func<IEnumerator>>
        {
            AttackCoroutine,
            HeavyAttack,
            CustomComboAction
        };
    }

    protected override IEnumerator AttackCoroutine()
    {
        player.Health -= Damage;
        print("Normal Attack");
        yield return new WaitForSeconds(0.5f);
    }

    protected override IEnumerator HeavyAttack()
    {
        player.Health -= Damage + 30;
        print("Heavy Attack");
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator CustomComboAction()
    {
        print("Custom Combo Action");
        yield return new WaitForSeconds(1f);
    }
}