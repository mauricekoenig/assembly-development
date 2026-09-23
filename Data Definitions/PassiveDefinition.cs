using System.Collections.Generic;

namespace GameEngine
{
    public sealed class PassiveDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool CanBeItemized { get; set; }

        public List<PassiveModifierDefinition> Modifiers { get; set; }

        public List<TriggeredEffectDefinition> Effects { get; set; }
    }
}
