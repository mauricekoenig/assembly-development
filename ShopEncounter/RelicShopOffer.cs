using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class RelicShopOffer :
        ShopOffer
    {
        private readonly Relic _relic;


        internal RelicShopOffer(
            string contentId,
            Relic relic,
            int price)
            : base(
                VendorOfferType.Relic,
                contentId,
                GetName(relic),
                GetDescription(relic),
                price)
        {
            Guard.NotNull(
                relic,
                nameof(relic));

            _relic =
                relic;
        }


        internal override List<GameEvent> Purchase(
            Run run)
        {
            Guard.NotNull(
                run,
                nameof(run));

            run.AddRelic(_relic);

            return new List<GameEvent>
            {
                new RelicAddedEvent(
                    new RelicInfo(_relic))
            };
        }


        private static string GetName(
            Relic relic)
        {
            Guard.NotNull(
                relic,
                nameof(relic));

            return relic.Name;
        }


        private static string GetDescription(
            Relic relic)
        {
            Guard.NotNull(
                relic,
                nameof(relic));

            return relic.Description;
        }
    }
}
