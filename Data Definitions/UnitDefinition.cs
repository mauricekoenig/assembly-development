using System.Collections.Generic;

namespace GameEngine
{
    public sealed class UnitDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool CanBeRewarded { get; set; }

        public int BaseHealth { get; set; }

        public int EffectSlotCapacity { get; set; }

        public int AbilitySlotCapacity { get; set; }

        public List<UnitTag> Tags { get; set; }

        public List<UnitActionDefinition> Actions { get; set; }

        public List<string> PassiveIds { get; set; }

        public List<string> AbilityIds { get; set; }
    }
}
