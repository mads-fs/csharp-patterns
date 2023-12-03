namespace Mediator
{
    /// <summary>
    /// This is the core of this Mediator implementation. Any class can subscribe
    /// to any type of event. This means that you no longer have to work out how
    /// to propagate state changes throughout your systems by tying them to each
    /// other through code. Each system is responsible for knowing *what* they want 
    /// to be told about and what events they can express while the <see cref="GameEvents"/>
    /// class will make brodcasting and event propagation possible.
    /// </summary>
    public static class GameEvents
    {
        public static event Action<DamageInfo> DamageTaken = delegate { };
        public static event Action<Guid, Guid> Death = delegate { };
        public static event Action<HealingInfo> HealingReceived = delegate { };
        public static event Action<Guid, ItemInfo, uint> ItemReceived = delegate { };

        public static void OnDamage(DamageInfo info) => DamageTaken.Invoke(info);
        public static void OnDeath(Guid killer, Guid target) => Death.Invoke(killer, target);
        public static void OnHealing(HealingInfo info) => HealingReceived.Invoke(info);
        public static void OnItemReceived(Guid receiver, ItemInfo info, uint amount) => ItemReceived.Invoke(receiver, info, amount);
    }

    /// <summary>
    /// This class merely exists to show that different types of events easily could
    /// be split out into separate appropriate classes for easier maintainability, 
    /// extendability and less coupling.
    /// </summary>
    public static class SystemEvents
    {
        public static event Action GameEnd = delegate { };

        public static void OnGameEnd() => GameEnd.Invoke();
    }
}