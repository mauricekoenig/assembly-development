namespace GameEngine
{
    internal interface IOutgoingDamageModifier
    {
        int ModifyOutgoingDamage(
            int amount,
            DamageType damageType,
            ICombatant target,
            EffectContext context);
    }
}
