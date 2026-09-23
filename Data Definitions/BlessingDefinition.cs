using System.Collections.Generic;

namespace GameEngine
{
    public sealed class BlessingDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool CanBeRewarded { get; set; }

        public List<BlessingEffectDefinition> Effects { get; set; } =
            new List<BlessingEffectDefinition>();
    }
}
