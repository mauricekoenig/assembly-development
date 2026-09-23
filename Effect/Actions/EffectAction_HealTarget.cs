using System;
using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EffectAction_HealTarget : EffectAction
    {
        public int Amount { get; }

        public EffectAction_HealTarget(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Amount = amount;
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

            int healthBefore =
                combatant.CurrentHealth;

            int finalHealing =
                CalculateHealing(
                    combatant,
                    context);

            combatant.Heal(finalHealing);

            int healingDone =
                combatant.CurrentHealth -
                healthBefore;

            if (healingDone <= 0)
                return events;

            if (target is Unit unit)
            {
                events.Add(
                    new UnitHealedEvent(
                        new UnitInfo(unit),
                        healingDone));

                return events;
            }

            if (target is Enemy enemy)
            {
                events.Add(
                    new EnemyHealedEvent(
                        new EnemyInfo(enemy),
                        healingDone));
            }

            return events;
        }

        private int CalculateHealing(
            ICombatant target,
            EffectContext context)
        {
            int healing = Amount;

            // Run-wide Sector Blessings modify the base value of a Sector
            // before normal combat modifiers are applied.
            if (context.Source is Sector sector &&
                context.Run != null)
            {
                healing +=
                    context.Run.SectorModifiers.GetValue(
                        sector.SectorType);

                healing =
                    Math.Max(
                        0,
                        healing);
            }

            // Outgoing healing modifiers belong to the Effect source / caster.
            if (context.Source is IHasPassives sourcePassiveOwner)
            {
                foreach (Passive passive
                         in sourcePassiveOwner.Passives)
                {
                    foreach (PassiveModifier modifier
                             in passive.Modifiers)
                    {
                        if (!(modifier
                            is IOutgoingHealingModifier healingModifier))
                        {
                            continue;
                        }

                        healing =
                            healingModifier
                                .ModifyOutgoingHealing(
                                    healing,
                                    target,
                                    context);

                        healing =
                            Math.Max(
                                0,
                                healing);
                    }
                }
            }

            if (context.Source is Unit &&
                context.Run != null)
            {
                foreach (Relic relic
                         in context.Run.Relics)
                {
                    if (!(relic
                        is IOutgoingHealingModifier modifier))
                    {
                        continue;
                    }

                    healing =
                        modifier.ModifyOutgoingHealing(
                            healing,
                            target,
                            context);

                    healing =
                        Math.Max(
                            0,
                            healing);
                }
            }

            // Incoming healing modifiers belong to the healed target.
            if (target is IHasPassives passiveOwner)
            {
                foreach (Passive passive
                         in passiveOwner.Passives)
                {
                    foreach (PassiveModifier modifier
                             in passive.Modifiers)
                    {
                        if (!(modifier
                            is IIncomingHealingModifier healingModifier))
                        {
                            continue;
                        }

                        healing =
                            healingModifier
                                .ModifyIncomingHealing(
                                    healing,
                                    target,
                                    context);

                        healing =
                            Math.Max(
                                0,
                                healing);
                    }
                }
            }

            if (target is Unit &&
                context.Run != null)
            {
                foreach (Relic relic
                         in context.Run.Relics)
                {
                    if (!(relic
                        is IIncomingHealingModifier modifier))
                    {
                        continue;
                    }

                    healing =
                        modifier.ModifyIncomingHealing(
                            healing,
                            target,
                            context);

                    healing =
                        Math.Max(
                            0,
                            healing);
                }
            }

            return healing;
        }
    }
}
