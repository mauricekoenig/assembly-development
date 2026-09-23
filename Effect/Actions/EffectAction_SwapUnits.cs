using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EffectAction_SwapUnits :
        EffectAction
    {
        public override List<GameEvent> Execute(
            EffectContext context,
            IEffectTarget target)
        {
            Guard.NotNull(context, nameof(context));
            Guard.NotNull(target, nameof(target));

            List<GameEvent> events = new List<GameEvent>();

            if (!(target is Unit firstUnit))
                return events;

            if (!(context.SecondRingObject is Unit secondUnit))
                return events;

            if (context.Ring == null)
                return events;

            if (!context.Ring.TryGetPosition(
                    firstUnit,
                    out RingPosition firstPosition))
            {
                return events;
            }

            if (!context.Ring.TryGetPosition(
                    secondUnit,
                    out RingPosition secondPosition))
            {
                return events;
            }

            if (!context.Ring.TryReposition(
                    firstPosition,
                    secondPosition))
            {
                return events;
            }

            events.Add(
                new RingRepositionedEvent(
                    firstPosition,
                    secondPosition,
                    new RingInfo(context.Ring)));

            return events;
        }
    }
}