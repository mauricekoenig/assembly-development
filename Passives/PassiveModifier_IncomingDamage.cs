using System;

namespace GameEngine
{
    internal sealed class PassiveModifier_IncomingDamage :
        PassiveModifier,
        IIncomingDamageModifier
    {
        public ModifierOperation Operation { get; }
        public int Amount { get; }
        public DamageType? DamageType { get; }

        internal PassiveModifier_IncomingDamage(
            ModifierOperation operation,
            int amount,
            DamageType? damageType)
        {
            Operation = operation;
            Amount = amount;
            DamageType = damageType;
        }

        public int ModifyIncomingDamage(
            int value,
            DamageType damageType,
            ICombatant target,
            EffectContext context)
        {
            if (DamageType.HasValue &&
                DamageType.Value != damageType)
            {
                return value;
            }

            return Operation switch
            {
                ModifierOperation.IncreasePercent =>
                    value * (100 + Amount) / 100,

                ModifierOperation.ReducePercent =>
                    value * (100 - Amount) / 100,

                ModifierOperation.AddFlat =>
                    value + Amount,

                ModifierOperation.ReduceFlat =>
                    value - Amount,

                _ =>
                    throw new NotSupportedException()
            };
        }
    }
}