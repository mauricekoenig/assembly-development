using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public sealed class UnitInfo : RingObjectInfo
    {
        public string DefinitionId { get; }

        public int BaseHealth { get; }
        public int CurrentHealth { get; }

        public bool IsAlive { get; }
        public Team Team { get; }

        public IReadOnlyList<UnitTag> Tags { get; }
        public IReadOnlyList<UnitActionInfo> Actions { get; }
        public IReadOnlyList<PassiveInfo> Passives { get; }

        public int EffectSlotCapacity { get; }
        public int UsedEffectSlots { get; }

        // Compatibility alias for existing Unity/UI consumers.
        public int AddedEffectCount { get; }

        public int AvailableEffectSlots { get; }

        public int AbilitySlotCapacity { get; }
        public int AvailableAbilitySlots { get; }

        public bool HasActiveAbility { get; }
        public ActiveAbilityInfo ActiveAbility { get; }

        // Compatibility view for existing Unity/UI code.
        public IReadOnlyList<ActiveAbilityInfo> ActiveAbilities { get; }

        internal UnitInfo(Unit unit)
            : base(unit)
        {
            Guard.NotNull(
                unit,
                nameof(unit));

            DefinitionId =
                unit.DefinitionId;

            BaseHealth =
                unit.BaseHealth;

            CurrentHealth =
                unit.CurrentHealth;

            IsAlive =
                unit.IsAlive;

            Team =
                unit.Team;

            Tags =
                unit.Tags.ToList();

            Actions =
                unit.Actions
                    .Select(action =>
                        new UnitActionInfo(
                            action))
                    .ToList();

            Passives =
                unit.Passives
                    .Select(passive =>
                        new PassiveInfo(
                            passive))
                    .ToList();

            EffectSlotCapacity =
                unit.EffectSlotCapacity;

            UsedEffectSlots =
                unit.UsedEffectSlots;

            AddedEffectCount =
                unit.AddedEffectCount;

            AvailableEffectSlots =
                unit.AvailableEffectSlots;

            AbilitySlotCapacity =
                unit.AbilitySlotCapacity;

            AvailableAbilitySlots =
                unit.AvailableAbilitySlots;

            HasActiveAbility =
                unit.HasActiveAbility;

            ActiveAbility =
                unit.ActiveAbility != null
                    ? new ActiveAbilityInfo(
                        unit.ActiveAbility)
                    : null;

            ActiveAbilities =
                ActiveAbility == null
                    ? new List<ActiveAbilityInfo>()
                    : new List<ActiveAbilityInfo>
                    {
                        ActiveAbility
                    };
        }

        public override string ToString()
        {
            return $"{Name} ({CurrentHealth}/{BaseHealth})";
        }
    }
}
