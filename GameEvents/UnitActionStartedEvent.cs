using System;

namespace GameEngine
{
    public sealed class UnitActionStartedEvent : GameEvent
    {
        public Guid UnitId { get; }

        public string UnitDefinitionId { get; }

        public string UnitName { get; }

        public RingPosition SourcePosition { get; }

        public Guid ActionId { get; }

        public string ActionDefinitionId { get; }

        public string ActionName { get; }

        public string ActionDescription { get; }


        internal UnitActionStartedEvent(
            Unit unit,
            RingPosition sourcePosition,
            UnitAction action)
        {
            Guard.NotNull(
                unit,
                nameof(unit));

            Guard.NotNull(
                action,
                nameof(action));

            UnitId =
                unit.Id;

            UnitDefinitionId =
                unit.DefinitionId;

            UnitName =
                unit.Name;

            SourcePosition =
                sourcePosition;

            ActionId =
                action.Id;

            ActionDefinitionId =
                action.DefinitionId;

            ActionName =
                action.Name;

            ActionDescription =
                action.Description;
        }
    }
}
