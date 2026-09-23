namespace GameEngine
{
    public sealed class ActionEffect
    {
        public Effect Effect { get; }

        public EffectTarget Target { get; }

        internal ActionEffect(
            Effect effect,
            EffectTarget target)
        {
            Guard.NotNull(
                effect,
                nameof(effect));

            Effect =
                effect;

            Target =
                target;
        }
    }
}
