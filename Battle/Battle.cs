using GameEngine;
using System;
using System.Collections.Generic;

internal sealed class Battle
{
    // =========================================================
    // RESOLUTION
    // =========================================================

    private static readonly RingPosition[] ResolutionOrder =
    {
        RingPosition.Right,
        RingPosition.Top,
        RingPosition.Bottom,
        RingPosition.Left
    };


    // =========================================================
    // DEPENDENCIES
    // =========================================================

    private readonly Run _run;

    private readonly EffectResolver _effectResolver =
        new EffectResolver();

    private readonly List<ScheduledEnemyAction>
        _scheduledEnemyActions =
            new List<ScheduledEnemyAction>();


    // =========================================================
    // STATE
    // =========================================================

    internal Enemy Enemy { get; }

    internal Ring Ring =>
        _run.Ring;

    internal SectorLayout SectorLayout { get; }

    internal bool IsWon =>
        !Enemy.IsAlive;


    private const int BaseActiveAbilityUsesPerRound =
        1;


    // Repositioning is intentionally unlimited.
    // Keep the legacy query surface compatible for callers that still
    // inspect RepositionsRemaining, but never consume this value.
    internal int RepositionsRemaining =>
        int.MaxValue;

    internal int ActiveAbilityUsesRemaining { get; private set; } =
        BaseActiveAbilityUsesPerRound;


    // =========================================================
    // CONSTRUCTOR
    // =========================================================

    internal Battle(
        Run run,
        Enemy enemy)
    {
        Guard.NotNull(
            run,
            nameof(run));

        Guard.NotNull(
            enemy,
            nameof(enemy));

        _run =
            run;

        Enemy =
            enemy;

        SectorLayout =
            enemy.CreateSectorLayout();
    }


    // =========================================================
    // COMMANDS
    // =========================================================

    internal bool TryRotate(
        RotationDirection direction)
    {
        return Ring.TryRotate(
            direction);
    }


    internal bool TryReposition(
        RingPosition firstPosition,
        RingPosition secondPosition)
    {
        return Ring.TryReposition(
            firstPosition,
            secondPosition);
    }


    internal bool CanUseUnitAbility(
        Guid unitId,
        AbilityContext abilityContext)
    {
        Guard.NotNull(
            abilityContext,
            nameof(abilityContext));

        if (ActiveAbilityUsesRemaining <= 0)
            return false;

        if (!TryGetLivingUnit(
                unitId,
                out Unit owner))
        {
            return false;
        }

        ActiveAbility ability =
            owner.ActiveAbility;

        if (ability == null ||
            !ability.IsReady)
        {
            return false;
        }

        if (!IsValidAbilityContext(
                ability,
                abilityContext))
        {
            return false;
        }

        return IsValidAbilitySelection(
            owner,
            ability,
            abilityContext);
    }


    internal bool TryUseUnitAbility(
        Guid unitId,
        AbilityContext abilityContext,
        out Guid abilityId,
        out List<GameEvent> events)
    {
        Guard.NotNull(
            abilityContext,
            nameof(abilityContext));

        abilityId =
            Guid.Empty;

        events =
            new List<GameEvent>();

        if (!TryGetLivingUnit(
                unitId,
                out Unit owner))
        {
            return false;
        }

        ActiveAbility ability =
            owner.ActiveAbility;

        if (ability == null)
            return false;

        abilityId =
            ability.Id;

        return TryUseAbility(
            ability.Id,
            abilityContext,
            out events);
    }


