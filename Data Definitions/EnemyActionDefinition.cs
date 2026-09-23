using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EnemyActionDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public EnemyActionTargeting Targeting { get; set; }

        public int DelayTurns { get; set; }

        public List<RingPosition> Positions { get; set; }

        public List<ActionEffectDefinition> Effects { get; set; }
    }
}
