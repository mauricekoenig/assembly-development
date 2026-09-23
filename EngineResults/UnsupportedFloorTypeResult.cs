namespace GameEngine
{
    public sealed class UnsupportedFloorTypeResult : EngineFailedResult
    {
        public FloorType FloorType { get; }

        public UnsupportedFloorTypeResult(FloorType floorType)
            : base($"Floor type '{floorType}' is currently not supported.")
        {
            FloorType = floorType;
        }
    }
}