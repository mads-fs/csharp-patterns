namespace Factory
{
    /// <summary>
    /// This is part of the Core of the Factory Pattern. With this class you can produce
    /// concrete classes that implements the <see cref="IStorage"/> interface and pass
    /// them back to the requesting code without that code needing to know anything about
    /// the underlying implementation. The Factory is the only class that needs to know how
    /// to construct the class or what concrete class is even passed to the request. This
    /// brings down coupling between your classes as the requester only needs to know the
    /// interface and none of the concrete classes.
    /// </summary>
    public static class StorageFactory
    {
        public static IStorage? Create(Platform platformType) => platformType switch
        {
            Platform.PC => new PCStorage(),
            Platform.Console => new ConsoleStorage(),
            Platform.Mobile => new MobileStorage(),
            _ => null,
        };
    }
}