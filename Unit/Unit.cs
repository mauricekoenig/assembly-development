using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class Unit : RingObject, ICombatant, IEffectSource, IHasPassives
    {
        private readonly HashSet<UnitTag> _tags =
            new HashSet<UnitTag>();

        private readonly List<UnitAction> _actions =
            new List<UnitAction>();

        private readonly List<Passive> _passives =
            new List<Passive>();

        private ActiveAbility _activeAbility;


        // =========================================================
        // HEALTH
        // =========================================================

        public int BaseHealth { get; }
        public int CurrentHealth { get; set; }

        public bool IsAlive =>
            CurrentHealth > 0;


        // =========================================================
        // UNIT ACTIONS
        // =========================================================

        public IReadOnlyList<UnitAction> Actions =>
            _actions;


        // =========================================================
        // EFFECT SLOTS
        // =========================================================
        // Effect Slots do not contain raw Effects anymore.
        // They are simply the capacity used by UnitActions acquired
        // during a run. Starting Actions are added through the exact
        // same Action collection, but do not consume a slot.

        public int EffectSlotCapacity { get; }

        public int UsedEffectSlots { get; private set; }

        // Compatibility alias for existing consumers. This counts
        // run-added UnitActions that consume Effect Slots; the Unit
        // does not own a raw Effect collection.
        public int AddedEffectCount =>
            UsedEffectSlots;

        public int AvailableEffectSlots =>
            EffectSlotCapacity - UsedEffectSlots;


        // =========================================================
        // ACTIVE ABILITIES
        // =========================================================

        // Every Unit owns exactly one Active Ability slot.
        // The slot may be empty, allowing a Unit to learn an Ability
        // during the run without introducing an Ability-selection layer.
        public int AbilitySlotCapacity =>
            1;

        public bool HasActiveAbility =>
            _activeAbility != null;

        public int AvailableAbilitySlots =>
            HasActiveAbility
                ? 0
                : 1;

        public ActiveAbility ActiveAbility =>
            _activeAbility;

        // Compatibility view for existing consumers. The engine itself
        // treats Active Ability ownership as a single optional slot.
        public IReadOnlyList<ActiveAbility> ActiveAbilities =>
            _activeAbility == null
                ? Array.Empty<ActiveAbility>()
                : new[] { _activeAbility };


        // =========================================================
        // IDENTITY
        // =========================================================

        public string DefinitionId { get; }

        public IReadOnlyCollection<UnitTag> Tags =>
            _tags;

        public IReadOnlyList<Passive> Passives =>
            _passives;

        public Team Team { get; set; } =
            Team.Player;


        // =========================================================
        // CONSTRUCTORS
        // =========================================================

        public Unit(
            string definitionId,
            int baseHealth,
            int effectSlotCapacity)
        {
            if (string.IsNullOrWhiteSpace(
                    definitionId))
            {
                throw new ArgumentException(
                    "Definition Id cannot be empty.",
                    nameof(definitionId));
            }

            ValidateConstructorArguments(
                baseHealth,
                effectSlotCapacity);

            DefinitionId =
                definitionId;

            BaseHealth =
                baseHealth;

            CurrentHealth =
                baseHealth;

            EffectSlotCapacity =
                effectSlotCapacity;
        }


        // Compatibility overload for older consumers. Ability slot
        // capacity is no longer configurable and must be exactly one.
        public Unit(
            string definitionId,
            int baseHealth,
            int effectSlotCapacity,
            int abilitySlotCapacity)
            : this(
                definitionId,
                baseHealth,
                effectSlotCapacity)
        {
            ValidateLegacyAbilitySlotCapacity(
                abilitySlotCapacity);
        }


        // Legacy/test constructor.
        public Unit(
            int baseHealth,
            int effectSlotCapacity)
        {
            ValidateConstructorArguments(
                baseHealth,
                effectSlotCapacity);

            DefinitionId =
                string.Empty;

            BaseHealth =
                baseHealth;

            CurrentHealth =
                baseHealth;

            EffectSlotCapacity =
                effectSlotCapacity;
        }


        // Compatibility overload for older tests/consumers.
        public Unit(
            int baseHealth,
            int effectSlotCapacity,
            int abilitySlotCapacity)
            : this(
                baseHealth,
                effectSlotCapacity)
        {
            ValidateLegacyAbilitySlotCapacity(
                abilitySlotCapacity);
        }


        private static void ValidateConstructorArguments(
            int baseHealth,
            int effectSlotCapacity)
        {
            if (baseHealth <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(baseHealth));
            }

            if (effectSlotCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(effectSlotCapacity));
            }
        }


        private static void ValidateLegacyAbilitySlotCapacity(
            int abilitySlotCapacity)
        {
            if (abilitySlotCapacity != 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(abilitySlotCapacity),
                    "Units always have exactly one Active Ability slot.");
            }
        }

        // =========================================================
        // TAGS
        // =========================================================

        public bool HasTag(UnitTag tag)
        {
            return _tags.Contains(
                tag);
        }

        internal void AddTag(UnitTag tag)
        {
            _tags.Add(
                tag);
        }


        // =========================================================
        // UNIT ACTIONS
        // =========================================================
        // There is only one Action collection. Starting Actions and
        // Actions gained during the run are runtime-equivalent and are
        // resolved by the same Battle pipeline. The boolean only controls
        // whether adding the Action consumes one Effect Slot.

        internal bool TryAddAction(
            UnitAction action,
            bool consumesEffectSlot)
        {
            Guard.NotNull(
                action,
                nameof(action));

            if (consumesEffectSlot &&
                AvailableEffectSlots <= 0)
            {
                return false;
            }

            _actions.Add(
                action);

            if (consumesEffectSlot)
            {
                UsedEffectSlots++;
            }

            return true;
        }


        // =========================================================
        // PASSIVES
        // =========================================================

        internal void AddInnatePassive(
            Passive passive)
        {
            Guard.NotNull(
                passive,
                nameof(passive));

            _passives.Add(
                passive);
        }

        internal void AddRunPassive(
            Passive passive)
        {
            Guard.NotNull(
                passive,
                nameof(passive));

            _passives.Add(
                passive);
        }


        // =========================================================
        // ACTIVE ABILITIES
        // =========================================================

        internal bool TrySetAbility(
            ActiveAbility ability)
        {
            Guard.NotNull(
                ability,
                nameof(ability));

            if (_activeAbility != null)
                return false;

            _activeAbility =
                ability;

            return true;
        }


        // Compatibility alias while consumers migrate to the
        // explicit single-slot terminology.
        internal bool AddAbility(
            ActiveAbility ability)
        {
            return TrySetAbility(
                ability);
        }


        // =========================================================
        // HEALTH
        // =========================================================

        public void Heal(int amount)
        {
            if (amount <= 0)
                return;

            CurrentHealth =
                Math.Min(
                    BaseHealth,
                    CurrentHealth + amount);
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0)
                return;

            CurrentHealth =
                Math.Max(
                    0,
                    CurrentHealth - amount);
        }
    }
}
