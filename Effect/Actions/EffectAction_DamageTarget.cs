using System;
using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EffectAction_DamageTarget : EffectAction
    {
        public int Amount { get; }
        public DamageType DamageType { get; }

        public EffectAction_DamageTarget(
            int amount,
            DamageType damageType = DamageType.Physical)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Amount = amount;
            DamageType = damageType;
        }

        public override List<GameEvent> Execute(
            EffectContext context,
            IEffectTarget target)
        {
            Guard.NotNull(context, nameof(context));
            Guard.NotNull(target, nameof(target));

            List<GameEvent> events =
                new List<GameEvent>();

            if (!(target is ICombatant combatant))
                return events;

            bool wasAlive =
                combatant.CurrentHealth > 0;

            int healthBefore =
                combatant.CurrentHealth;

            int finalDamage =
                CalculateDamage(
                    combatant,
                    context);

            combatant.TakeDamage(finalDamage);

            int damageDealt =
                healthBefore -
                combatant.CurrentHealth;

            if (damageDealt <= 0)
                return events;

            if (target is Enemy enemy)
            {
                events.Add(
                    new EnemyDamagedEvent(
                        new EnemyInfo(enemy),
                        damageDealt));

                return events;
            }

            if (target is Unit unit)
            {
                events.Add(
                    new UnitDamagedEvent(
                        new UnitInfo(unit),
                        damageDealt));

                if (wasAlive && !unit.IsAlive)
                {
                    events.Add(
                        new UnitDiedEvent(
                            new UnitInfo(unit)));
                }
            }

            return events;
        }

        private int CalculateDamage(
            ICombatant target,
            EffectContext context)
        {
            int damage = Amount;

            // Run-wide Sector Blessings modify the base value of a Sector
            // before normal combat modifiers are applied.
            if (context.Source is Sector sector &&
                context.Run != null)
            {
                damage +=
                    context.Run.SectorModifiers.GetValue(
                        sector.SectorType);

                damage =
                    Math.Max(
                        0,
                        damage);
            }

            // Outgoing modifiers belong to the player source / caster.
            if (context.Source is IHasPassives sourcePassiveOwner)
            {
                foreach (Passive passive
                         in sourcePassiveOwner.Passives)
                {
                    foreach (PassiveModifier passiveModifier
                             in passive.Modifiers)
                    {
                        if (!(passiveModifier
                            is IOutgoingDamageModifier modifier))
                        {
                            continue;
                        }

                        damage =
                            modifier.ModifyOutgoingDamage(
                                damage,
                                DamageType,
                                target,
                                context);

                        damage =
                            Math.Max(
                                0,
                                damage);
                    }
                }
            }

            // Relics are player-side battle modifiers. They must never
            // increase or reduce Enemy outgoing damage.
            if (context.Source is Unit &&
                context.Run != null)
            {
                foreach (Relic relic
                         in context.Run.Relics)
                {
                    if (!(relic
                        is IOutgoingDamageModifier modifier))
                    {
                        continue;
                    }

                    damage =
                        modifier.ModifyOutgoingDamage(
                            damage,
                            DamageType,
                            target,
                            context);

                    damage =
                        Math.Max(
                            0,
                            damage);
                }
            }

            // Incoming modifiers belong to the target.
            if (target is IHasPassives passiveOwner)
            {
                foreach (Passive passive
                         in passiveOwner.Passives)
                {
                    foreach (PassiveModifier passiveModifier
                             in passive.Modifiers)
                    {
                        if (!(passiveModifier
                            is IIncomingDamageModifier modifier))
                        {
                            continue;
                        }

                        damage =
                            modifier.ModifyIncomingDamage(
                                damage,
                                DamageType,
                                target,
                                context);

                        damage =
                            Math.Max(
                                0,
                                damage);
                    }
                }
            }

            // Relic incoming modifiers only protect player Units.
            if (target is Unit &&
                context.Run != null)
            {
                foreach (Relic relic
                         in context.Run.Relics)
                {
                    if (!(relic
                        is IIncomingDamageModifier modifier))
                    {
                        continue;
                    }

                    damage =
                        modifier.ModifyIncomingDamage(
                            damage,
                            DamageType,
                            target,
                            context);

                    damage =
                        Math.Max(
                            0,
                            damage);
                }
            }

            return damage;
        }
    }
}
