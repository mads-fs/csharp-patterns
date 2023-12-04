namespace Factory
{
    /// <summary>
    /// This class is part of the Core for the Factory pattern. It allows
    /// for the polymorphism that the pattern makes use of. It is also 
    /// quite common to have Abstract classes together with Interfaces to 
    /// accomplish the same thing. The trick is that the Factory classes can
    /// produce new concrete objects but the code that needs to utilize the
    /// concrete classes only work with the abstracted version, not needing
    /// to understand or care about the underlying implementation.
    /// </summary>
    public interface IStorage
    {
        public void Save();
        public void Load();
    }
}
