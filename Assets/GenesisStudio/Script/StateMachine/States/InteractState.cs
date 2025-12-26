using System.Threading.Tasks;
using UnityEngine;

namespace GenesisStudio
{
    public class InteractState : State
    {
        public override void Enter()
        {
            Break(Random.Range(4, 10));
        }

        public override void Perform()
        {
        }

        public override void Exit()
        {
        }
        
        private async void Break(int time)
        {
            await Task.Delay(time * 1000);
            _brain.isBusy = false;
            _stateMachine.ChangeState(_stateMachine.patrolState);
        }
    }
}