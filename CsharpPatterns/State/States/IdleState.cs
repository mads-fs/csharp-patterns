namespace State
{
    public class IdleState : AbstractState
    {
        protected override int HungerThreshold { get { return 50; } }
        protected override int EnergyThreshold { get { return 0; } }

        public IdleState(Agent agent) : base(agent) { }
        public override void Evaluate()
        {
            if (this.agent.Hunger >= HungerThreshold)
            {
                this.agent.ChangeState(new HungryState(this.agent));
            }
            else
            {
                this.agent.Actions.Enqueue(new() { Action = () => this.agent.Idle(), ActionName = "Idle" });
            }
        }
    }
}
