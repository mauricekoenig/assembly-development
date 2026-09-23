using System.Collections.Generic;

namespace GameEngine
{
    public sealed class RelicDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool CanBeRewarded { get; set; }

        public List<RelicModifierDefinition> Modifiers { get; set; }

        public List<TriggeredEffectDefinition> Effects { get; set; }
    }
}
