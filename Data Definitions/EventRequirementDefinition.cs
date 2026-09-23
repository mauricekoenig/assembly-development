namespace GameEngine
{
    public sealed class EventRequirementDefinition
    {
        public EventRequirementType Type { get; set; }

        public int Amount { get; set; }

        public UnitTag? UnitTag { get; set; }
    }
}
