using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public sealed class FloorGenerationInfo
    {
        public IReadOnlyList<FloorInfo> Floors { get; }

        internal FloorGenerationInfo(Run run)
        {
            Floors = run.Floors
                .Select(floor => new FloorInfo(floor))
                .ToList();
        }

        public override string ToString()
        {
            if (Floors.Count == 0)
                return "No Floors generated.";

            return string.Join(
                "\n",
                Floors.Select(
                    floor => floor.ToString()));
        }
    }
}
