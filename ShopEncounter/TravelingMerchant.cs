namespace GameEngine
{
    // Legacy compatibility class. Normal Shop floors are created from
    // VendorDefinition + Vendor Pools and no longer instantiate this type.
    internal sealed class TravelingMerchant :
        ShopEncounter
    {
        internal TravelingMerchant(
            ContentRegistry contentRegistry)
            : base(
                "legacy:traveling_merchant",
                "Traveling Merchant")
        {
            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));

            AddOffer(
                contentRegistry.CreateShopOffer(
                    new VendorOfferDefinition
                    {
                        Type = VendorOfferType.PassiveRecipe,
                        ContentId = "passive:spikes",
                        Price = 50,
                        Weight = 100
                    }));

            AddOffer(
                contentRegistry.CreateShopOffer(
                    new VendorOfferDefinition
                    {
                        Type = VendorOfferType.Blessing,
                        ContentId = ContentRegistry.LegacyGoldBlessingId,
                        Price = 100,
                        Weight = 100
                    }));

            AddOffer(
                contentRegistry.CreateShopOffer(
                    new VendorOfferDefinition
                    {
                        Type = VendorOfferType.Relic,
                        ContentId = ContentRegistry.HeartOfTheDeepRelicId,
                        Price = 100,
                        Weight = 100
                    }));
        }
    }
}
