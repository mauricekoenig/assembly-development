using System;

namespace GameEngine
{
    internal sealed class LootTableEntry
    {
        private readonly Func<Reward> _rewardFactory;

        internal int Weight { get; }


        internal LootTableEntry(
            Func<Reward> rewardFactory,
            int weight)
        {
            Guard.NotNull(rewardFactory, nameof(rewardFactory));

            if (weight <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(weight));
            }

            _rewardFactory = rewardFactory;
            Weight = weight;
        }


        internal Reward CreateReward()
        {
            Reward reward =
                _rewardFactory();

            if (reward == null)
            {
                throw new InvalidOperationException(
                    "Loot reward factory returned null.");
            }

            return reward;
        }
    }
}