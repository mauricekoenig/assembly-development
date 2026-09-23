using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    internal sealed class DataDrivenVendor :
        ShopEncounter
    {
        internal DataDrivenVendor(
            VendorDefinition definition,
            Random random,
            ContentRegistry contentRegistry)
            : base(
                GetDefinitionId(definition),
                GetName(definition))
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            List<VendorOfferDefinition> candidates =
                definition.Offers
                    .Where(offer =>
                        offer != null &&
                        offer.Weight > 0)
                    .ToList();

            int offerCount =
                Math.Min(
                    definition.OfferCount,
                    candidates.Count);

            for (int index = 0;
                 index < offerCount;
                 index++)
            {
                VendorOfferDefinition selected =
                    SelectWeightedOffer(
                        candidates,
                        random);

                AddOffer(
                    contentRegistry.CreateShopOffer(
                        selected));

                candidates.Remove(selected);
            }
        }


        private static VendorOfferDefinition SelectWeightedOffer(
            IReadOnlyList<VendorOfferDefinition> candidates,
            Random random)
        {
            int totalWeight =
                candidates.Sum(candidate =>
                    candidate.Weight);

            int roll =
                random.Next(totalWeight);

            int cumulativeWeight = 0;

            foreach (VendorOfferDefinition candidate
                     in candidates)
            {
                cumulativeWeight +=
                    candidate.Weight;

                if (roll < cumulativeWeight)
                    return candidate;
            }

            return candidates[candidates.Count - 1];
        }


        private static string GetDefinitionId(
            VendorDefinition definition)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNullOrWhiteSpace(
                definition.Id,
                nameof(definition.Id));

            return definition.Id;
        }


        private static string GetName(
            VendorDefinition definition)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNullOrWhiteSpace(
                definition.Name,
                nameof(definition.Name));

            return definition.Name;
        }
    }
}
