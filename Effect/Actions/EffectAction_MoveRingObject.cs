using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EffectAction_MoveRingObject :
        EffectAction
    {
        public override List<GameEvent> Execute(
            EffectContext context,
            IEffectTarget target)
        {
            Guard.NotNull(context, nameof(context));
            Guard.NotNull(target, nameof(target));

            List<GameEvent> events = new List<GameEvent>();

            if (!(target is RingObject ringObject))
                return events;

            if (context.Ring == null)
                return events;

            if (!context.TargetPosition.HasValue)
                return events;

            if (!context.Ring.TryGetPosition(
                    ringObject,
                    out RingPosition currentPosition))
            {
                return events;
            }

            RingPosition targetPosition =
                context.TargetPosition.Value;

            if (!context.Ring.TryMoveObject(
                    ringObject,
                    targetPosition))
            {
                return events;
            }

            events.Add(
                new RingRepositionedEvent(
                    currentPosition,
                    targetPosition,
                    new RingInfo(context.Ring)));

            return events;
        }
    }
}