using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class BlessingShopOffer :
        ShopOffer
    {
        private readonly IBlessing _blessing;


        internal BlessingShopOffer(
            string contentId,
            IBlessing blessing,
            int price)
            : base(
                VendorOfferType.Blessing,
                contentId,
                GetName(blessing),
                GetDescription(blessing),
                price)
        {
            Guard.NotNull(
                blessing,
                nameof(blessing));

            _blessing =
                blessing;
        }


        internal override List<GameEvent> Purchase(
            Run run)
        {
            Guard.NotNull(
                run,
                nameof(run));

            run.AddBlessing(_blessing);

            return new List<GameEvent>
            {
                new BlessingAddedEvent(
                    new BlessingInfo(_blessing))
            };
        }


        private static string GetName(
            IBlessing blessing)
        {
            Guard.NotNull(
                blessing,
                nameof(blessing));

            return blessing.Name;
        }


        private static string GetDescription(
            IBlessing blessing)
        {
            Guard.NotNull(
                blessing,
                nameof(blessing));

            return string.IsNullOrWhiteSpace(
                    blessing.Description)
                ? "Add this blessing to your run."
                : blessing.Description;
        }
    }
}
