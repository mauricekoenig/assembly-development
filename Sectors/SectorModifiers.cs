namespace GameEngine
{
    public sealed class SectorModifiers
    {
        public int FireValue { get; private set; }
        public int WaterValue { get; private set; }
        public int HealValue { get; private set; }

        public int CursedValue { get; private set; }

        public void AddCursedValue(int amount)
        {
            CursedValue += amount;
        }

        public void AddFireValue(int amount)
        {
            FireValue += amount;
        }

        public void AddWaterValue(int amount)
        {
            WaterValue += amount;
        }

        public void AddGrassValue(int amount)
        {
            HealValue += amount;
        }

        internal int GetValue(
            SectorType sectorType)
        {
            return sectorType switch
            {
                SectorType.Fire => FireValue,
                SectorType.Water => WaterValue,
                SectorType.Grass => HealValue,
                SectorType.Cursed => CursedValue,
                _ => 0
            };
        }
    }
}