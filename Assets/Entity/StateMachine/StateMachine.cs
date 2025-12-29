using System;
using UnityEngine;

namespace GenesisStudio
{
    public class StateMachine : MonoBehaviour
    {
        public State activeState { get; private set; }
        public State lastState { get; private set; }
        public PatrolState patrolState { get; private set; }
        public IdleState idleState{ get; private set; }
        public InteractState interactState{ get; private set; }
        
        public void Initialize(State initState)
        {
            patrolState = new PatrolState();
            idleState = new IdleState();
            interactState = new InteractState();
            ChangeState(initState);
        }

        private void FixedUpdate()
        {
            activeState?.Perform();
        }

        public void ChangeState(State newState)
        {
            lastState = activeState;
            activeState?.Exit();
            activeState = newState;

            if (activeState != null)
            {
                activeState._stateMachine = this;
                activeState._brain = GetComponent<AIBrain>();
                activeState.Enter();
                Debug.Log($"<color=green>StateMachine</color>: Entered {activeState}");
            }
        }
    }
}