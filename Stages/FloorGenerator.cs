using System;
using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class FloorGenerator
    {
        internal List<Floor> GenerateFloors(
            RunPlanDefinition runPlan)
        {
            if (runPlan == null)
                throw new ArgumentNullException(nameof(runPlan));

            if (runPlan.Stages == null ||
                runPlan.Stages.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Run Plan '{runPlan.Id}' must contain at least one Stage.");
            }

            List<Floor> floors =
                new List<Floor>();

            int globalFloorIndex = 0;

            for (int stageIndex = 0;
                 stageIndex < runPlan.Stages.Count;
                 stageIndex++)
            {
                RunStageDefinition stage =
                    runPlan.Stages[stageIndex];

                if (stage?.FloorTypes == null ||
                    stage.FloorTypes.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} has no Floors.");
                }

                for (int stageFloorIndex = 0;
                     stageFloorIndex < stage.FloorTypes.Count;
                     stageFloorIndex++)
                {
                    floors.Add(
                        new Floor(
                            globalFloorIndex++,
                            stage.FloorTypes[stageFloorIndex],
                            stageIndex,
                            stageFloorIndex));
                }

            }

            return floors;
        }
    }
}
