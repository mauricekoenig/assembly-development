namespace GameEngine
{
    public sealed class TriggeredEffectDefinition
    {
        public string EffectId { get; set; }

        public EffectTrigger Trigger { get; set; }

        public EffectTarget Target { get; set; }
    }
}
