using System.Collections.Generic;

namespace GameEngine
{
    public sealed class RunStageDefinition
    {
        public string EnemyPoolId { get; set; }

        public string BossPoolId { get; set; }

        public string EventPoolId { get; set; }

        public string VendorPoolId { get; set; }

        public List<FloorType> FloorTypes { get; set; } =
            new List<FloorType>();
    }
}
