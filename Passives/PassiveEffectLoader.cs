using System;
using System.Collections.Generic;

namespace GameEngine
{
    internal static class PassiveEffectLoader
    {
        internal static void AddEffects(
            Passive passive,
            PassiveDefinition definition,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(
                passive,
                nameof(passive));

            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            if (definition.Effects == null ||
                definition.Effects.Count == 0)
            {
                return;
            }

            HashSet<string> addedBindings =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (TriggeredEffectDefinition effectDefinition
                     in definition.Effects)
            {
                if (effectDefinition == null ||
                    string.IsNullOrWhiteSpace(
                        effectDefinition.EffectId))
                {
                    throw new InvalidOperationException(
                        $"Passive '{definition.Id}' contains an invalid triggered Effect.");
                }

                string bindingKey =
                    $"{effectDefinition.Trigger}|{effectDefinition.Target}|{effectDefinition.EffectId}";

                if (!addedBindings.Add(bindingKey))
                {
                    throw new InvalidOperationException(
                        $"Passive '{definition.Id}' contains duplicate triggered Effect '{bindingKey}'.");
                }

                passive.AddEffect(
                    EffectFactory.CreateTriggeredEffect(
                        effectDefinition,
                        contentRegistry));
            }
        }
    }
}
