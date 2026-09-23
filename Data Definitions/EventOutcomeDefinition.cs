namespace GameEngine
{
    public sealed class EventOutcomeDefinition
    {
        public EventOutcomeType Type { get; set; }

        public int Amount { get; set; }

        public ItemReference Item { get; set; }

        public RewardDefinition Reward { get; set; }
    }
}
