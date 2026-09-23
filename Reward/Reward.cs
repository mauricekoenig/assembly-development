using System.Collections.Generic;

namespace GameEngine
{
    internal abstract class Reward
    {
        internal abstract RewardInfo CreateInfo();

        internal abstract List<GameEvent> Apply(
            Run run);
    }
}