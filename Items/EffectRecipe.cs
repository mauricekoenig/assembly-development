namespace GameEngine
{
    // The item name stays EffectRecipe because Effect Slots remain the
    // progression resource. Consuming this item grants a UnitAction,
    // never a raw Effect to the Unit.
    public sealed class EffectRecipe :
        ConsumableItem
    {
        internal UnitAction Action { get; }


        internal EffectRecipe(
            UnitAction action)
            : base(
                GetName(action),
                GetDescription(action))
        {
            Guard.NotNull(
                action,
                nameof(action));

            Action =
                action;
        }


        private static string GetName(
            UnitAction action)
        {
            Guard.NotNull(
                action,
                nameof(action));

            return $"Recipe: {action.Name}";
        }


        private static string GetDescription(
            UnitAction action)
        {
            Guard.NotNull(
                action,
                nameof(action));

            return
                $"Grants the UnitAction {action.Name} to a Unit.";
        }
    }
}
