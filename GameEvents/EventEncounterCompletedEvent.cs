using System;

namespace GameEngine
{
    public sealed class EventEncounterCompletedEvent : GameEvent
    {
        public Guid EncounterId { get; }
        public string DefinitionId { get; }
        public string Name { get; }

        internal EventEncounterCompletedEvent(
            Guid encounterId,
            string definitionId,
            string name)
        {
            EncounterId = encounterId;
            DefinitionId = definitionId ?? string.Empty;
            Name = name ?? string.Empty;
        }
    }
}
