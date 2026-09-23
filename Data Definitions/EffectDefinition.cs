using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EffectDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool CanBeItemized { get; set; }

        // Optional metadata used only when this Effect is turned into an
        // Effect Recipe. Targeting remains outside the Effect mechanics.
        public EffectItemizationDefinition Itemization { get; set; }

        public List<EffectComponentDefinition> Components { get; set; }
    }
}
