namespace GameEngine
{
    public sealed class PassiveModifierDefinition
    {
        public ModifierType Type { get; set; }

        public ModifierOperation Operation { get; set; }

        public int Amount { get; set; }

        public DamageType? DamageType { get; set; }
    }
}