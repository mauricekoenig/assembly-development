using System;

namespace GameEngine
{
    public sealed class EnemyActionStartedEvent : GameEvent
    {
        public string EnemyName { get; }

        public string ActionDefinitionId { get; }

        public string ActionName { get; }

        public string ActionDescription { get; }

        public Guid? ScheduledActionId { get; }


        internal EnemyActionStartedEvent(
            string enemyName,
            string actionDefinitionId,
            string actionName,
            string actionDescription,
            Guid? scheduledActionId)
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


            EnemyName =
                enemyName;

            ActionDefinitionId =
                actionDefinitionId;

            ActionName =
                actionName;

            ActionDescription =
                actionDescription;

            ScheduledActionId =
                scheduledActionId;
        }
    }
}
