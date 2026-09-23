namespace GameEngine
{
    public sealed class FloorInfo
    {
        public int Index { get; }
        public FloorType Type { get; }

        public int StageIndex { get; }
        public int StageNumber => StageIndex + 1;

        public int StageFloorIndex { get; }
        public int StageFloorNumber =>
            StageFloorIndex < 0
                ? 0
                : StageFloorIndex + 1;

        internal FloorInfo(Floor floor)
        {
            Index = floor.Index;
            Type = floor.Type;
            StageIndex = floor.StageIndex;
            StageFloorIndex = floor.StageFloorIndex;
        }

        public override string ToString()
        {
            return
                $"Floor {Index}: Stage {StageNumber}, " +
                $"Stage Floor {StageFloorNumber} - {Type}";
        }
    }
}
