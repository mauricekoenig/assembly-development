using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    internal static class EventOutcomeResolver
    {
        internal static bool CanPayCosts(
            Run run,
            IReadOnlyList<EventOutcomeDefinition> costs,
            out string reason)
        {
            Guard.NotNull(run, nameof(run));

            reason = string.Empty;

            if (costs == null || costs.Count == 0)
                return true;

            int currencyCost = 0;

            foreach (EventOutcomeDefinition cost in costs)
            {
                if (cost == null)
                {
                    reason = "Event cost is missing.";
                    return false;
                }

                switch (cost.Type)
                {
                    case EventOutcomeType.LoseCurrency:
                        currencyCost += Math.Max(0, cost.Amount);
                        break;

                    case EventOutcomeType.DamageFirstLivingUnit:
                    case EventOutcomeType.DamageAllLivingUnits:
                        if (!run.Team.Any(unit => unit.IsAlive))
                        {
                            reason = "Requires at least one living unit to pay this cost.";
                            return false;
                        }
                        break;

                    default:
                        reason = $"'{cost.Type}' cannot be used as an event cost.";
                        return false;
                }
            }

            if (run.Currency < currencyCost)
            {
                reason = $"Requires {currencyCost} currency.";
                return false;
            }

            return true;
        }

        internal static List<GameEvent> Apply(
            Run run,
            IReadOnlyList<EventOutcomeDefinition> outcomes,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(run, nameof(run));
            Guard.NotNull(contentRegistry, nameof(contentRegistry));

            List<GameEvent> events = new List<GameEvent>();

            if (outcomes == null)
                return events;

            foreach (EventOutcomeDefinition outcome in outcomes)
            {
                if (outcome == null)
                    throw new InvalidOperationException("Event outcome cannot be null.");

                switch (outcome.Type)
                {
                    case EventOutcomeType.GainCurrency:
                        ApplyGainCurrency(run, outcome.Amount, events);
                        break;

                    case EventOutcomeType.LoseCurrency:
                        ApplyLoseCurrency(run, outcome.Amount, events);
                        break;

                    case EventOutcomeType.DamageFirstLivingUnit:
                        ApplyDamageFirstLivingUnit(run, outcome.Amount, events);
                        break;

                    case EventOutcomeType.HealFirstLivingUnit:
                        ApplyHealFirstLivingUnit(run, outcome.Amount, events);
                        break;

                    case EventOutcomeType.DamageAllLivingUnits:
                        ApplyDamageAllLivingUnits(run, outcome.Amount, events);
                        break;

                    case EventOutcomeType.HealAllLivingUnits:
                        ApplyHealAllLivingUnits(run, outcome.Amount, events);
                        break;

                    case EventOutcomeType.GainItem:
                        ApplyGainItem(run, outcome.Item, contentRegistry, events);
                        break;

                    case EventOutcomeType.GainReward:
                        ApplyGainReward(run, outcome.Reward, contentRegistry, events);
                        break;

                    default:
                        throw new NotSupportedException(
                            $"Event outcome '{outcome.Type}' is not supported.");
                }
            }

            return events;
        }

        private static void ApplyGainCurrency(
            Run run,
            int amount,
            List<GameEvent> events)
        {
            if (amount <= 0)
                return;

            run.AddCurrency(amount);

            events.Add(
                new CurrencyGainedEvent(
                    amount,
                    run.Currency));
        }

        private static void ApplyLoseCurrency(
            Run run,
            int amount,
            List<GameEvent> events)
        {
            if (amount <= 0)
                return;

            if (!run.TryRemoveCurrency(amount))
            {
                throw new InvalidOperationException(
                    $"Cannot remove {amount} currency from the run.");
            }

            events.Add(
                new CurrencyLostEvent(
                    amount,
                    run.Currency));
        }

        private static void ApplyDamageFirstLivingUnit(
            Run run,
            int amount,
            List<GameEvent> events)
        {
            Unit unit = run.Team.FirstOrDefault(candidate => candidate.IsAlive);

            if (unit == null || amount <= 0)
                return;

            ApplyDamage(unit, amount, events);
        }

        private static void ApplyHealFirstLivingUnit(
            Run run,
            int amount,
            List<GameEvent> events)
        {
            Unit unit = run.Team.FirstOrDefault(candidate => candidate.IsAlive);

            if (unit == null || amount <= 0)
                return;

            ApplyHeal(unit, amount, events);
        }

        private static void ApplyDamageAllLivingUnits(
            Run run,
            int amount,
            List<GameEvent> events)
        {
            if (amount <= 0)
                return;

            foreach (Unit unit in run.Team.Where(candidate => candidate.IsAlive).ToList())
            {
                ApplyDamage(unit, amount, events);
            }
        }

        private static void ApplyHealAllLivingUnits(
            Run run,
            int amount,
            List<GameEvent> events)
        {
            if (amount <= 0)
                return;

            foreach (Unit unit in run.Team.Where(candidate => candidate.IsAlive))
            {
                ApplyHeal(unit, amount, events);
            }
        }

        private static void ApplyDamage(
            Unit unit,
            int amount,
            List<GameEvent> events)
        {
            bool wasAlive = unit.IsAlive;
            int before = unit.CurrentHealth;

            unit.TakeDamage(amount);

            int actualAmount = before - unit.CurrentHealth;

            if (actualAmount <= 0)
                return;

            events.Add(
                new UnitDamagedEvent(
                    new UnitInfo(unit),
                    actualAmount));

            if (wasAlive && !unit.IsAlive)
            {
                events.Add(
                    new UnitDiedEvent(
                        new UnitInfo(unit)));
            }
        }

        private static void ApplyHeal(
            Unit unit,
            int amount,
            List<GameEvent> events)
        {
            int before = unit.CurrentHealth;
            unit.Heal(amount);
            int actualAmount = unit.CurrentHealth - before;

            if (actualAmount <= 0)
                return;

            events.Add(
                new UnitHealedEvent(
                    new UnitInfo(unit),
                    actualAmount));
        }

        private static void ApplyGainItem(
            Run run,
            ItemReference itemReference,
            ContentRegistry contentRegistry,
            List<GameEvent> events)
        {
            if (itemReference == null)
            {
                throw new InvalidOperationException(
                    "GainItem event outcome requires an Item reference.");
            }

            Item item = contentRegistry.CreateItem(itemReference);
            run.AddItem(item);

            events.Add(
                new ItemAddedEvent(
                    new ItemInfo(item)));
        }


        private static void ApplyGainReward(
            Run run,
            RewardDefinition rewardDefinition,
            ContentRegistry contentRegistry,
            List<GameEvent> events)
        {
            if (rewardDefinition == null)
            {
                throw new InvalidOperationException(
                    "GainReward event outcome requires a Reward definition.");
            }

            Reward reward =
                contentRegistry.CreateReward(
                    rewardDefinition);

            events.AddRange(
                reward.Apply(
                    run));
        }
    }
}
