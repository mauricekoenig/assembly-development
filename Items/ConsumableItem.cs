namespace GameEngine
{
    public abstract class ConsumableItem :
        Item
    {
        protected ConsumableItem(
            string name,
            string description)
            : base(
                name,
                description)
        {
        }
    }
}