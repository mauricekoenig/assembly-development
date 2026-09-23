namespace GameEngine
{
    // Itemization metadata is deliberately separate from Effect mechanics.
    // It only describes how a raw Effect is wrapped when an Effect Recipe
    // grants it as a run-added UnitAction.
    public sealed class EffectItemizationDefinition
    {
        public EffectTarget Target { get; set; }
    }
}
