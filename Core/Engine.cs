
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public sealed class Engine
    {
        // =========================================================
        // DEPENDENCIES
        // =========================================================

        private readonly FloorGenerator _floorGenerator = new FloorGenerator();

        private readonly List<Unit> _unitChoices = new List<Unit>();
        private readonly ContentRegistry _contentRegistry;


        private readonly Random _random;


        // =========================================================
        // STATE
        // =========================================================

        private Battle _battle;
        private DataDrivenEventEncounter _eventEncounter;

        public Run Run { get; private set; }

        public bool HasRun => Run != null;

        public bool HasPendingUnitChoice => _unitChoices.Count > 0;

        public bool InBattle => _battle != null;
        public bool InEventEncounter => _eventEncounter != null;

        private ShopEncounter _shopEncounter;
        public bool InShop => _shopEncounter != null;


        public Engine()
            : this(
                new Random(),
                new ContentRegistry())
        {
        }

        public Engine(int seed)
            : this(
                new Random(seed),
                new ContentRegistry())
        {
        }

        public Engine(
            int seed,
            string contentDirectory)
            : this(
                new Random(seed),
                new ContentRegistry(
                    contentDirectory))
        {
        }

        private Engine(
            Random random,
            ContentRegistry contentRegistry)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            if (contentRegistry == null)
            {
                throw new ArgumentNullException(
                    nameof(contentRegistry));
            }

            _random = random;
            _contentRegistry = contentRegistry;
        }

        // =========================================================
        // EVENT QUEUE
        // =========================================================

        private readonly Queue<GameEvent> _eventQueue = new Queue<GameEvent>();
        public int PendingEventCount => _eventQueue.Count;

        public bool TryGetNextEvent(out GameEvent gameEvent)
        {
            if (_eventQueue.Count == 0)
            {
                gameEvent = null;
                return false;
            }

            gameEvent = _eventQueue.Dequeue();
            return true;
        }

        private void EnqueueEvent(GameEvent gameEvent)
        {
            if (gameEvent == null)
                throw new ArgumentNullException(nameof(gameEvent));

            _eventQueue.Enqueue(gameEvent);
        }



        // =========================================================
        // RUN COMMANDS
        // =========================================================

        private void AdvanceRun()
        {
            Floor completedFloor =
                null;

            Run.TryGetCurrentFloor(
                out completedFloor);

            RunProgression progression =
                Run.AdvanceFloor();

            if (DidCompleteStage(
                    completedFloor,
                    progression))
            {
                EnqueueEvent(
                    new StageCompletedEvent(
                        completedFloor.StageIndex,
                        new RunInfo(Run)));
            }

            if (progression ==
                RunProgression.RunCompleted)
            {
                EnqueueEvent(
                    new RunCompletedEvent(
                        new RunInfo(Run)));
            }
        }

        private bool DidCompleteStage(
            Floor completedFloor,
            RunProgression progression)
        {
            if (completedFloor == null ||
                completedFloor.StageFloorIndex < 0)
            {
                return false;
            }

            if (progression ==
                RunProgression.RunCompleted)
            {
                return true;
            }

            if (!Run.TryGetCurrentFloor(
                    out Floor nextFloor))
            {
                return false;
            }

            return
                nextFloor.StageIndex > completedFloor.StageIndex;
        }

        public EngineResult StartNewRun()
        {
            if (Run != null)
                return new RunAlreadyActiveResult();

            RunPlanDefinition runPlan =
                _contentRegistry.GetDefaultRunPlan();

            Run =
                new Run(
                    _random,
                    runPlan.Id,
                    runPlan.Name,
                    runPlan.Stages?.Count ?? 0);

            EnqueueEvent(
                new RunStartedEvent(
                    new RunInfo(Run)));

            GenerateUnitChoices(
                runPlan.StartingUnitSelection);

            List<UnitInfo> unitChoiceInfos =
                _unitChoices
                    .Select(
                        unit =>
                            new UnitInfo(unit))
                    .ToList();

            EnqueueEvent(
                new UnitChoicesGeneratedEvent(
                    unitChoiceInfos));

            return new RunStartedResult();
        }



        public EngineResult UseItem(
            Guid itemId,
            ItemUseContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (Run == null)
                return new NoActiveRunResult();

            if (_battle != null)
            {
                return new ItemUseFailedResult(
                    "Items cannot be used during battle.");
            }

            if (!Run.TryGetItem(
                    itemId,
                    out Item item))
            {
                return new ItemUseFailedResult(
                    "Item was not found in the inventory.");
            }

            switch (item)
            {
                case AbilityRecipe abilityRecipe:
                    {
                        if (!context.TargetUnitId.HasValue)
                        {
                            return new ItemUseFailedResult(
                                "Ability Recipe requires a target Unit.");
                        }

                        if (!Run.TryGetTeamUnit(
                                context.TargetUnitId.Value,
                                out Unit targetUnit))
                        {
                            return new ItemUseFailedResult(
                                "Target Unit was not found.");
                        }

                        if (targetUnit.HasActiveAbility)
                        {
                            return new ItemUseFailedResult(
                                "Target Unit already has an Active Ability.");
                        }

                        if (!targetUnit.TrySetAbility(
                                abilityRecipe.Ability))
                        {
                            return new ItemUseFailedResult(
                                "Ability could not be learned by the target Unit.");
                        }

                        if (!Run.TryRemoveItem(item))
                        {
                            throw new InvalidOperationException(
                                "Used Item could not be removed from the inventory.");
                        }

                        _eventQueue.Enqueue(
                            new ItemUsedEvent(
                                item,
                                targetUnit));

                        return new ItemUsedResult(
                            item.Name);
                    }

                case PassiveRecipe passiveRecipe:
                    {
                        if (!context.TargetUnitId.HasValue)
                        {
                            return new ItemUseFailedResult(
                                "Passive Recipe requires a target Unit.");
                        }

                        if (!Run.TryGetTeamUnit(
                                context.TargetUnitId.Value,
                                out Unit targetUnit))
                        {
                            return new ItemUseFailedResult(
                                "Target Unit was not found.");
                        }

                        targetUnit.AddRunPassive(
                            passiveRecipe.Passive);

                        if (!Run.TryRemoveItem(item))
                        {
                            throw new InvalidOperationException(
                                "Used Item could not be removed from the inventory.");
                        }

                        EnqueueEvent(
                            new ItemUsedEvent(
                                item,
                                targetUnit));

                        return new ItemUsedResult(
                            item.Name);
                    }

                case EffectRecipe effectRecipe:
                    {
                        if (!context.TargetUnitId.HasValue)
                        {
                            return new ItemUseFailedResult(
                                "Effect Recipe requires a target Unit.");
                        }

                        if (!Run.TryGetTeamUnit(
                                context.TargetUnitId.Value,
                                out Unit targetUnit))
                        {
                            return new ItemUseFailedResult(
                                "Target Unit was not found.");
                        }

                        if (!targetUnit.TryAddAction(
                                effectRecipe.Action,
                                consumesEffectSlot: true))
                        {
                            return new ItemUseFailedResult(
                                "Target Unit has no available Effect slots.");
                        }

                        if (!Run.TryRemoveItem(item))
                        {
                            throw new InvalidOperationException(
                                "Used Item could not be removed from the inventory.");
                        }

                        EnqueueEvent(
                            new ItemUsedEvent(
                                item,
                                targetUnit));

                        return new ItemUsedResult(
                            item.Name);
                    }

                default:
                    return new ItemUseFailedResult(
                        "This Item type cannot currently be used.");
            }
        }

        public EngineResult SelectUnitChoice(Guid unitId)
        {
            if (!HasPendingUnitChoice)
                return new NoPendingUnitChoiceResult();

            Unit unit = _unitChoices
                .FirstOrDefault(unit => unit.Id == unitId);

            if (unit == null)
                return new UnitChoiceNotFoundResult();

            EngineResult result = AddUnitToTeam(unit);

            if (!result.Success)
                return result;

            _unitChoices.Clear();

            return new UnitChoiceSelectedResult(unit);
        }

        public bool CanUseUnitAbility(
            Guid unitId,
            AbilityContext abilityContext)
        {
            if (abilityContext == null)
                throw new ArgumentNullException(nameof(abilityContext));

            if (_battle == null)
                return false;

            return _battle.CanUseUnitAbility(
                unitId,
                abilityContext);
        }


        public EngineResult UseUnitAbility(
            Guid unitId,
            AbilityContext abilityContext)
        {
            if (abilityContext == null)
                throw new ArgumentNullException(nameof(abilityContext));

            if (_battle == null)
                return new NoActiveBattleResult();

            if (!_battle.TryUseUnitAbility(
                    unitId,
                    abilityContext,
                    out Guid abilityId,
                    out List<GameEvent> events))
            {
                return new AbilityUseFailedResult();
            }

            foreach (GameEvent gameEvent in events)
            {
                EnqueueEvent(
                    gameEvent);
            }

            FinalizeBattleIfResolved();

            return new AbilityUsedResult(
                abilityId);
        }


        public EngineResult UseAbility(
            Guid abilityId,
                AbilityContext abilityContext)
        {

            if (abilityContext == null)
                throw new ArgumentNullException(nameof(abilityContext));

            if (_battle == null)
                return new NoActiveBattleResult();

            if (!_battle.TryUseAbility(
                abilityId,
                abilityContext,
                out List<GameEvent> events))
            {
                return new AbilityUseFailedResult();
            }

            foreach (GameEvent gameEvent in events)
            {
                EnqueueEvent(gameEvent);
            }

            FinalizeBattleIfResolved();

            return new AbilityUsedResult(
                abilityId);
        }

        public EngineResult ChooseEventOption(Guid optionId)
        {
            if (_eventEncounter == null)
                return new NoActiveEventEncounterResult();

            if (!_eventEncounter.TryGetOption(
                    optionId,
                    out EventOption option))
            {
                return new EventOptionNotFoundResult();
            }

            if (!_eventEncounter.CanChooseOption(
                    Run,
                    option,
                    out string unavailableReason))
            {
                return new EventOptionUnavailableResult(
                    unavailableReason);
            }

            List<GameEvent> events =
                _eventEncounter.ChooseOption(
                    Run,
                    option,
                    _contentRegistry);

            foreach (GameEvent gameEvent in events)
            {
                EnqueueEvent(gameEvent);
            }

            string optionName = option.Name;

            FinalizeEventStep();

            return new EventOptionChosenResult(
                optionName);
        }

        public EngineResult ContinueEventPresentation()
        {
            if (_eventEncounter == null)
                return new NoActiveEventEncounterResult();


            if (_eventEncounter.CurrentNodeType !=
                EventNodeType.Presentation)
            {
                return new NoActiveEventPresentationResult();
            }


            string nodeId =
                _eventEncounter.CurrentNodeId;


            List<GameEvent> events =
                _eventEncounter.ContinuePresentation(
                    Run,
                    _contentRegistry);


            foreach (GameEvent gameEvent
                     in events)
            {
                EnqueueEvent(
                    gameEvent);
            }


            FinalizeEventStep();


            return new EventPresentationContinuedResult(
                nodeId);
        }

        public EngineResult CompleteEventActivity(
            string resultId)
        {
            if (_eventEncounter == null)
                return new NoActiveEventEncounterResult();

            if (_eventEncounter.CurrentNodeType !=
                EventNodeType.Activity)
            {
                return new NoActiveEventActivityResult();
            }

            if (!_eventEncounter.TryGetActivityResult(
                    resultId,
                    out EventActivityResultDefinition activityResult))
            {
                return new EventActivityResultNotFoundResult(
                    resultId);
            }

            string activityId =
                _eventEncounter.ActivityId;

            List<GameEvent> events =
                _eventEncounter.CompleteActivity(
                    Run,
                    activityResult,
                    _contentRegistry);

            foreach (GameEvent gameEvent in events)
            {
                EnqueueEvent(gameEvent);
            }

            FinalizeEventStep();

            return new EventActivityCompletedResult(
                activityId,
                activityResult.ResultId);
        }

        public EngineResult BuyShopOffer(Guid offerId)
        {
            if (_shopEncounter == null)
                return new NoActiveShopEncounterResult();

            if (!_shopEncounter.TryGetOffer(
                offerId,
                out ShopOffer offer))
            {
                return new ShopOfferNotFoundResult();
            }

            if (!Run.TryRemoveCurrency(offer.Price))
                return new NotEnoughCurrencyResult();

            List<GameEvent> events =
                offer.Purchase(Run);

            _shopEncounter.RemoveOffer(offer);

            foreach (GameEvent gameEvent in events)
            {
                EnqueueEvent(gameEvent);
            }

            return new ShopOfferPurchasedResult(
                offer.Name);
        }

        public EngineResult LeaveShop()
        {
            if (_shopEncounter == null)
                return new NoActiveShopEncounterResult();

            string shopName =
                _shopEncounter.Name;

            _shopEncounter = null;

            AdvanceRun();

            return new ShopEncounterLeftResult(
                shopName);
        }

        public EngineResult AddUnitToTeam(Unit unit)
        {
            if (unit == null)
                throw new ArgumentNullException(unit.ToString());

            if (Run == null)
                return new NoActiveRunResult();

            if (!Run.TryAddUnit(unit))
                return new TeamFullResult();

            EnqueueEvent(
                new UnitAddedEvent(
                    new UnitInfo(unit)));

            return new UnitAddedResult(unit);
        }

        public EngineResult AddBlessing(
            string blessingId)
        {
            if (string.IsNullOrWhiteSpace(
                    blessingId))
            {
                throw new ArgumentException(
                    "Blessing Id cannot be empty.",
                    nameof(blessingId));
            }

            return AddBlessing(
                _contentRegistry.CreateBlessing(
                    blessingId));
        }


        public EngineResult AddBlessing(
            IBlessing blessing)
        {
            if (blessing == null)
                throw new ArgumentNullException(nameof(blessing));

            if (Run == null)
                return new NoActiveRunResult();

            Run.AddBlessing(blessing);

            EnqueueEvent(
                new BlessingAddedEvent(
                    new BlessingInfo(blessing)));

            return new BlessingAddedResult(blessing);
        }

        public EngineResult AddRelic(
            string relicId)
        {
            if (string.IsNullOrWhiteSpace(
                    relicId))
            {
                throw new ArgumentException(
                    "Relic Id cannot be empty.",
                    nameof(relicId));
            }

            if (Run == null)
                return new NoActiveRunResult();

            Relic relic =
                _contentRegistry.CreateRelic(
                    relicId);

            Run.AddRelic(
                relic);

            EnqueueEvent(
                new RelicAddedEvent(
                    new RelicInfo(
                        relic)));

            return new RelicAddedResult(
                relic);
        }


        public EngineResult BeginClimb()
        {
            if (Run == null)
                return new NoActiveRunResult();

            if (HasPendingUnitChoice)
                return new UnitChoiceRequiredResult();

            if (_battle != null)
                return new BattleAlreadyActiveResult();

            if (!Run.CanBeginClimb())
                return new ClimbRequirementsNotMetResult();

            RunPlanDefinition runPlan =
                _contentRegistry.GetRunPlan(
                    Run.RunPlanId);

            List<Floor> floors =
                _floorGenerator.GenerateFloors(
                    runPlan);

            if (floors == null ||
                floors.Count == 0)
            {
                return new FloorGenerationFailedResult();
            }

            Run.BeginClimb(floors);

            EnqueueEvent(
                new ClimbStartedEvent(
                    new RunInfo(Run)));

            return new ClimbStartedResult();
        }


        // =========================================================
        // FLOOR COMMANDS
        // =========================================================

        public EngineResult ResolveCurrentFloor()
        {
            if (Run == null)
                return new NoActiveRunResult();



            if (Run.State != RunState.Climbing)
                return new RunNotClimbingResult();

            if (_battle != null)
                return new BattleAlreadyActiveResult();

            if (_eventEncounter != null)
                return new EventEncounterAlreadyActiveResult();

            if (_shopEncounter != null)
                return new ShopEncounterAlreadyActiveResult();

            if (!Run.TryGetCurrentFloor(out Floor floor))
                return new CurrentFloorNotFoundResult();

            return ResolveFloor(floor);
        }


        // =========================================================
        // BATTLE COMMANDS
        // =========================================================

        public EngineResult Rotate(RotationDirection direction)
        {
            if (_battle == null)
                return new NoActiveBattleResult();

            if (!_battle.TryRotate(direction))
                return new RotationFailedResult();

            EnqueueEvent(
                new RingRotatedEvent(
                    new RingInfo(_battle.Ring)));

            foreach (GameEvent gameEvent
                     in _battle.ResolveRelicTrigger(
                         EffectTrigger.RingRotated))
            {
                EnqueueEvent(
                    gameEvent);
            }

            if (_battle.IsWon)
            {
                EnqueueEvent(
                    new BattleWonEvent(
                        new BattleInfo(
                            _battle)));
            }

            FinalizeBattleIfResolved();

            return new RingRotatedResult(direction);
        }

        public EngineResult Reposition(
        RingPosition firstPosition,
            RingPosition secondPosition)
        {
            if (_battle == null)
                return new NoActiveBattleResult();

            if (!_battle.TryReposition(
                firstPosition,
                secondPosition))
            {
                return new RepositionFailedResult();
            }

            EnqueueEvent(
                new RingRepositionedEvent(
                    firstPosition,
                    secondPosition,
                    new RingInfo(_battle.Ring)));

            return new RingRepositionedResult(
                firstPosition,
                secondPosition);
        }


        // =========================================================
        // QUERIES
        // =========================================================

        public bool TryGetCurrentFloorIndex(out int floorIndex)
        {
            floorIndex = 0;

            if (Run == null)
                return false;

            if (Run.State != RunState.Climbing)
                return false;

            floorIndex =
                Run.CurrentFloorIndex;

            return true;
        }

        public bool TryGetCurrentStageIndex(out int stageIndex)
        {
            stageIndex = 0;

            if (Run == null)
                return false;

            if (Run.State != RunState.Climbing)
                return false;

            stageIndex =
                Run.CurrentStageIndex;

            return stageIndex >= 0;
        }

        public ShopEncounterInfo GetShopEncounterInfo()
        {
            if (_shopEncounter == null)
                return null;

            return new ShopEncounterInfo(_shopEncounter);
        }

        public RunInfo GetRunInfo()
        {
            if (Run == null)
                return null;

            return new RunInfo(Run);
        }

        public EventEncounterInfo GetEventEncounterInfo()
        {
            if (_eventEncounter == null)
                return null;

            return new EventEncounterInfo(
                _eventEncounter,
                Run);
        }

        public BattleInfo GetBattleInfo()
        {
            if (_battle == null)
                return null;

            return new BattleInfo(_battle);
        }

        public FloorGenerationInfo GetFloorGenerationInfo()
        {
            if (Run == null)
                return null;

            return new FloorGenerationInfo(Run);
        }


        // =========================================================
        // FLOOR INTERNALS
        // =========================================================

        // DISPATCHER
        private EngineResult ResolveFloor(Floor floor)
        {
            return floor.Type switch
            {
                FloorType.Battle => ResolveBattleFloor(floor),
                FloorType.Event => ResolveEventFloor(floor),
                FloorType.Shop => ResolveShopFloor(floor),
                FloorType.Boss => ResolveBossFloor(floor),

                _ => new UnsupportedFloorTypeResult(floor.Type)
            };
        }

        private EngineResult ResolveBattleFloor(
            Floor floor)
        {
            RunStageDefinition stage =
                GetStageDefinition(
                    floor);

            return StartBattle(
                _contentRegistry.CreateEnemyFromPool(
                    stage.EnemyPoolId,
                    EnemyType.Normal,
                    _random));
        }

        private EngineResult ResolveEventFloor(
            Floor floor)
        {
            RunStageDefinition stage =
                GetStageDefinition(floor);

            _eventEncounter =
                _contentRegistry.CreateEventFromPool(
                    stage.EventPoolId,
                    _random);

            string eventName =
                _eventEncounter.Name;

            // Every Event is announced before its first Node resolves.
            // This also gives presentation consumers a chance to show
            // Automatic-only Events such as "Found Gold".
            bool startedOnAutomaticNode =
                _eventEncounter.CurrentNodeType == EventNodeType.Automatic;

            EnqueueEvent(
                new EventEncounterStartedEvent(
                    new EventEncounterInfo(
                        _eventEncounter,
                        Run)));

            List<GameEvent> events =
                _eventEncounter.ResolveAutomaticNodes(
                    Run,
                    _contentRegistry);

            foreach (GameEvent gameEvent in events)
            {
                EnqueueEvent(gameEvent);
            }

            if (Run.IsTeamDefeated)
            {
                FailRunFromEvent();

                return new EventEncounterResolvedResult(
                    eventName);
            }

            if (_eventEncounter.IsComplete)
            {
                CompleteCurrentEvent();

                return new EventEncounterResolvedResult(
                    eventName);
            }

            // If the Event entered through Automatic Nodes and they moved
            // into a Choice or Activity, publish that new interactive state.
            if (startedOnAutomaticNode)
            {
                EnqueueEvent(
                    new EventEncounterUpdatedEvent(
                        new EventEncounterInfo(
                            _eventEncounter,
                            Run)));
            }

            return new EventEncounterStartedResult(
                eventName);
        }

        private void FinalizeEventStep()
        {
            if (_eventEncounter == null)
                return;

            if (Run.IsTeamDefeated)
            {
                FailRunFromEvent();
                return;
            }

            if (_eventEncounter.IsComplete)
            {
                CompleteCurrentEvent();
                return;
            }

            EnqueueEvent(
                new EventEncounterUpdatedEvent(
                    new EventEncounterInfo(
                        _eventEncounter,
                        Run)));
        }

        private void CompleteCurrentEvent()
        {
            DataDrivenEventEncounter encounter =
                _eventEncounter;

            if (encounter == null)
                return;

            _eventEncounter = null;

            EnqueueEvent(
                new EventEncounterCompletedEvent(
                    encounter.Id,
                    encounter.DefinitionId,
                    encounter.Name));

            AdvanceRun();
        }

        private void FailRunFromEvent()
        {
            DataDrivenEventEncounter encounter =
                _eventEncounter;

            _eventEncounter = null;

            if (encounter != null)
            {
                EnqueueEvent(
                    new EventEncounterCompletedEvent(
                        encounter.Id,
                        encounter.DefinitionId,
                        encounter.Name));
            }

            if (Run.State == RunState.Climbing)
            {
                Run.Fail();

                EnqueueEvent(
                    new RunFailedEvent(
                        new RunInfo(Run)));
            }
        }

        private EngineResult ResolveShopFloor(
            Floor floor)
        {
            RunStageDefinition stage =
                GetStageDefinition(
                    floor);

            _shopEncounter =
                _contentRegistry.CreateVendorFromPool(
                    stage.VendorPoolId,
                    _random);

            EnqueueEvent(
                new ShopEncounterStartedEvent(
                    new ShopEncounterInfo(
                        _shopEncounter)));

            return new ShopEncounterStartedResult(
                _shopEncounter.Name);
        }

        private EngineResult ResolveBossFloor(
            Floor floor)
        {
            RunStageDefinition stage =
                GetStageDefinition(
                    floor);

            return StartBattle(
                _contentRegistry.CreateEnemyFromPool(
                    stage.BossPoolId,
                    EnemyType.Boss,
                    _random));
        }


        private RunStageDefinition GetStageDefinition(
            Floor floor)
        {
            if (floor == null)
                throw new ArgumentNullException(nameof(floor));

            RunPlanDefinition runPlan =
                _contentRegistry.GetRunPlan(
                    Run.RunPlanId);

            if (runPlan.Stages == null ||
                floor.StageIndex < 0 ||
                floor.StageIndex >= runPlan.Stages.Count)
            {
                throw new InvalidOperationException(
                    $"Stage definition not found for Stage {floor.StageIndex + 1}.");
            }

            return runPlan.Stages[floor.StageIndex];
        }

        // =========================================================
        // BATTLE INTERNALS
        // =========================================================

        private void FinalizeBattleIfResolved()
        {
            if (_battle == null)
                return;

            if (_battle.IsWon)
            {
                Reward reward =
                    _battle.Enemy.RollReward(
                        _random);

                RewardInfo rewardInfo =
                    reward.CreateInfo();

                List<GameEvent> rewardEvents =
                    reward.Apply(Run);

                // Some rewards can be unavailable in the current run state.
                // Example: a Unit reward while all ring slots are occupied.
                // In that case Apply returns no events, so the reward is skipped
                // instead of being reported as granted.
                if (rewardEvents.Count > 0)
                {
                    EnqueueEvent(
                        new BattleRewardGrantedEvent(
                            rewardInfo));

                    foreach (GameEvent gameEvent in rewardEvents)
                    {
                        EnqueueEvent(gameEvent);
                    }
                }

                _battle = null;

                AdvanceRun();

                return;
            }

            if (!Run.IsTeamDefeated)
                return;

            BattleInfo battleInfo =
                new BattleInfo(_battle);

            _battle = null;

            EnqueueEvent(
                new BattleLostEvent(
                    battleInfo));

            Run.Fail();

            EnqueueEvent(
                new RunFailedEvent(
                    new RunInfo(Run)));
        }

        private EngineResult StartBattle(Enemy enemy)
        {
            if (enemy == null)
                throw new ArgumentNullException(enemy.ToString());

            if (Run == null)
                return new NoActiveRunResult();

            if (_battle != null)
                return new BattleAlreadyActiveResult();

            if (Run.State != RunState.Climbing)
                return new RunNotClimbingResult();

            _battle = new Battle(
                Run,
                enemy);

            EnqueueEvent(
                new BattleStartedEvent(
                    new BattleInfo(_battle)));

            return new BattleStartedResult(enemy);
        }


        private void GenerateUnitChoices(
            PoolSelectionDefinition selection)
        {
            if (selection == null)
            {
                throw new InvalidOperationException(
                    "Run plan has no StartingUnitSelection.");
            }

            if (selection.ChoiceCount <= 0)
            {
                throw new InvalidOperationException(
                    "Starting Unit choice count must be greater than zero.");
            }

            PoolDefinition pool =
                _contentRegistry.GetPool(
                    selection.PoolId);

            List<PoolEntryDefinition> entries =
                WeightedPoolSelector.SelectDistinct(
                    pool,
                    PoolEntryType.Unit,
                    selection.ChoiceCount,
                    _random);

            if (entries.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Starting Unit pool '{selection.PoolId}' contains no usable Units.");
            }

            _unitChoices.Clear();

            foreach (PoolEntryDefinition entry in entries)
            {
                _unitChoices.Add(
                    _contentRegistry.CreateUnit(
                        entry.ContentId));
            }
        }

        public EngineResult ResolveRing()
        {
            if (_battle == null)
                return new NoActiveBattleResult();

            List<GameEvent> events =
                _battle.ResolveRing();

            foreach (GameEvent gameEvent in events)
            {
                EnqueueEvent(gameEvent);
            }

            FinalizeBattleIfResolved();

            return new RingResolvedResult();
        }

    }
}