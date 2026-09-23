using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class ItemReward :
        Reward
    {
        private readonly Item _item;
        private readonly RewardContentType _contentType;
        private readonly string _contentId;


        internal ItemReward(
            Item item,
            RewardContentType contentType,
            string contentId)
        {
            Guard.NotNull(
                item,
                nameof(item));

            Guard.NotNullOrWhiteSpace(
                contentId,
                nameof(contentId));

            _item = item;
            _contentType = contentType;
            _contentId = contentId;
        }


        internal override List<GameEvent> Apply(
            Run run)
        {
            Guard.NotNull(run, nameof(run));

            run.AddItem(_item);

            return new List<GameEvent>
            {
                new ItemAddedEvent(
                    new ItemInfo(_item))
            };
        }


        internal override RewardInfo CreateInfo()
        {
            return new RewardInfo(
                RewardType.Item,
                _contentType,
                _contentId,
                _item.Name,
                _item.Description);
        }
    }
}
