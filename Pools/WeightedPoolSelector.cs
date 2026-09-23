using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    internal static class WeightedPoolSelector
    {
        internal static List<PoolEntryDefinition> SelectDistinct(
            PoolDefinition pool,
            PoolEntryType requiredType,
            int count,
            Random random)
        {
            if (pool == null)
                throw new ArgumentNullException(nameof(pool));
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            if (count <= 0)
                return new List<PoolEntryDefinition>();

            List<PoolEntryDefinition> available =
                (pool.Entries ?? new List<PoolEntryDefinition>())
                    .Where(entry =>
                        entry != null &&
                        entry.Type == requiredType &&
                        !string.IsNullOrWhiteSpace(entry.ContentId) &&
                        entry.Weight > 0)
                    .GroupBy(entry => entry.ContentId)
                    .Select(group => group.First())
                    .ToList();

            int amountToSelect = Math.Min(count, available.Count);
            List<PoolEntryDefinition> selected =
                new List<PoolEntryDefinition>(amountToSelect);

            for (int i = 0; i < amountToSelect; i++)
            {
                int totalWeight = available.Sum(entry => entry.Weight);
                int roll = random.Next(totalWeight);
                int cumulativeWeight = 0;
                int selectedIndex = 0;

                for (int entryIndex = 0; entryIndex < available.Count; entryIndex++)
                {
                    cumulativeWeight += available[entryIndex].Weight;
                    if (roll < cumulativeWeight)
                    {
                        selectedIndex = entryIndex;
                        break;
                    }
                }

                selected.Add(available[selectedIndex]);
                available.RemoveAt(selectedIndex);
            }

            return selected;
        }
    }
}
