using System;

namespace GameEngine
{
    internal sealed class PassiveModifier_IncomingHealing :
        PassiveModifier,
        IIncomingHealingModifier
    {
        public ModifierOperation Operation { get; }
        public int Amount { get; }

        internal PassiveModifier_IncomingHealing(
            ModifierOperation operation,
            int amount)
        {
            Operation = operation;
            Amount = amount;
        }

        public int ModifyIncomingHealing(
            int value,
            ICombatant target,
            EffectContext context)
        {
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