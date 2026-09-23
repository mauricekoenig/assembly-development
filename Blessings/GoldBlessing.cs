namespace GameEngine
{
    public class GoldBlessing : BaseBlessing
    {
        private readonly int _amount;

        public GoldBlessing(int amount) : base("Gold Blessing")
        {
            _amount = amount;
        }

        public override void Apply(Run run)
        {
            run.AddCurrency(_amount);
        }
    }
}