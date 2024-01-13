namespace State
{
    public class SleepingState : AbstractState
    {
        protected override int HungerThreshold { get { return 60; } }
        protected override int EnergyThreshold { get { return 50; } }

        public SleepingState(Agent agent) : base(agent) { }

        public override void Evaluate()
        {
            if (this.agent.Energy >= EnergyThreshold)
            {
                if (this.agent.Hunger >= HungerThreshold)
                {
                    this.agent.ChangeState(new HungryState(this.agent));
                }
                else this.agent.ChangeState(new IdleState(this.agent));
            }
            else
            {
                this.agent.Actions.Enqueue(new() { Action = () => this.agent.Sleep(), ActionName = "Sleep" });
            }
        }
    }
}
