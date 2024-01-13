#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace State
{
    /// <summary>
    /// This class represents a typical Vector2 struct that only works with Integers
    /// for simplicity's sake and because the Maps grid only supports integer coordinates.
    /// This implementation goes slightly beyond the scope of what the sample needs
    /// however it allows for more experimentation should you wish to expand and play
    /// with the sample code. This example could also have used the .NET
    /// <see cref="System.Numerics.Vector"/> types instead of this implementation.
    /// </summary>
    public readonly struct Vector2Int : IEquatable<Vector2Int>
    {
        public readonly int X;
        public readonly int Y;

        public static Vector2Int Zero => new();
        public static Vector2Int MinusOne => new(-1, -1);

        public Vector2Int() { X = 0; Y = 0; }
        public Vector2Int(int x, int y) { X = x; Y = y; }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
            => new(Math.Min(a.X + b.X, Program.World.XMax), Math.Min(a.Y + b.Y, Program.World.YMax));
        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
            => new(Math.Max(a.X - b.X, 0), Math.Max(a.Y - b.Y, 0));
        public static Vector2Int operator *(Vector2Int a, int scalar)
            => new(Math.Clamp(a.X * scalar, 0, Program.World.XMax), Math.Clamp(a.Y * scalar, 0, Program.World.YMax));
        public static bool operator ==(Vector2Int a, Vector2Int b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2Int a, Vector2Int b) => a.X != b.X || a.Y != b.Y;

        public override string ToString() => $"{this.X},{this.Y}";
        public override int GetHashCode() => HashCode.Combine(X, Y);

        public override bool Equals(object? obj)
        {
            if (obj is Vector2Int other) return other == this;
            return false;
        }
        public bool Equals(Vector2Int other) => other == this;
    }
}
