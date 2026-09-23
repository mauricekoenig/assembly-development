using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class ActiveAbility :
        IGameEntity
    {
        private readonly List<ActionEffect> _effects =
            new List<ActionEffect>();

        private readonly HashSet<AbilitySelectionRuleType>
            _selectionRules =
                new HashSet<AbilitySelectionRuleType>();

        public Guid Id { get; } =
            Guid.NewGuid();

        public string Name { get; }

        public string Description { get; }

        public AbilitySelectionType SelectionType { get; }

        public IReadOnlyCollection<AbilitySelectionRuleType>
            SelectionRules =>
                _selectionRules;

        public int CooldownRounds { get; }

        public int CurrentCooldown { get; private set; }

        public bool IsReady =>
            CurrentCooldown <= 0;

        public IReadOnlyList<ActionEffect> Effects =>
            _effects;

        internal ActiveAbility(
            string name,
            string description,
            AbilitySelectionType selectionType,
            int cooldownRounds,
            IEnumerable<AbilitySelectionRuleType> selectionRules = null)
        {
            Guard.NotNullOrWhiteSpace(
                name,
                nameof(name));

            Guard.NotNullOrWhiteSpace(
                description,
                nameof(description));

            if (cooldownRounds < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(cooldownRounds));
            }

            Name = name;
            Description = description;
            SelectionType = selectionType;
            CooldownRounds = cooldownRounds;

            if (selectionRules == null)
                return;

            foreach (AbilitySelectionRuleType rule
                     in selectionRules)
            {
                _selectionRules.Add(rule);
            }
        }

        internal void AddEffect(
            ActionEffect effect)
        {
            Guard.NotNull(
                effect,
                nameof(effect));

            _effects.Add(effect);
        }

        internal void StartCooldown()
        {
            CurrentCooldown =
                CooldownRounds;
        }

        internal void AdvanceCooldown()
        {
            if (CurrentCooldown <= 0)
                return;

            CurrentCooldown--;
        }

        internal virtual bool IsValidSelection(
            Unit owner,
            Unit target)
        {
            Guard.NotNull(
                owner,
                nameof(owner));

            Guard.NotNull(
                target,
                nameof(target));

            if (_selectionRules.Contains(
                    AbilitySelectionRuleType.TargetMustBeDamaged))
            {
                if (target.CurrentHealth >= target.BaseHealth)
                    return false;
            }

            return true;
        }

        internal virtual bool IsValidSelection(
            Unit owner,
            Unit target,
            RingPosition targetPosition,
            Ring ring)
        {
            Guard.NotNull(
                owner,
                nameof(owner));

            Guard.NotNull(
                target,
                nameof(target));

            Guard.NotNull(
                ring,
                nameof(ring));

            if (_selectionRules.Contains(
                    AbilitySelectionRuleType.TargetPositionMustDifferFromCurrent))
            {
                if (!ring.TryGetPosition(
                        target,
                        out RingPosition currentPosition))
                {
                    return false;
                }

                if (currentPosition == targetPosition)
                    return false;
            }

            if (_selectionRules.Contains(
                    AbilitySelectionRuleType.TargetPositionMustBeEmpty))
            {
                if (ring.TryGetObject(
                        targetPosition,
                        out _))
                {
                    return false;
                }
            }

            return true;
        }

        internal virtual bool IsValidSelection(
            Unit owner,
            Unit firstTarget,
            Unit secondTarget)
        {
            Guard.NotNull(
                owner,
                nameof(owner));

            Guard.NotNull(
                firstTarget,
                nameof(firstTarget));

            Guard.NotNull(
                secondTarget,
                nameof(secondTarget));

            if (_selectionRules.Contains(
                    AbilitySelectionRuleType.TargetsMustBeDifferent))
            {
                if (firstTarget.Id == secondTarget.Id)
                    return false;
            }

            return true;
        }
    }
}