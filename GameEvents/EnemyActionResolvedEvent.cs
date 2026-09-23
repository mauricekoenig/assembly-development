namespace GameEngine
{
    public sealed class EnemyActionResolvedEvent : GameEvent
    {
        public string EnemyName { get; }
        public string ActionDefinitionId { get; }
        public string ActionName { get; }


        internal EnemyActionResolvedEvent(
            string enemyName,
            string actionDefinitionId,
            string actionName)
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
        }
    }
}
