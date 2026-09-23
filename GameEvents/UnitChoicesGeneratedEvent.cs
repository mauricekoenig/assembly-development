
using System.Collections.Generic;

namespace GameEngine
{
    public sealed class UnitChoicesGeneratedEvent : GameEvent
    {
        public IReadOnlyList<UnitInfo> Choices { get; }

        public UnitChoicesGeneratedEvent(IReadOnlyList<UnitInfo> choices)
        {
            Choices = choices;
        }
    }
}