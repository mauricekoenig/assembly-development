using System;

namespace GameEngine
{
    internal static class UnitActionFactory
    {
        internal static UnitAction Create(
            UnitActionDefinition definition,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            UnitAction action =
                new UnitAction(
                    definition.Id,
                    definition.Name,
                    definition.Description);

            if (definition.Effects == null)
                return action;

            foreach (ActionEffectDefinition effectDefinition
                     in definition.Effects)
            {
                action.AddEffect(
                    EffectFactory.CreateActionEffect(
                        effectDefinition,
                        contentRegistry));
            }

            return action;
        }

        internal static UnitAction CreateFromEffectRecipe(
            EffectDefinition definition,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            if (definition.Components == null ||
                definition.Components.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Itemizable Effect '{definition.Id}' has no Components.");
            }

            if (!definition.CanBeItemized ||
                definition.Itemization == null)
            {
                throw new InvalidOperationException(
                    $"Effect '{definition.Id}' has no itemization target.");
            }

            UnitAction action =
                new UnitAction(
                    $"unitaction:recipe:{definition.Id}",
                    definition.Name,
                    definition.Description);

            action.AddEffect(
                new ActionEffect(
                    contentRegistry.CreateEffect(
                        definition.Id),
                    definition.Itemization.Target));

            return action;
        }
    }
}
