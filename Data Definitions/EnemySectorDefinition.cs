using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EnemySectorDefinition
    {
        public RingPosition Position { get; set; }

        public string Name { get; set; }

        public SectorType SectorType { get; set; }

        public List<ActionEffectDefinition> Effects { get; set; }
    }
}
