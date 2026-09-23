using System;
using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class DataDrivenRelic :
        Relic,
        IIncomingDamageModifier,
        IOutgoingDamageModifier,
        IIncomingHealingModifier,
        IOutgoingHealingModifier
    {
        private readonly IReadOnlyList<RelicModifierDefinition>
            _modifiers;


        internal DataDrivenRelic(
            RelicDefinition definition,
            ContentRegistry contentRegistry)
            : base(
                GetDefinitionId(definition),
                GetName(definition),
                GetDescription(definition))
        {
            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            _modifiers =
                definition.Modifiers ??
                new List<RelicModifierDefinition>();

            if (definition.Effects == null)
                return;

            foreach (TriggeredEffectDefinition effectDefinition
                     in definition.Effects)
            {
                AddEffect(
                    EffectFactory.CreateTriggeredEffect(
                        effectDefinition,
                        contentRegistry));
            }
        }


        public int ModifyIncomingDamage(
            int amount,
            DamageType damageType,
            ICombatant target,
            EffectContext context)
        {
            return ApplyDamageModifiers(
                amount,
                ModifierType.IncomingDamage,
                damageType,
                target,
                context);
        }


        public int ModifyOutgoingDamage(
            int amount,
            DamageType damageType,
            ICombatant target,
            EffectContext context)
        {
            return ApplyDamageModifiers(
                amount,
                ModifierType.OutgoingDamage,
                damageType,
                target,
                context);
        }


        public int ModifyIncomingHealing(
            int amount,
            ICombatant target,
            EffectContext context)
        {
            return ApplyHealingModifiers(
                amount,
                ModifierType.IncomingHealing,
                target,
                context);
        }


        public int ModifyOutgoingHealing(
            int amount,
            ICombatant target,
            EffectContext context)
        {
            return ApplyHealingModifiers(
                amount,
                ModifierType.OutgoingHealing,
                target,
                context);
        }


        private int ApplyDamageModifiers(
            int value,
            ModifierType type,
            DamageType damageType,
            ICombatant target,
            EffectContext context)
        {
            int result = value;

            foreach (RelicModifierDefinition modifier
                     in _modifiers)
            {
                if (modifier == null ||
                    modifier.Type != type ||
                    !MatchesUnitFilters(
                        modifier,
                        target,
                        context) ||
                    modifier.DamageType.HasValue &&
                    modifier.DamageType.Value != damageType)
                {
                    continue;
                }

                result =
                    ApplyOperation(
                        result,
                        modifier.Operation,
                        modifier.Amount);

                result =
                    Math.Max(
                        0,
                        result);
            }

            return result;
        }


        private int ApplyHealingModifiers(
            int value,
            ModifierType type,
            ICombatant target,
            EffectContext context)
        {
            int result = value;

            foreach (RelicModifierDefinition modifier
                     in _modifiers)
            {
                if (modifier == null ||
                    modifier.Type != type ||
                    !MatchesUnitFilters(
                        modifier,
                        target,
                        context))
                {
                    continue;
                }

                result =
                    ApplyOperation(
                        result,
                        modifier.Operation,
                        modifier.Amount);

                result =
                    Math.Max(
                        0,
                        result);
            }

            return result;
        }


        private static bool MatchesUnitFilters(
            RelicModifierDefinition modifier,
            ICombatant target,
            EffectContext context)
        {
            if (modifier.SourceUnitTag.HasValue)
            {
                if (!(context.Source is Unit sourceUnit) ||
                    !sourceUnit.HasTag(
                        modifier.SourceUnitTag.Value))
                {
                    return false;
                }
            }

            if (modifier.TargetUnitTag.HasValue)
            {
                if (!(target is Unit targetUnit) ||
                    !targetUnit.HasTag(
                        modifier.TargetUnitTag.Value))
                {
                    return false;
                }
            }

            return true;
        }


        private static int ApplyOperation(
            int value,
            ModifierOperation operation,
            int amount)
        {
            return operation switch
            {
                ModifierOperation.IncreasePercent =>
                    value * (100 + amount) / 100,

                ModifierOperation.ReducePercent =>
                    value * (100 - amount) / 100,

                ModifierOperation.AddFlat =>
                    value + amount,

                ModifierOperation.ReduceFlat =>
                    value - amount,

                _ =>
                    throw new NotSupportedException(
                        $"Unsupported modifier operation '{operation}'.")
            };
        }


        private static string GetDefinitionId(
            RelicDefinition definition)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNullOrWhiteSpace(
                definition.Id,
                nameof(definition.Id));

            return definition.Id;
        }


        private static string GetName(
            RelicDefinition definition)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNullOrWhiteSpace(
                definition.Name,
                nameof(definition.Name));

            return definition.Name;
        }


        private static string GetDescription(
            RelicDefinition definition)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            return definition.Description ??
                string.Empty;
        }
    }
}
