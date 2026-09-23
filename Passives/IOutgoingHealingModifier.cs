namespace GameEngine
{
    internal interface IOutgoingHealingModifier
    {
        int ModifyOutgoingHealing(
            int amount,
            ICombatant target,
            EffectContext context);
    }
}