    internal bool TryUseAbility(
        Guid abilityId,
        AbilityContext abilityContext,
        out List<GameEvent> events)
    {
        Guard.NotNull(
            abilityContext,
            nameof(abilityContext));

        events =
            new List<GameEvent>();


        // =========================================================
        // PLAYER WINDOW
        // =========================================================

        if (ActiveAbilityUsesRemaining <= 0)
        {
            return false;
        }


        // =========================================================
        // FIND ABILITY + OWNER
        // =========================================================

        if (!TryGetAbility(
                abilityId,
                out Unit owner,
                out ActiveAbility ability))
        {
            return false;
        }

        if (!ability.IsReady)
        {
            return false;
        }


        // =========================================================
        // CONTEXT SHAPE
        // =========================================================

        if (!IsValidAbilityContext(
                ability,
                abilityContext))
        {
            return false;
        }


        // =========================================================
        // PLAYER SELECTION
        // =========================================================

        RingObject selectedRingObject =
            null;

        RingObject secondSelectedRingObject =
            null;

        RingPosition? selectedPosition =
            null;


        switch (ability.SelectionType)
        {
            case AbilitySelectionType.None:
                break;


            case AbilitySelectionType.Unit:

                if (!TryGetLivingUnit(
                        abilityContext.TargetUnitId.Value,
                        out Unit targetUnit))
                {
                    return false;
                }

                if (!ability.IsValidSelection(
                        owner,
                        targetUnit))
                {
                    return false;
                }

                selectedRingObject =
                    targetUnit;

                break;


            case AbilitySelectionType.UnitAndRingPosition:

                if (!TryGetLivingUnit(
                        abilityContext.TargetUnitId.Value,
                        out Unit movingUnit))
                {
                    return false;
                }

                RingPosition targetPosition =
                    abilityContext.TargetPosition.Value;

                if (!ability.IsValidSelection(
                        owner,
                        movingUnit,
                        targetPosition,
                        Ring))
                {
                    return false;
                }

                selectedRingObject =
                    movingUnit;

                selectedPosition =
                    targetPosition;

                break;


            case AbilitySelectionType.TwoUnits:

                if (!TryGetLivingUnit(
                        abilityContext.TargetUnitId.Value,
                        out Unit firstTarget))
                {
                    return false;
                }

                if (!TryGetLivingUnit(
                        abilityContext.SecondUnitId.Value,
                        out Unit secondTarget))
                {
                    return false;
                }

                if (!ability.IsValidSelection(
                        owner,
                        firstTarget,
                        secondTarget))
                {
                    return false;
                }

                selectedRingObject =
                    firstTarget;

                secondSelectedRingObject =
                    secondTarget;

                break;


            default:

                return false;
        }


        // =========================================================
        // EFFECT RESOLUTION
        // =========================================================

        EffectContext effectContext =
            new EffectContext
            {
                Enemy =
                    Enemy,

                Ring =
                    Ring,

                RingObject =
                    selectedRingObject,

                SecondRingObject =
                    secondSelectedRingObject,

                TargetPosition =
                    selectedPosition,

                Run =
                    _run
            };


        List<GameEvent> resolvedEvents =
            _effectResolver.Resolve(
                owner,
                ability.Effects,
                effectContext);


        // =========================================================
        // COMMIT USE
        // =========================================================

        ability.StartCooldown();

        ActiveAbilityUsesRemaining--;


        // =========================================================
        // EVENTS
        // =========================================================

        events.Add(
            new ActiveAbilityUsedEvent(
                owner,
                ability));

        events.AddRange(
            resolvedEvents);

        events.AddRange(
            ResolveRelicTrigger(
                EffectTrigger.ActiveAbilityActivated,
                selectedRingObject,
                selectedPosition));


        if (!Enemy.IsAlive)
        {
            events.Add(
                new BattleWonEvent(
                    new BattleInfo(this)));
        }

        return true;
    }


