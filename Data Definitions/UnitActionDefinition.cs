using System.Collections.Generic;

namespace GameEngine
{
    public sealed class UnitActionDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ActionEffectDefinition> Effects { get; set; }
    }
}
