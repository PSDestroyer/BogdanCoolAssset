namespace GenesisStudio
{
    public abstract class State
    {
        public StateMachine _stateMachine { get; set; }
        public AIBrain _brain { get; set; }

        public abstract void Enter();
        public abstract void Perform();
        public abstract void Exit();
    }
}