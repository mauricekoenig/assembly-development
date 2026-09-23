using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class RelicReward :
        Reward
    {
        private readonly Relic _relic;


        internal RelicReward(
            Relic relic)
        {
            Guard.NotNull(relic, nameof(relic));

            _relic = relic;
        }


        internal override List<GameEvent> Apply(
            Run run)
        {
            Guard.NotNull(run, nameof(run));

            run.AddRelic(_relic);

            return new List<GameEvent>
            {
                new RelicAddedEvent(
                    new RelicInfo(_relic))
            };
        }


        internal override RewardInfo CreateInfo()
        {
            return new RewardInfo(
                RewardType.Relic,
                RewardContentType.Relic,
                _relic.DefinitionId,
                _relic.Name,
                _relic.Description);
        }
    }
}
