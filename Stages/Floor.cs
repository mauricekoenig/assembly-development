namespace GameEngine
{
    public sealed class Floor
    {
        public int Index { get; }
        public FloorType Type { get; }

        // Zero-based stage index this floor belongs to.
        public int StageIndex { get; }

        // Zero-based index inside the stage.
        public int StageFloorIndex { get; }

        public Floor(
            int index,
            FloorType type)
            : this(
                index,
                type,
                0,
                0)
        {
        }

        public Floor(
            int index,
            FloorType type,
            int stageIndex,
            int stageFloorIndex)
        {
            Index = index;
            Type = type;
            StageIndex = stageIndex;
            StageFloorIndex = stageFloorIndex;
        }
    }
}
