namespace GameEngine
{
    // Pure mechanical component. WHEN and WHO are supplied by the wrapper
    // that uses the Effect (ActionEffect / TriggeredEffect).
    public sealed class EffectComponentDefinition
    {
        public EffectActionType Action { get; set; }

        public int Amount { get; set; }

        public DamageType DamageType { get; set; }
    }
}
