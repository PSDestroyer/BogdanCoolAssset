using GenesisStudio;
using UnityEngine;

public class ChaseTargetState : State
{
    Transform target;
    Transform chaseTarget;
    Path path;
    public ChaseTargetState(Transform chaseTarget, Transform target, Path nextPath = null)
    {
        this.target = target;
        this.path = nextPath;
        this.chaseTarget = chaseTarget;
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {
        if (!QuestManager.Instance.IsActiveQuest) return;
        if (QuestManager.Instance.ActiveQuest is FetchQuest fq)
            fq.Complete();
    }

    public override void Perform()
    {
        if(!_brain.ReachedTarget(target))
        {
            _brain.MoveInstantly(chaseTarget.position);
            _brain.Agent.isStopped = _brain.Agent.remainingDistance < 2f;
            _brain.Sprint(_brain.Agent.remainingDistance > 10f);
        } 
        else
        {
            if(path != null)
            {
                _brain.stateMachine.ChangeState(new FollowThePathState(path));
                return;
            }
            _brain.Move(target.position);
            _brain.stateMachine.ChangeState(new IdleState());
        }
    }
}