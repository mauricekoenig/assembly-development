using System;

namespace GameEngine
{
    public sealed class EnemyActionCountdownEvent : GameEvent
    {
        public Guid ScheduledActionId { get; }

        public string EnemyName { get; }

        public string ActionDefinitionId { get; }

        public string ActionName { get; }

        public string ActionDescription { get; }

        public int TurnsRemaining { get; }


        internal EnemyActionCountdownEvent(
            string enemyName,
            string actionDefinitionId,
            string actionName,
            string actionDescription,
            int turnsRemaining,
            Guid scheduledActionId)
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

            Guard.NotNullOrWhiteSpace(
                actionDescription,
                nameof(actionDescription));

            if (turnsRemaining <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(turnsRemaining));
            }


            EnemyName =
                enemyName;

            ActionDefinitionId =
                actionDefinitionId;

            ActionName =
                actionName;

            ActionDescription =
                actionDescription;

            TurnsRemaining =
                turnsRemaining;

            ScheduledActionId =
                scheduledActionId;
        }
    }
}
