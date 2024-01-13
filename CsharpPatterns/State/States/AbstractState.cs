namespace State
{
    /// <summary>
    /// This is the bread and butter of the State pattern. In this case we have a very simple
    /// abstract class that knows about Hunger, Energy and that it needs to evaluate agent state
    /// in order to decide what to do next. Each type of state then implements this logic and the
    /// <see cref="Agent"/> class can then call Evaluate on it's state object. It doesn't need
    /// to know what the state object is, I just needs to call <see cref="Evaluate"/>.
    /// </summary>
    public abstract class AbstractState
    {
        protected abstract int HungerThreshold { get; }
        protected abstract int EnergyThreshold { get; }

        protected Agent agent;
        protected AbstractState(Agent agent)
        {
            this.agent = agent;
            this.agent.Actions.Clear();
        }

        public abstract void Evaluate();
    }
}