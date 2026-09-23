using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class Run
    {
        // =========================================================
        // IDENTITY
        // =========================================================

        public Guid Id { get; } =
            Guid.NewGuid();


        // =========================================================
        // CORE RUN STATE
        // =========================================================

        public Ring Ring { get; }

        internal Random random;

        public RunState State { get; private set; } =
            RunState.Setup;


        // =========================================================
        // PROGRESSION
        // =========================================================

        internal int CurrentFloorIndex
        {
            get;
            private set;
        }

        private readonly List<Floor> _floors =
            new List<Floor>();

        internal IReadOnlyList<Floor> Floors =>
            _floors;


        // =========================================================
        // RESOURCES
        // =========================================================

        public int Currency { get; private set; }


        // =========================================================
        // ITEMS
        // =========================================================

        private readonly List<Item> _items =
            new List<Item>();

        internal IReadOnlyList<Item> Items =>
            _items;

        public int ItemCount =>
            _items.Count;


        // =========================================================
        // RELICS
        // =========================================================

        private readonly List<Relic> _relics =
            new List<Relic>();

        internal IReadOnlyList<Relic> Relics =>
            _relics;

        public int RelicCount =>
            _relics.Count;


        // =========================================================
        // TEAM
        // =========================================================

        public List<Unit> Team { get; } =
            new List<Unit>();

        internal bool IsTeamDefeated
        {
            get
            {
                foreach (Unit unit in Team)
                {
                    if (unit.IsAlive)
                        return false;
                }

                return true;
            }
        }


        // =========================================================
        // BLESSINGS / MODIFIERS
        // =========================================================

        private readonly Stack<IBlessing>
            _blessingHistory =
                new Stack<IBlessing>();

        public int BlessingCount =>
            _blessingHistory.Count;

        internal SectorModifiers SectorModifiers
        {
            get;
        } = new SectorModifiers();


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public string RunPlanId { get; }

        public string RunPlanName { get; }

        public int StageCount { get; }

        internal int CurrentStageIndex
        {
            get
            {
                if (CurrentFloorIndex < 0 ||
                    CurrentFloorIndex >= _floors.Count)
                {
                    return -1;
                }

                return _floors[CurrentFloorIndex].StageIndex;
            }
        }

        internal int CurrentStageNumber =>
            CurrentStageIndex < 0
                ? 0
                : CurrentStageIndex + 1;

        public Run(Random random)
            : this(
                random,
                string.Empty,
                string.Empty,
                0)
        {
        }

        internal Run(
            Random random,
            string runPlanId,
            string runPlanName,
            int stageCount)
        {
            this.random =
                random ??
                throw new ArgumentNullException(
                    nameof(random));

            RunPlanId = runPlanId ?? string.Empty;
            RunPlanName = runPlanName ?? string.Empty;
            StageCount = Math.Max(0, stageCount);

            Ring =
                new Ring();

            Currency =
                100;
        }


        // =========================================================
        // PROGRESSION
        // =========================================================

        internal RunProgression AdvanceFloor()
        {
            if (State != RunState.Climbing)
            {
                throw new InvalidOperationException(
                    "Cannot advance while the run is not climbing.");
            }

            if (CurrentFloorIndex + 1 <
                _floors.Count)
            {
                CurrentFloorIndex++;

                return
                    RunProgression.FloorAdvanced;
            }

            State =
                RunState.Finished;

            return
                RunProgression.RunCompleted;
        }

        internal void BeginClimb(
            List<Floor> generatedFloors)
        {
            Guard.NotNull(
                generatedFloors,
                nameof(generatedFloors));

            _floors.Clear();

            _floors.AddRange(
                generatedFloors);

            CurrentFloorIndex =
                0;

            State =
                RunState.Climbing;
        }

        internal bool TryGetCurrentFloor(
            out Floor floor)
        {
            floor =
                null;

            if (CurrentFloorIndex < 0 ||
                CurrentFloorIndex >=
                _floors.Count)
            {
                return false;
            }

            floor =
                _floors[CurrentFloorIndex];

            return true;
        }


        // =========================================================
        // RUN STATE
        // =========================================================

        internal void Fail()
        {
            if (State != RunState.Climbing)
            {
                throw new InvalidOperationException(
                    "Cannot fail a run that is not climbing.");
            }

            State =
                RunState.Failed;
        }


        // =========================================================
        // ITEMS
        // =========================================================

        internal void AddItem(
            Item item)
        {
            Guard.NotNull(
                item,
                nameof(item));

            _items.Add(item);
        }

        internal bool TryRemoveItem(
            Item item)
        {
            Guard.NotNull(
                item,
                nameof(item));

            return
                _items.Remove(item);
        }

        internal bool TryGetItem(
            Guid itemId,
            out Item item)
        {
            foreach (Item currentItem in _items)
            {
                if (currentItem.Id != itemId)
                    continue;

                item =
                    currentItem;

                return true;
            }

            item =
                null;

            return false;
        }


        // =========================================================
        // RELICS
        // =========================================================

        internal void AddRelic(
            Relic relic)
        {
            Guard.NotNull(
                relic,
                nameof(relic));

            _relics.Add(relic);
        }


        // =========================================================
        // TEAM
        // =========================================================

        internal bool TryGetTeamUnit(
            Guid unitId,
            out Unit unit)
        {
            foreach (Unit currentUnit in Team)
            {
                if (currentUnit.Id != unitId)
                    continue;

                unit =
                    currentUnit;

                return true;
            }

            unit =
                null;

            return false;
        }

        internal bool TryAddUnit(
            Unit unit)
        {
            Guard.NotNull(
                unit,
                nameof(unit));

            if (!Ring.TryAddObject(unit))
                return false;

            Team.Add(unit);

            return true;
        }


        // =========================================================
        // CURRENCY
        // =========================================================

        internal void AddCurrency(
            int amount)
        {
            Currency +=
                amount;
        }

        internal bool TryRemoveCurrency(
            int amount)
        {
            if (amount < 0)
                return false;

            if (Currency < amount)
                return false;

            Currency -=
                amount;

            return true;
        }


        // =========================================================
        // BLESSINGS
        // =========================================================

        internal void AddBlessing(
            IBlessing blessing)
        {
            if (blessing == null)
                return;

            blessing.Apply(this);

            _blessingHistory.Push(
                blessing);
        }


        // =========================================================
        // CLIMB REQUIREMENTS
        // =========================================================

        internal bool CanBeginClimb()
        {
            if (State != RunState.Setup)
                return false;

            if (Team.Count < 1)
                return false;

            return true;
        }
    }
}