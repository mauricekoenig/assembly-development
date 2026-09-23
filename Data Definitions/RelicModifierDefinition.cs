namespace GameEngine
{
    public sealed class RelicModifierDefinition
    {
        public ModifierType Type { get; set; }

        public ModifierOperation Operation { get; set; }

        public int Amount { get; set; }

        public DamageType? DamageType { get; set; }

        public UnitTag? SourceUnitTag { get; set; }

        public UnitTag? TargetUnitTag { get; set; }
    }
}
