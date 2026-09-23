using System;

namespace GameEngine
{
    public static class PassiveFactory
    {
        public static Passive Create(
            PassiveDefinition definition)
        {
            return Create(
                definition,
                null);
        }


        internal static Passive Create(
            PassiveDefinition definition,
            ContentRegistry contentRegistry)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            Passive passive =
                new Passive(
                    definition.Name,
                    definition.Description);


            // =========================================================
            // MODIFIERS
            // =========================================================

            if (definition.Modifiers != null)
            {
                foreach (PassiveModifierDefinition modifierDefinition
                         in definition.Modifiers)
                {
                    PassiveModifier modifier =
                        CreateModifier(
                            modifierDefinition);

                    passive.AddModifier(
                        modifier);
                }
            }


            // =========================================================
            // TRIGGERED EFFECTS
            // =========================================================

            if (definition.Effects != null &&
                definition.Effects.Count > 0)
            {
                if (contentRegistry == null)
                {
                    throw new InvalidOperationException(
                        $"Passive '{definition.Id}' references Effects and must be created through ContentRegistry.");
                }

                PassiveEffectLoader.AddEffects(
                    passive,
                    definition,
                    contentRegistry);
            }

            return passive;
        }


        private static PassiveModifier CreateModifier(
            PassiveModifierDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            return definition.Type switch
            {
                ModifierType.IncomingHealing =>
                    new PassiveModifier_IncomingHealing(
                        definition.Operation,
                        definition.Amount),

                ModifierType.IncomingDamage =>
                    new PassiveModifier_IncomingDamage(
                        definition.Operation,
                        definition.Amount,
                        definition.DamageType),

                ModifierType.OutgoingDamage =>
                    new PassiveModifier_OutgoingDamage(
                        definition.Operation,
                        definition.Amount,
                        definition.DamageType),

                ModifierType.OutgoingHealing =>
                    new PassiveModifier_OutgoingHealing(
                        definition.Operation,
                        definition.Amount),

                _ =>
                    throw new NotSupportedException(
                        $"Modifier type '{definition.Type}' is not supported yet.")
            };
        }
    }
}
