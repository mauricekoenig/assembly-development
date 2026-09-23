namespace GameEngine
{
    public sealed class PassiveRecipe :
        ConsumableItem
    {
        internal Passive Passive { get; }


        internal PassiveRecipe(
            Passive passive)
            : base(
                GetName(passive),
                GetDescription(passive))
        {
            Guard.NotNull(passive, nameof(passive));

            Passive = passive;
        }


        private static string GetName(
            Passive passive)
        {
            Guard.NotNull(passive, nameof(passive));

            return $"Recipe: {passive.Name}";
        }


        private static string GetDescription(
            Passive passive)
        {
            Guard.NotNull(passive, nameof(passive));

            return
                $"Grants the Passive {passive.Name} to a Unit.";
        }
    }
}