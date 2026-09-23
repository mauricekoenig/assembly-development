namespace GameEngine
{
    internal interface IIncomingHealingModifier
    {
        int ModifyIncomingHealing(
            int amount,
            ICombatant target,
            EffectContext context);
    }
}