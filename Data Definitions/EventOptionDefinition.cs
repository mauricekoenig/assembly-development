using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EventOptionDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<EventRequirementDefinition> Requirements { get; set; } =
            new List<EventRequirementDefinition>();

        public List<EventOutcomeDefinition> Costs { get; set; } =
            new List<EventOutcomeDefinition>();

        public List<EventOutcomeDefinition> Outcomes { get; set; } =
            new List<EventOutcomeDefinition>();

        public string NextNodeId { get; set; }
    }
}
