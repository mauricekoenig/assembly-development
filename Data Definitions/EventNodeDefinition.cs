using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EventNodeDefinition
    {
        public string Id { get; set; }

        public EventNodeType Type { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<EventOptionDefinition> Options { get; set; } =
            new List<EventOptionDefinition>();

        public List<EventOutcomeDefinition> Outcomes { get; set; } =
            new List<EventOutcomeDefinition>();

        public string NextNodeId { get; set; }

        public string ActivityId { get; set; }

        public string ActivityContentId { get; set; }

        public List<EventActivityResultDefinition> ActivityResults { get; set; } =
            new List<EventActivityResultDefinition>();
    }
}
