using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EventActivityResultDefinition
    {
        public string ResultId { get; set; }

        public List<EventOutcomeDefinition> Outcomes { get; set; } =
            new List<EventOutcomeDefinition>();

        public string NextNodeId { get; set; }
    }
}
