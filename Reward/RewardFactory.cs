using System;

namespace GameEngine
{
    internal static class RewardFactory
    {
        internal static Reward Create(
            RewardDefinition definition,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            Guard.NotNullOrWhiteSpace(
                definition.ContentId,
                nameof(definition.ContentId));

            switch (definition.Type)
            {
                case RewardContentType.EffectRecipe:
                    return CreateItemReward(
                        ItemType.EffectRecipe,
                        definition,
                        contentRegistry);

                case RewardContentType.PassiveRecipe:
                    return CreateItemReward(
                        ItemType.PassiveRecipe,
                        definition,
                        contentRegistry);

                case RewardContentType.AbilityRecipe:
                    return CreateItemReward(
                        ItemType.AbilityRecipe,
                        definition,
                        contentRegistry);

                case RewardContentType.Unit:
                    return new UnitReward(
                        contentRegistry.CreateUnit(
                            definition.ContentId));

                case RewardContentType.Relic:
                    return new RelicReward(
                        contentRegistry.CreateRelic(
                            definition.ContentId));

                case RewardContentType.Blessing:
                    return new BlessingReward(
                        contentRegistry.CreateBlessing(
                            definition.ContentId));

                default:
                    throw new NotSupportedException(
                        $"Reward content type '{definition.Type}' is not supported.");
            }
        }


        private static ItemReward CreateItemReward(
            ItemType itemType,
            RewardDefinition definition,
            ContentRegistry contentRegistry)
        {
            ItemReference itemReference =
                new ItemReference
                {
                    Type = itemType,
                    ContentId = definition.ContentId
                };

            return new ItemReward(
                contentRegistry.CreateItem(
                    itemReference),
                definition.Type,
                definition.ContentId);
        }
    }
}
