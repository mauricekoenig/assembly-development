using System.Collections.Generic;

namespace GameEngine
{
    public sealed class AbilityDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool CanBeItemized { get; set; }

        public int CooldownRounds { get; set; }

        public AbilitySelectionType SelectionType { get; set; }

        public List<AbilitySelectionRuleType> SelectionRules { get; set; }

        public List<ActionEffectDefinition> Effects { get; set; }
    }
}
