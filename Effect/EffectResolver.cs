using System;
using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class EffectResolver
    {
        private static readonly RingPosition[]
            MultiTargetResolutionOrder =
            {
                RingPosition.Right,
                RingPosition.Top,
                RingPosition.Bottom,
                RingPosition.Left
            };


        internal List<GameEvent> Resolve(
            IEffectSource source,
            IReadOnlyList<ActionEffect> effects,
            EffectContext context)
        {
            Guard.NotNull(
                source,
                nameof(source));

            Guard.NotNull(
                effects,
                nameof(effects));

            Guard.NotNull(
                context,
                nameof(context));

            context.Source =
                source;

            List<GameEvent> events =
                new List<GameEvent>();

            foreach (ActionEffect actionEffect
                     in effects)
            {
                if (actionEffect == null)
                    continue;

                ResolveEffect(
                    actionEffect.Effect,
                    actionEffect.Target,
                    context,
                    events);
            }

            return events;
        }


        internal List<GameEvent> Resolve(
            IEffectSource source,
            IReadOnlyList<TriggeredEffect> effects,
            EffectTrigger trigger,
            EffectContext context)
        {
            Guard.NotNull(
                source,
                nameof(source));

            Guard.NotNull(
                effects,
                nameof(effects));

            Guard.NotNull(
                context,
                nameof(context));

            context.Source =
                source;

            List<GameEvent> events =
                new List<GameEvent>();

            foreach (TriggeredEffect triggeredEffect
                     in effects)
            {
                if (triggeredEffect == null ||
                    triggeredEffect.Trigger != trigger)
                {
                    continue;
                }

                ResolveEffect(
                    triggeredEffect.Effect,
                    triggeredEffect.Target,
                    context,
                    events);
            }

            return events;
        }


        private static void ResolveEffect(
            Effect effect,
            EffectTarget target,
            EffectContext context,
            List<GameEvent> events)
        {
            if (effect == null)
                return;

            foreach (EffectComponent component
                     in effect.Components)
            {
                if (component == null)
                    continue;

                switch (target)
                {
                    case EffectTarget.AllLivingUnits:

                        ResolveAllLivingUnits(
                            component,
                            context,
                            events);

                        continue;


                    case EffectTarget.AdjacentFriendlyUnits:

                        ResolveAdjacentFriendlyUnits(
                            component,
                            context,
                            events);

                        continue;
                }

                if (!component.Condition.IsMet(
                        context))
                {
                    continue;
                }

                IEffectTarget resolvedTarget =
                    ResolveSingleTarget(
                        target,
                        context);

                events.AddRange(
                    component.Action.Execute(
                        context,
                        resolvedTarget));
            }
        }


        private static void ResolveAllLivingUnits(
            EffectComponent component,
            EffectContext context,
            List<GameEvent> events)
        {
            if (context.Ring == null)
            {
                throw new InvalidOperationException(
                    "No Ring is available in the effect context for AllLivingUnits targeting.");
            }

            RingObject previousRingObject =
                context.RingObject;

            RingPosition? previousTargetPosition =
                context.TargetPosition;

            try
            {
                foreach (RingPosition position
                         in MultiTargetResolutionOrder)
                {
                    if (!context.Ring.TryGetObject(
                            position,
                            out RingObject ringObject))
                    {
                        continue;
                    }

                    if (!(ringObject is Unit unit) ||
                        !unit.IsAlive)
                    {
                        continue;
                    }

                    context.RingObject =
                        unit;

                    context.TargetPosition =
                        position;

                    if (!component.Condition.IsMet(
                            context))
                    {
                        continue;
                    }

                    events.AddRange(
                        component.Action.Execute(
                            context,
                            unit));
                }
            }
            finally
            {
                context.RingObject =
                    previousRingObject;

                context.TargetPosition =
                    previousTargetPosition;
            }
        }


        private static void ResolveAdjacentFriendlyUnits(
            EffectComponent component,
            EffectContext context,
            List<GameEvent> events)
        {
            if (context.Ring == null)
            {
                throw new InvalidOperationException(
                    "No Ring is available in the effect context for AdjacentFriendlyUnits targeting.");
            }

            if (!(context.Source is Unit sourceUnit))
            {
                throw new InvalidOperationException(
                    "AdjacentFriendlyUnits targeting requires a Unit effect source.");
            }

            if (!context.Ring.TryGetPosition(
                    sourceUnit,
                    out RingPosition sourcePosition))
            {
                throw new InvalidOperationException(
                    "The source Unit is not present on the Ring.");
            }

            GetAdjacentPositions(
                sourcePosition,
                out RingPosition firstPosition,
                out RingPosition secondPosition);

            RingObject previousRingObject =
                context.RingObject;

            RingPosition? previousTargetPosition =
                context.TargetPosition;

            try
            {
                ResolveAdjacentFriendlyUnitAtPosition(
                    component,
                    context,
                    events,
                    sourceUnit,
                    firstPosition);

                ResolveAdjacentFriendlyUnitAtPosition(
                    component,
                    context,
                    events,
                    sourceUnit,
                    secondPosition);
            }
            finally
            {
                context.RingObject =
                    previousRingObject;

                context.TargetPosition =
                    previousTargetPosition;
            }
        }


        private static void ResolveAdjacentFriendlyUnitAtPosition(
            EffectComponent component,
            EffectContext context,
            List<GameEvent> events,
            Unit sourceUnit,
            RingPosition position)
        {
            if (!context.Ring.TryGetObject(
                    position,
                    out RingObject ringObject))
            {
                return;
            }

            if (!(ringObject is Unit targetUnit) ||
                !targetUnit.IsAlive ||
                targetUnit.Team != sourceUnit.Team)
            {
                return;
            }

            context.RingObject =
                targetUnit;

            context.TargetPosition =
                position;

            if (!component.Condition.IsMet(
                    context))
            {
                return;
            }

            events.AddRange(
                component.Action.Execute(
                    context,
                    targetUnit));
        }


        private static void GetAdjacentPositions(
            RingPosition sourcePosition,
            out RingPosition firstPosition,
            out RingPosition secondPosition)
        {
            switch (sourcePosition)
            {
                case RingPosition.Top:
                    firstPosition = RingPosition.Right;
                    secondPosition = RingPosition.Left;
                    return;

                case RingPosition.Right:
                    firstPosition = RingPosition.Bottom;
                    secondPosition = RingPosition.Top;
                    return;

                case RingPosition.Bottom:
                    firstPosition = RingPosition.Left;
                    secondPosition = RingPosition.Right;
                    return;

                case RingPosition.Left:
                    firstPosition = RingPosition.Top;
                    secondPosition = RingPosition.Bottom;
                    return;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(sourcePosition),
                        sourcePosition,
                        "Unsupported ring position.");
            }
        }


        private static IEffectTarget ResolveSingleTarget(
            EffectTarget target,
            EffectContext context)
        {
            return target switch
            {
                EffectTarget.Self =>
                    context.Source as IEffectTarget
                    ?? throw new InvalidOperationException(
                        "The effect source cannot target itself."),

                EffectTarget.Enemy =>
                    context.Enemy
                    ?? throw new InvalidOperationException(
                        "No enemy is available in the effect context."),

                EffectTarget.RingObject =>
                    context.RingObject
                    ?? throw new InvalidOperationException(
                        "No ring object is available in the effect context."),

                EffectTarget.AllLivingUnits =>
                    throw new InvalidOperationException(
                        "AllLivingUnits is a multi-target EffectTarget and must be resolved as a collection."),

                EffectTarget.AdjacentFriendlyUnits =>
                    throw new InvalidOperationException(
                        "AdjacentFriendlyUnits is a multi-target EffectTarget and must be resolved as a collection."),

                _ =>
                    throw new ArgumentOutOfRangeException(
                        nameof(target),
                        target,
                        "Unsupported effect target.")
            };
        }
    }
}
