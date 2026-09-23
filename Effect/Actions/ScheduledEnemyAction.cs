using System;

namespace GameEngine
{
    internal sealed class ScheduledEnemyAction
    {
        internal Guid Id { get; } =
            Guid.NewGuid();

        internal Enemy Source { get; }

        internal EnemyAction Action { get; }

        internal int TurnsRemaining { get; private set; }


        internal ScheduledEnemyAction(
            Enemy source,
            EnemyAction action,
            int turnsRemaining)
        {
            Guard.NotNull(
                source,
                nameof(source));

            Guard.NotNull(
                action,
                nameof(action));

            if (turnsRemaining <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(turnsRemaining));
            }


            Source =
                source;

            Action =
                action;

            TurnsRemaining =
                turnsRemaining;
        }


        internal bool AdvanceCountdown()
        {
            TurnsRemaining--;

            return
                TurnsRemaining <= 0;
        }
    }
}
