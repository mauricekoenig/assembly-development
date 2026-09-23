namespace GameEngine
{
    public sealed class TriggeredEffect
    {
        public Effect Effect { get; }

        public EffectTrigger Trigger { get; }

        public EffectTarget Target { get; }

        internal TriggeredEffect(
            Effect effect,
            EffectTrigger trigger,
            EffectTarget target)
        {
            Guard.NotNull(
                effect,
                nameof(effect));

            Effect =
                effect;

            Trigger =
                trigger;

            Target =
                target;
        }
    }
}
