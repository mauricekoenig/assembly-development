using System;
using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EnemyActionTargetsSelectedEvent : GameEvent
    {
        private readonly List<Guid> _targetUnitIds;
        private readonly List<RingPosition> _targetPositions;


        public string EnemyName { get; }
        public string ActionDefinitionId { get; }
        public string ActionName { get; }

        public IReadOnlyList<Guid> TargetUnitIds =>
            _targetUnitIds;

        public IReadOnlyList<RingPosition> TargetPositions =>
            _targetPositions;


        internal EnemyActionTargetsSelectedEvent(
            string enemyName,
            string actionDefinitionId,
            string actionName,
            IEnumerable<Guid> targetUnitIds,
            IEnumerable<RingPosition> targetPositions)
        {
            Guard.NotNullOrWhiteSpace(
                enemyName,
                nameof(enemyName));

            Guard.NotNullOrWhiteSpace(
                actionDefinitionId,
                nameof(actionDefinitionId));

            Guard.NotNullOrWhiteSpace(
                actionName,
                nameof(actionName));


            EnemyName =
                enemyName;

            ActionDefinitionId =
                actionDefinitionId;

            ActionName =
                actionName;

            _targetUnitIds =
                targetUnitIds != null
                    ? new List<Guid>(
                        targetUnitIds)
                    : new List<Guid>();

            _targetPositions =
                targetPositions != null
                    ? new List<RingPosition>(
                        targetPositions)
                    : new List<RingPosition>();
        }
    }
}
