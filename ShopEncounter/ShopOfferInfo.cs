using System;

namespace GameEngine
{
    public sealed class ShopOfferInfo
    {
        public Guid Id { get; }

        public VendorOfferType Type { get; }

        public string ContentId { get; }

        public string Name { get; }

        public string Description { get; }

        public int Price { get; }


        internal ShopOfferInfo(
            ShopOffer offer)
        {
            Guard.NotNull(
                offer,
                nameof(offer));

            Id =
                offer.Id;

            Type =
                offer.Type;

            ContentId =
                offer.ContentId;

            Name =
                offer.Name;

            Description =
                offer.Description;

            Price =
                offer.Price;
        }
    }
}
