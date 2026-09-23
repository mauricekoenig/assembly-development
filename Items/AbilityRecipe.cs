namespace GameEngine
{
    public sealed class AbilityRecipe :
        ConsumableItem
    {
        internal ActiveAbility Ability { get; }


        internal AbilityRecipe(
            ActiveAbility ability)
            : base(
                GetName(ability),
                GetDescription(ability))
        {
            Guard.NotNull(ability, nameof(ability));

            Ability = ability;
        }


        private static string GetName(
            ActiveAbility ability)
        {
            Guard.NotNull(ability, nameof(ability));

            return $"Recipe: {ability.Name}";
        }


        private static string GetDescription(
            ActiveAbility ability)
        {
            Guard.NotNull(ability, nameof(ability));

            return $"Grants the Active Ability {ability.Name} to a Unit.";
        }
    }
}