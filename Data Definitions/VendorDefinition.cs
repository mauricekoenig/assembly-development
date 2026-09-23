using System.Collections.Generic;

namespace GameEngine
{
    public sealed class VendorDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int OfferCount { get; set; } = 3;

        public List<VendorOfferDefinition> Offers { get; set; } =
            new List<VendorOfferDefinition>();
    }
}
