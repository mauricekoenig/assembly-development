using System;
using System.Collections.Generic;

namespace GameEngine
{
    internal abstract class ShopOffer : IGameEntity
    {
        public Guid Id { get; } =
            Guid.NewGuid();

        internal VendorOfferType Type { get; }

        internal string ContentId { get; }

        internal string Name { get; }

        internal string Description { get; }

        internal int Price { get; }


        protected ShopOffer(
            VendorOfferType type,
            string contentId,
            string name,
            string description,
            int price)
        {
            Guard.NotNullOrWhiteSpace(
                contentId,
                nameof(contentId));

            Guard.NotNullOrWhiteSpace(
                name,
                nameof(name));

            Guard.NotNullOrWhiteSpace(
                description,
                nameof(description));

            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price));

            Type =
                type;

            ContentId =
                contentId;

            Name =
                name;

            Description =
                description;

            Price =
                price;
        }


        internal abstract List<GameEvent> Purchase(
            Run run);
    }
}
