using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EventDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string StartNodeId { get; set; }

        public List<EventNodeDefinition> Nodes { get; set; } =
            new List<EventNodeDefinition>();
    }
}
