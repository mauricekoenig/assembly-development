using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public sealed class ShopEncounterInfo
    {
        public Guid Id { get; }

        public string DefinitionId { get; }

        public string Name { get; }

        public IReadOnlyList<ShopOfferInfo> Offers { get; }


        internal ShopEncounterInfo(
            ShopEncounter encounter)
        {
            Guard.NotNull(
                encounter,
                nameof(encounter));

            Id =
                encounter.Id;

            DefinitionId =
                encounter.DefinitionId;

            Name =
                encounter.Name;

            Offers =
                encounter.Offers
                    .Select(offer =>
                        new ShopOfferInfo(offer))
                    .ToList();
        }
    }
}
