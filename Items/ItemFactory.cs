using System;

namespace GameEngine
{
    public static class ItemFactory
    {
        internal static Item Create(
            ItemReference itemReference,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(
                itemReference,
                nameof(itemReference));

            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            if (string.IsNullOrWhiteSpace(
                    itemReference.ContentId))
            {
                throw new ArgumentException(
                    "Item reference ContentId cannot be empty.",
                    nameof(itemReference));
            }

            switch (itemReference.Type)
            {
                case ItemType.EffectRecipe:
                    {
                        EffectDefinition definition =
                            contentRegistry.GetEffect(
                                itemReference.ContentId);

                        EnsureItemizable(
                            definition.CanBeItemized,
                            "Effect",
                            definition.Id);

                        return new EffectRecipe(
                            UnitActionFactory.CreateFromEffectRecipe(
                                definition,
                                contentRegistry));
                    }

                case ItemType.PassiveRecipe:
                    {
                        PassiveDefinition definition =
                            contentRegistry.GetPassive(
                                itemReference.ContentId);

                        EnsureItemizable(
                            definition.CanBeItemized,
                            "Passive",
                            definition.Id);

                        return new PassiveRecipe(
                            contentRegistry.CreatePassive(
                                definition.Id));
                    }

                case ItemType.AbilityRecipe:
                    {
                        AbilityDefinition definition =
                            contentRegistry.GetAbility(
                                itemReference.ContentId);

                        EnsureItemizable(
                            definition.CanBeItemized,
                            "Ability",
                            definition.Id);

                        return new AbilityRecipe(
                            contentRegistry.CreateAbility(
                                definition.Id));
                    }

                default:
                    throw new NotSupportedException(
                        $"Item type '{itemReference.Type}' is not supported.");
            }
        }


        private static void EnsureItemizable(
            bool canBeItemized,
            string contentType,
            string contentId)
        {
            if (canBeItemized)
                return;

            throw new InvalidOperationException(
                $"{contentType} '{contentId}' cannot be itemized.");
        }
    }
}
