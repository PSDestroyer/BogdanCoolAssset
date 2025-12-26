using GenesisStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Fetch", menuName = "Genesis Studio/Mission/Fetch Quest")]
public class FetchQuest : Quest
{
    NPC target;
    Transform to;
    GameObject indicator;

    public override bool IsAlreadyCompleted()
    {
        return target.transform.IsNearThePoint(to);
    }

    public override void OnComplete()
    {
        target = null;
        to = null;
        Destroy(indicator);
    }

    public override void OnInitialize(QuestParams @params)
    {
        target = @params.Target_npc;
        to = @params.Target_point;
        indicator = to.AddIndicator(GameManager.Instance.indicator_height, GameManager.Instance.indicator_color);
        target.stateMachine.ChangeState(new ChaseTargetState(chaseTarget: GameManager.Instance.Player.transform, target: to)); 
    }
}