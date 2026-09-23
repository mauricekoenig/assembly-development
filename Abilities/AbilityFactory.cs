using System;

namespace GameEngine
{
    public static class AbilityFactory
    {
        internal static ActiveAbility Create(
            AbilityDefinition definition,
            ContentRegistry contentRegistry)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (contentRegistry == null)
            {
                throw new ArgumentNullException(
                    nameof(contentRegistry));
            }

            ActiveAbility ability =
                new ActiveAbility(
                    definition.Name,
                    definition.Description,
                    definition.SelectionType,
                    definition.CooldownRounds,
                    definition.SelectionRules);

            if (definition.Effects == null)
                return ability;

            foreach (ActionEffectDefinition effectDefinition
                     in definition.Effects)
            {
                ability.AddEffect(
                    EffectFactory.CreateActionEffect(
                        effectDefinition,
                        contentRegistry));
            }

            return ability;
        }
    }
}
