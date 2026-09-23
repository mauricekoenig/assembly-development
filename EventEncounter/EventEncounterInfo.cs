using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public sealed class EventEncounterInfo
    {
        public System.Guid Id { get; }
        public string DefinitionId { get; }
        public string Name { get; }
        public string Description { get; }

        public string CurrentNodeId { get; }
        public EventNodeType CurrentNodeType { get; }
        public string CurrentNodeTitle { get; }
        public string CurrentNodeDescription { get; }

        public string ActivityId { get; }
        public string ActivityContentId { get; }
        public IReadOnlyList<string> ActivityResultIds { get; }

        public IReadOnlyList<EventOptionInfo> Options { get; }

        internal EventEncounterInfo(
            DataDrivenEventEncounter encounter,
            Run run)
        {
            Guard.NotNull(encounter, nameof(encounter));
            Guard.NotNull(run, nameof(run));

            Id = encounter.Id;
            DefinitionId = encounter.DefinitionId;
            Name = encounter.Name;
            Description = encounter.Description;

            CurrentNodeId = encounter.CurrentNodeId ?? string.Empty;
            CurrentNodeType = encounter.CurrentNodeType;
            CurrentNodeTitle = encounter.CurrentNode?.Title ?? string.Empty;
            CurrentNodeDescription = encounter.CurrentNode?.Description ?? string.Empty;

            ActivityId = encounter.ActivityId;
            ActivityContentId = encounter.ActivityContentId;

            ActivityResultIds =
                encounter.CurrentNodeType == EventNodeType.Activity
                    ? (encounter.CurrentNode.ActivityResults ??
                        new List<EventActivityResultDefinition>())
                        .Where(result => result != null)
                        .Select(result => result.ResultId)
                        .ToList()
                    : new List<string>();

            Options = encounter.Options
                .Select(option => new EventOptionInfo(option, run))
                .ToList();
        }
    }
}
