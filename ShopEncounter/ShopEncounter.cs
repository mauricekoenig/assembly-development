using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public abstract class ShopEncounter : IGameEntity
    {
        private readonly List<ShopOffer> _offers =
            new List<ShopOffer>();

        public Guid Id { get; } =
            Guid.NewGuid();

        public string DefinitionId { get; }

        public string Name { get; protected set; }

        internal IReadOnlyList<ShopOffer> Offers =>
            _offers;


        protected ShopEncounter(
            string definitionId,
            string name)
        {
            Guard.NotNullOrWhiteSpace(
                definitionId,
                nameof(definitionId));

            Guard.NotNullOrWhiteSpace(
                name,
                nameof(name));

            DefinitionId =
                definitionId;

            Name =
                name;
        }


        internal void AddOffer(
            ShopOffer offer)
        {
            Guard.NotNull(
                offer,
                nameof(offer));

            _offers.Add(offer);
        }


        internal bool RemoveOffer(
            ShopOffer offer)
        {
            Guard.NotNull(
                offer,
                nameof(offer));

            return _offers.Remove(offer);
        }


        internal bool TryGetOffer(
            Guid offerId,
            out ShopOffer offer)
        {
            offer =
                _offers.FirstOrDefault(
                    candidate =>
                        candidate.Id == offerId);

            return offer != null;
        }
    }
}