    internal List<GameEvent> ResolveRing()
    {
        List<GameEvent> events =
            new List<GameEvent>();


        // The high-level resolution lifecycle is emitted explicitly so
        // consumers never have to infer battle phases from damage/heal
        // events or from the concrete actions that happen inside them.
        events.Add(
            new RingResolutionStartedEvent());


        if (_scheduledEnemyActions.Count > 0)
        {
            events.Add(
                new ScheduledEnemyActionsStartedEvent());

            ResolveScheduledEnemyActions(
                events);

            events.Add(
                new ScheduledEnemyActionsResolvedEvent());
        }


        if (_run.IsTeamDefeated)
        {
            events.Add(
                new RingResolutionResolvedEvent());

            return events;
        }


        events.Add(
            new PlayerRingResolutionStartedEvent());


        foreach (RingPosition position
                 in ResolutionOrder)
        {
            ResolvePosition(
                position,
                events);

            if (!Enemy.IsAlive)
            {
                break;
            }
        }


        events.Add(
            new PlayerRingResolutionResolvedEvent());


        if (Enemy.IsAlive &&
            !_run.IsTeamDefeated)
        {
            events.Add(
                new EnemyPhaseStartedEvent());

            ResolveEnemyPhase(
                events);

            events.Add(
                new EnemyPhaseResolvedEvent());
        }


        // End-of-turn passives resolve only while the battle is still
        // active after the normal Enemy Phase.
        //
        // Priority is deliberately deterministic:
        // 1) Player Units in normal ring resolution order
        // 2) Enemy
        if (Enemy.IsAlive &&
            !_run.IsTeamDefeated)
        {
            events.Add(
                new EndOfTurnStartedEvent());

            ResolveEndOfTurn(
                events);

            events.Add(
                new EndOfTurnResolvedEvent());
        }


        if (!Enemy.IsAlive)
        {
            events.Add(
                new BattleWonEvent(
                    new BattleInfo(this)));
        }


        if (Enemy.IsAlive &&
            !_run.IsTeamDefeated)
        {
            OpenNextPlayerWindow();
        }


        events.Add(
            new RingResolutionResolvedEvent());


        return events;
    }


    // =========================================================
    // RESOLUTION INTERNALS
    // =========================================================

    private bool TryGetLivingUnit(
        Guid unitId,
        out Unit unit)
    {
        foreach (Unit candidate
                 in GetLivingUnits())
        {
            if (candidate.Id != unitId)
            {
                continue;
            }

            unit =
                candidate;

            return true;
        }


        unit =
            null;

        return false;
    }


    private void OpenNextPlayerWindow()
    {
        ActiveAbilityUsesRemaining =
            BaseActiveAbilityUsesPerRound;

        AdvanceAbilityCooldowns();
    }


    private void AdvanceAbilityCooldowns()
    {
        foreach (Unit unit
                 in GetLivingUnits())
        {
            unit.ActiveAbility?.AdvanceCooldown();
        }
    }


    private bool IsValidAbilityContext(
        ActiveAbility ability,
        AbilityContext context)
    {
        return ability.SelectionType switch
        {
            AbilitySelectionType.None =>
                context.TargetUnitId == null &&
                context.SecondUnitId == null &&
                context.TargetPosition == null,

            AbilitySelectionType.Unit =>
                context.TargetUnitId != null &&
                context.SecondUnitId == null &&
                context.TargetPosition == null,

            AbilitySelectionType.UnitAndRingPosition =>
                context.TargetUnitId != null &&
                context.SecondUnitId == null &&
                context.TargetPosition != null,

            AbilitySelectionType.TwoUnits =>
                context.TargetUnitId != null &&
                context.SecondUnitId != null &&
                context.TargetPosition == null,

            _ =>
                false
        };
    }


    private bool IsValidAbilitySelection(
        Unit owner,
        ActiveAbility ability,
        AbilityContext context)
    {
        switch (ability.SelectionType)
        {
            case AbilitySelectionType.None:
                return true;


            case AbilitySelectionType.Unit:

                return
                    TryGetLivingUnit(
                        context.TargetUnitId.Value,
                        out Unit targetUnit) &&
                    ability.IsValidSelection(
                        owner,
                        targetUnit);


            case AbilitySelectionType.UnitAndRingPosition:

                if (!TryGetLivingUnit(
                        context.TargetUnitId.Value,
                        out Unit movingUnit))
                {
                    return false;
                }

                return ability.IsValidSelection(
                    owner,
                    movingUnit,
                    context.TargetPosition.Value,
                    Ring);


            case AbilitySelectionType.TwoUnits:

                if (!TryGetLivingUnit(
                        context.TargetUnitId.Value,
                        out Unit firstTarget) ||
                    !TryGetLivingUnit(
                        context.SecondUnitId.Value,
                        out Unit secondTarget))
                {
                    return false;
                }

                return ability.IsValidSelection(
                    owner,
                    firstTarget,
                    secondTarget);


            default:
                return false;
        }
    }


