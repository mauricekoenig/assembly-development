using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class ItemShopOffer :
        ShopOffer
    {
        private readonly Item _item;


        internal ItemShopOffer(
            VendorOfferType type,
            string contentId,
            Item item,
            int price)
            : base(
                type,
                contentId,
                GetName(item),
                GetDescription(item),
                price)
        {
            Guard.NotNull(
                item,
                nameof(item));

            _item =
                item;
        }


        internal override List<GameEvent> Purchase(
            Run run)
        {
            Guard.NotNull(
                run,
                nameof(run));

            run.AddItem(_item);

            return new List<GameEvent>
            {
                new ItemAddedEvent(
                    new ItemInfo(_item))
            };
        }


        private static string GetName(
            Item item)
        {
            Guard.NotNull(
                item,
                nameof(item));

            return item.Name;
        }


        private static string GetDescription(
            Item item)
        {
            Guard.NotNull(
                item,
                nameof(item));

            return item.Description;
        }
    }
}
