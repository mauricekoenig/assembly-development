namespace GameEngine
{
    public sealed class ShopEncounterStartedEvent :
        GameEvent
    {
        public ShopEncounterInfo Shop { get; }

        public ShopEncounterStartedEvent(
            ShopEncounterInfo shop)
        {
            Shop = shop;
        }
    }
}