    private bool TryGetAbility(
        Guid abilityId,
        out Unit owner,
        out ActiveAbility ability)
    {
        foreach (Unit unit
                 in GetLivingUnits())
        {
            ActiveAbility candidate =
                unit.ActiveAbility;

            if (candidate == null ||
                candidate.Id != abilityId)
            {
                continue;
            }

            owner =
                unit;

            ability =
                candidate;

            return true;
        }


        owner =
            null;

        ability =
            null;

        return false;
    }


    // =========================================================
    // ENEMY ACTION SCHEDULING
    // =========================================================

    private void ScheduleEnemyAction(
        Enemy source,
        EnemyAction action,
        int delayTurns,
        List<GameEvent> events)
    {
        Guard.NotNull(
            source,
            nameof(source));

        Guard.NotNull(
            action,
            nameof(action));

        Guard.NotNull(
            events,
            nameof(events));


        if (delayTurns <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(delayTurns));
        }


        ScheduledEnemyAction scheduledAction =
            new ScheduledEnemyAction(
                source,
                action,
                delayTurns);


        _scheduledEnemyActions.Add(
            scheduledAction);


        events.Add(
            new EnemyActionScheduledEvent(
                source.Name,
                action.DefinitionId,
                action.Name,
                action.Description,
                delayTurns,
                scheduledAction.Id));
    }


    // =========================================================
    // RING RESOLUTION
    // =========================================================

    private void ResolvePosition(
        RingPosition position,
        List<GameEvent> events)
    {
        if (!Ring.TryGetObject(
                position,
                out RingObject ringObject))
        {
            return;
        }


        if (!(ringObject is Unit unit) ||
            !unit.IsAlive)
        {
            return;
        }


        // =====================================================
        // OPTIONAL SECTOR EFFECTS
        // =====================================================

        if (SectorLayout.TryGetSector(
                position,
                out Sector sector))
        {
            events.Add(
                new SectorActivationStartedEvent(
                    position,
                    sector.SectorType));


            EffectContext sectorContext =
                new EffectContext
                {
                    Enemy =
                        Enemy,

                    Ring =
                        Ring,

                    RingObject =
                        unit,

                    TargetPosition =
                        position,

                    Run =
                        _run
                };


            events.AddRange(
                _effectResolver.Resolve(
                    sector,
                    sector.Effects,
                    sectorContext));


            if (!unit.IsAlive ||
                !Enemy.IsAlive ||
                _run.IsTeamDefeated)
            {
                return;
            }


            events.AddRange(
                ResolveRelicTrigger(
                    EffectTrigger.SectorActivated,
                    unit,
                    position));


            if (!Enemy.IsAlive ||
                _run.IsTeamDefeated)
            {
                return;
            }
        }


        // =====================================================
        // UNIT ACTIONS
        // =====================================================
        // A Unit has no RingObject Effect stack anymore. Every
        // ability that fires during Ring Resolve is a UnitAction.
        // Starting and run-added Actions live in the same list and
        // pass through this exact same Started -> Effects -> Resolved
        // pipeline.

        ResolveUnitActions(
            unit,
            position,
            events);


        if (!Enemy.IsAlive ||
            _run.IsTeamDefeated)
        {
            return;
        }


        // RingObjectActivated remains available as a generic hook for
        // systems such as Relics that react once to the resolved slot.
        // It no longer resolves Effects owned directly by the Unit.
        events.AddRange(
            ResolveRelicTrigger(
                EffectTrigger.RingObjectActivated,
                unit,
                position));
    }


    // =========================================================
    // UNIT ACTIONS
    // =========================================================

    private void ResolveUnitActions(
        Unit source,
        RingPosition sourcePosition,
        List<GameEvent> events)
    {
        Guard.NotNull(
            source,
            nameof(source));

        Guard.NotNull(
            events,
            nameof(events));


        foreach (UnitAction action
                 in source.Actions)
        {
            if (action == null ||
                !source.IsAlive ||
                !Enemy.IsAlive ||
                _run.IsTeamDefeated)
            {
                break;
            }

            ResolveUnitAction(
                source,
                sourcePosition,
                action,
                events);
        }
    }


    private void ResolveUnitAction(
        Unit source,
        RingPosition sourcePosition,
        UnitAction action,
        List<GameEvent> events)
    {
        Guard.NotNull(
            source,
            nameof(source));

        Guard.NotNull(
            action,
            nameof(action));

        Guard.NotNull(
            events,
            nameof(events));


        events.Add(
            new UnitActionStartedEvent(
                source,
                sourcePosition,
                action));


        EffectContext actionContext =
            new EffectContext
            {
                Enemy =
                    Enemy,

                Ring =
                    Ring,

                RingObject =
                    source,

                TargetPosition =
                    sourcePosition,

                Run =
                    _run
            };


        events.AddRange(
            _effectResolver.Resolve(
                source,
                action.Effects,
                actionContext));


        // Passives and Relics may react to the action as a trigger, but
        // they do not become part of the Unit's Action collection.
        ResolvePassiveTrigger(
            source,
            source.Passives,
            EffectTrigger.UnitActionActivated,
            source,
            events,
            sourcePosition);


        if (Enemy.IsAlive &&
            !_run.IsTeamDefeated)
        {
            events.AddRange(
                ResolveRelicTrigger(
                    EffectTrigger.UnitActionActivated,
                    source,
                    sourcePosition));
        }


        events.Add(
            new UnitActionResolvedEvent(
                source,
                sourcePosition,
                action));
    }


    // =========================================================
    // END OF TURN
    // =========================================================

    private void ResolveEndOfTurn(
        List<GameEvent> events)
    {
        // Player side first, using the same deterministic order as the
        // normal Ring resolution.
        foreach (RingPosition position
                 in ResolutionOrder)
        {
            if (!Ring.TryGetObject(
                    position,
                    out RingObject ringObject))
            {
                continue;
            }

            Unit unit =
                ringObject as Unit;

            if (unit == null ||
                !unit.IsAlive)
            {
                continue;
            }

            ResolvePassiveTrigger(
                unit,
                unit.Passives,
                EffectTrigger.TurnEnded,
                unit,
                events,
                position);
        }

        // The player side owns priority for the whole End Of Turn phase.
        // If its passives defeat the Enemy, the Enemy does not get an
        // additional End Of Turn response.
        if (!Enemy.IsAlive ||
            _run.IsTeamDefeated)
        {
            return;
        }

        ResolvePassiveTrigger(
            Enemy,
            Enemy.Passives,
            EffectTrigger.TurnEnded,
            null,
            events);

        if (!Enemy.IsAlive ||
            _run.IsTeamDefeated)
        {
            return;
        }

        events.AddRange(
            ResolveRelicTrigger(
                EffectTrigger.TurnEnded));
    }


    internal List<GameEvent> ResolveRelicTrigger(
        EffectTrigger trigger,
        RingObject contextualRingObject = null,
        RingPosition? targetPosition = null)
    {
        List<GameEvent> events =
            new List<GameEvent>();

        if (_run.Relics == null ||
            _run.Relics.Count == 0)
        {
            return events;
        }

        EffectContext context =
            new EffectContext
            {
                Enemy = Enemy,
                Ring = Ring,
                RingObject = contextualRingObject,
                TargetPosition = targetPosition,
                Run = _run
            };

        foreach (Relic relic
                 in _run.Relics)
        {
            if (relic == null ||
                relic.Effects == null ||
                relic.Effects.Count == 0)
            {
                continue;
            }

            events.AddRange(
                _effectResolver.Resolve(
                    relic,
                    relic.Effects,
                    trigger,
                    context));
        }

        return events;
    }


    private void ResolvePassiveTrigger(
        IEffectSource owner,
        IReadOnlyList<Passive> passives,
        EffectTrigger trigger,
        RingObject contextualRingObject,
        List<GameEvent> events,
        RingPosition? targetPosition = null)
    {
        if (owner == null ||
            passives == null ||
            passives.Count == 0)
        {
            return;
        }

        EffectContext context =
            new EffectContext
            {
                Enemy =
                    Enemy,

                Ring =
                    Ring,

                RingObject =
                    contextualRingObject,

                TargetPosition =
                    targetPosition,

                Run =
                    _run
            };

        foreach (Passive passive
                 in passives)
        {
            if (passive == null ||
                passive.Effects == null ||
                passive.Effects.Count == 0)
            {
                continue;
            }

            events.AddRange(
                _effectResolver.Resolve(
                    owner,
                    passive.Effects,
                    trigger,
                    context));
        }
    }


    // =========================================================
    // ENEMY PHASE
    // =========================================================

    private void ResolveEnemyPhase(
        List<GameEvent> events)
    {
        if (!Enemy.IsAlive)
        {
            return;
        }


        if (_run.IsTeamDefeated)
        {
            return;
        }


        if (Enemy.Actions.Count == 0)
        {
            return;
        }


        EnemyAction action =
            Enemy.Actions[
                _run.random.Next(
                    Enemy.Actions.Count)];


        if (action.IsScheduled)
        {
            ScheduleEnemyAction(
                Enemy,
                action,
                action.DelayTurns,
                events);
        }
        else
        {
            ResolveEnemyAction(
                Enemy,
                action,
                events);
        }
    }


    private void ResolveEnemyAction(
        Enemy source,
        EnemyAction action,
        List<GameEvent> events,
        Guid? scheduledActionId = null)
    {
        Guard.NotNull(
            source,
            nameof(source));

        Guard.NotNull(
            action,
            nameof(action));

        Guard.NotNull(
            events,
            nameof(events));


        events.Add(
            new EnemyActionStartedEvent(
                source.Name,
                action.DefinitionId,
                action.Name,
                action.Description,
                scheduledActionId));


        switch (action.Targeting)
        {
            // =====================================================
            // NO RING TARGET
            // =====================================================

            case EnemyActionTargeting.None:

                ResolveEnemyActionEffects(
                    source,
                    action,
                    null,
                    events);

                break;


            // =====================================================
            // RANDOM LIVING UNIT
            // =====================================================

            case EnemyActionTargeting.RandomLivingUnit:

                Unit randomTarget =
                    GetRandomLivingUnit();

                if (randomTarget != null)
                {
                    events.Add(
                        new EnemyActionTargetsSelectedEvent(
                            source.Name,
                            action.DefinitionId,
                            action.Name,
                            new[]
                            {
                                randomTarget.Id
                            },
                            null));


                    ResolveEnemyActionEffects(
                        source,
                        action,
                        randomTarget,
                        events);
                }

                break;


            // =====================================================
            // ALL LIVING UNITS
            // =====================================================

            case EnemyActionTargeting.AllLivingUnits:

                List<Unit> livingUnits =
                    GetLivingUnits();

                if (livingUnits.Count > 0)
                {
                    List<Guid> targetUnitIds =
                        new List<Guid>();

                    foreach (Unit unit
                             in livingUnits)
                    {
                        targetUnitIds.Add(
                            unit.Id);
                    }


                    events.Add(
                        new EnemyActionTargetsSelectedEvent(
                            source.Name,
                            action.DefinitionId,
                            action.Name,
                            targetUnitIds,
                            null));
                }


                foreach (Unit unit
                         in livingUnits)
                {
                    ResolveEnemyActionEffects(
                        source,
                        action,
                        unit,
                        events);
                }

                break;


            // =====================================================
            // SPECIFIC RING POSITIONS
            // =====================================================

            case EnemyActionTargeting.RingPositions:

                ResolveRingPositionTargets(
                    source,
                    action,
                    events);

                break;


            default:

                throw new ArgumentOutOfRangeException(
                    nameof(action.Targeting),
                    action.Targeting,
                    "Unsupported enemy action targeting.");
        }


        events.Add(
            new EnemyActionResolvedEvent(
                source.Name,
                action.DefinitionId,
                action.Name));
    }


    private void ResolveRingPositionTargets(
        Enemy source,
        EnemyAction action,
        List<GameEvent> events)
    {
        List<Guid> targetUnitIds =
            new List<Guid>();

        List<RingPosition> targetPositions =
            new List<RingPosition>();

        List<RingObject> resolvedTargets =
            new List<RingObject>();

        List<RingPosition> resolvedTargetPositions =
            new List<RingPosition>();


        foreach (RingPosition position
                 in action.Positions)
        {
            targetPositions.Add(
                position);


            if (!Ring.TryGetObject(
                    position,
                    out RingObject ringObject))
            {
                continue;
            }


            if (ringObject is Unit unit)
            {
                if (!unit.IsAlive)
                {
                    continue;
                }


                targetUnitIds.Add(
                    unit.Id);
            }


            resolvedTargets.Add(
                ringObject);

            resolvedTargetPositions.Add(
                position);
        }


        if (targetUnitIds.Count > 0 ||
            targetPositions.Count > 0)
        {
            events.Add(
                new EnemyActionTargetsSelectedEvent(
                    source.Name,
                    action.DefinitionId,
                    action.Name,
                    targetUnitIds,
                    targetPositions));
        }


        for (int i = 0;
             i < resolvedTargets.Count;
             i++)
        {
            ResolveEnemyActionEffects(
                source,
                action,
                resolvedTargets[i],
                events,
                resolvedTargetPositions[i]);
        }
    }


    private void ResolveEnemyActionEffects(
        Enemy source,
        EnemyAction action,
        RingObject target,
        List<GameEvent> events,
        RingPosition? targetPosition = null)
    {
        EffectContext context =
            new EffectContext
            {
                Enemy =
                    source,

                Ring =
                    Ring,

                RingObject =
                    target,

                TargetPosition =
                    targetPosition,

                Run =
                    _run
            };


        events.AddRange(
            _effectResolver.Resolve(
                source,
                action.Effects,
                context));

        if (!Enemy.IsAlive ||
            _run.IsTeamDefeated)
        {
            return;
        }

        events.AddRange(
            ResolveRelicTrigger(
                EffectTrigger.EnemyActionActivated,
                target,
                targetPosition));
    }


    // =========================================================
    // SCHEDULED ENEMY ACTIONS
    // =========================================================

    private void ResolveScheduledEnemyActions(
        List<GameEvent> events)
    {
        for (int i =
                 _scheduledEnemyActions.Count - 1;
             i >= 0;
             i--)
        {
            ScheduledEnemyAction scheduledAction =
                _scheduledEnemyActions[i];


            bool isReady =
                scheduledAction.AdvanceCountdown();


            if (!isReady)
            {
                events.Add(
                    new EnemyActionCountdownEvent(
                        scheduledAction.Source.Name,
                        scheduledAction.Action.DefinitionId,
                        scheduledAction.Action.Name,
                        scheduledAction.Action.Description,
                        scheduledAction.TurnsRemaining,
                        scheduledAction.Id));

                continue;
            }


            _scheduledEnemyActions.RemoveAt(
                i);


            ResolveEnemyAction(
                scheduledAction.Source,
                scheduledAction.Action,
                events,
                scheduledAction.Id);
        }
    }


    // =========================================================
    // UNIT HELPERS
    // =========================================================

    private List<Unit> GetLivingUnits()
    {
        List<Unit> livingUnits =
            new List<Unit>();


        foreach (RingPosition position
                 in ResolutionOrder)
        {
            if (!Ring.TryGetObject(
                    position,
                    out RingObject ringObject))
            {
                continue;
            }


            if (ringObject is Unit unit &&
                unit.IsAlive)
            {
                livingUnits.Add(
                    unit);
            }
        }


        return livingUnits;
    }


    private Unit GetRandomLivingUnit()
    {
        List<Unit> livingUnits =
            GetLivingUnits();


        if (livingUnits.Count == 0)
        {
            return null;
        }


        return livingUnits[
            _run.random.Next(
                livingUnits.Count)];
    }
}
