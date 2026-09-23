namespace GameEngine
{
    internal interface IIncomingDamageModifier
    {
        int ModifyIncomingDamage(
            int amount,
            DamageType damageType,
            ICombatant target,
            EffectContext context);
    }
}