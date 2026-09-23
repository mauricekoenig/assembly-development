using System.Collections.Generic;

namespace GameEngine
{
    public sealed class RunPlanDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public PoolSelectionDefinition StartingUnitSelection { get; set; }

        // Ordered stage definitions for this run.
        public List<RunStageDefinition> Stages { get; set; } =
            new List<RunStageDefinition>();
    }
}
