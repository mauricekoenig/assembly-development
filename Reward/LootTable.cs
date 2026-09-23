using System;
using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class LootTable
    {
        private readonly List<LootTableEntry> _entries =
            new List<LootTableEntry>();


        internal void Add(
            Func<Reward> rewardFactory,
            int weight)
        {
            _entries.Add(
                new LootTableEntry(
                    rewardFactory,
                    weight));
        }


        internal Reward Roll(
            Random random)
        {
            Guard.NotNull(random, nameof(random));

            if (_entries.Count == 0)
            {
                throw new InvalidOperationException(
                    "Cannot roll an empty LootTable.");
            }

            int totalWeight = 0;

            foreach (LootTableEntry entry in _entries)
            {
                totalWeight += entry.Weight;
            }

            int roll =
                random.Next(
                    0,
                    totalWeight);

            int currentWeight = 0;

            foreach (LootTableEntry entry in _entries)
            {
                currentWeight += entry.Weight;

                if (roll < currentWeight)
                {
                    return entry.CreateReward();
                }
            }

            throw new InvalidOperationException(
                "LootTable failed to resolve a Reward.");
        }
    }
}