namespace GameEngine
{
    public sealed class VendorOfferDefinition
    {
        public VendorOfferType Type { get; set; }

        public string ContentId { get; set; }

        public int Price { get; set; }

        public int Weight { get; set; } = 100;
    }
}
