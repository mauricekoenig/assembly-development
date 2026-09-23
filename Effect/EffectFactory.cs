using System;

namespace GameEngine
{
    public static class EffectFactory
    {
        internal static Effect CreateHealEffect(
            int amount,
            string name = "Healing",
            string description = "Restores health.")
        {
            Effect effect =
                new Effect(
                    name,
                    description);

            effect.AddComponent(
                new EffectComponent
                {
                    Condition = new EffectCondition_NoCondition(),
                    Action = new EffectAction_HealTarget(amount)
                });

            return effect;
        }

        internal static Effect CreateDamageEffect(
            int amount,
            DamageType damageType = DamageType.Physical,
            string name = "Damage",
            string description = "Deals damage.")
        {
            Effect effect =
                new Effect(
                    name,
                    description);

            effect.AddComponent(
                new EffectComponent
                {
                    Condition = new EffectCondition_NoCondition(),
                    Action = new EffectAction_DamageTarget(
                        amount,
                        damageType)
                });

            return effect;
        }

        internal static Effect CreateMoveRingObjectEffect(
            string name = "Move",
            string description = "Moves the selected RingObject.")
        {
            Effect effect =
                new Effect(
                    name,
                    description);

            effect.AddComponent(
                new EffectComponent
                {
                    Condition = new EffectCondition_NoCondition(),
                    Action = new EffectAction_MoveRingObject()
                });

            return effect;
        }

        internal static Effect CreateSwapUnitsEffect(
            string name = "Swap",
            string description = "Swaps the selected Units.")
        {
            Effect effect =
                new Effect(
                    name,
                    description);

            effect.AddComponent(
                new EffectComponent
                {
                    Condition = new EffectCondition_NoCondition(),
                    Action = new EffectAction_SwapUnits()
                });

            return effect;
        }

        public static Effect Create(
            EffectDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (definition.Components == null ||
                definition.Components.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Effect '{definition.Id}' has no components.");
            }

            Effect effect =
                new Effect(
                    definition.Name,
                    definition.Description);

            foreach (EffectComponentDefinition componentDefinition
                     in definition.Components)
            {
                effect.AddComponent(
                    CreateComponent(
                        componentDefinition));
            }

            return effect;
        }

        internal static ActionEffect CreateActionEffect(
            ActionEffectDefinition definition,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            Guard.NotNullOrWhiteSpace(
                definition.EffectId,
                nameof(definition.EffectId));

            return new ActionEffect(
                contentRegistry.CreateEffect(
                    definition.EffectId),
                definition.Target);
        }

        internal static TriggeredEffect CreateTriggeredEffect(
            TriggeredEffectDefinition definition,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            Guard.NotNullOrWhiteSpace(
                definition.EffectId,
                nameof(definition.EffectId));

            return new TriggeredEffect(
                contentRegistry.CreateEffect(
                    definition.EffectId),
                definition.Trigger,
                definition.Target);
        }

        private static EffectComponent CreateComponent(
            EffectComponentDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            EffectAction action =
                definition.Action switch
                {
                    EffectActionType.Damage =>
                        new EffectAction_DamageTarget(
                            definition.Amount,
                            definition.DamageType),

                    EffectActionType.Heal =>
                        new EffectAction_HealTarget(
                            definition.Amount),

                    EffectActionType.MoveRingObject =>
                        new EffectAction_MoveRingObject(),

                    EffectActionType.SwapUnits =>
                        new EffectAction_SwapUnits(),

                    _ =>
                        throw new NotSupportedException(
                            $"Effect action '{definition.Action}' is not supported.")
                };

            return new EffectComponent
            {
                Condition = new EffectCondition_NoCondition(),
                Action = action
            };
        }
    }
}
