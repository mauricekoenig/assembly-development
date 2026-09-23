namespace GameEngine
{
    public class SectorBlessing : BaseBlessing
    {
        private readonly int _amount;

        public SectorType TargetSectorType { get; }

        public SectorBlessing(int amount, SectorType targetSectorType) : base("Sector Blessing")
        {
            _amount = amount;
            TargetSectorType = targetSectorType;
        }

        public override void Apply(Run run)
        {
            if (run == null)
                return;

            switch (TargetSectorType)
            {
                case SectorType.Fire:
                    run.SectorModifiers.AddFireValue(_amount);
                    break;

                case SectorType.Water:
                    run.SectorModifiers.AddWaterValue(_amount);
                    break;

                case SectorType.Grass:
                    run.SectorModifiers.AddGrassValue(_amount);
                    break;

                case SectorType.Cursed:
                    run.SectorModifiers.AddCursedValue(_amount);
                    break;
            }
        }
    }
}