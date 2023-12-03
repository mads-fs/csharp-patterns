namespace Command
{
    /// <summary>
    /// This is the core of the pattern. This interface represents anything
    /// that needs to be executed some time in the future. Typically as part
    /// of a chain of other <see cref="ICommand"/> objects. This interface can
    /// be as complex or as simple as your use-case calls for, but what is key
    /// to the pattern is the Execute method or a similar way of Invocation.
    /// </summary>
    public interface ICommand
    {
        public void Execute();
    }
}