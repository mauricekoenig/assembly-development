using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class BlessingReward :
        Reward
    {
        private readonly IBlessing _blessing;


        internal BlessingReward(
            IBlessing blessing)
        {
            Guard.NotNull(
                blessing,
                nameof(blessing));

            _blessing = blessing;
        }


        internal override List<GameEvent> Apply(
            Run run)
        {
            Guard.NotNull(
                run,
                nameof(run));

            run.AddBlessing(
                _blessing);

            return new List<GameEvent>
            {
                new BlessingAddedEvent(
                    new BlessingInfo(_blessing))
            };
        }


        internal override RewardInfo CreateInfo()
        {
            return new RewardInfo(
                RewardType.Blessing,
                RewardContentType.Blessing,
                _blessing.DefinitionId,
                _blessing.Name,
                _blessing.Description);
        }
    }
}
