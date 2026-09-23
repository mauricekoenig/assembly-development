using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    public sealed class ActiveAbilityInfo
    {
        public Guid Id { get; }

        public string Name { get; }
        public string Description { get; }

        public int CooldownRounds { get; }
        public int CurrentCooldown { get; }

        public bool IsReady { get; }

        public AbilitySelectionType SelectionType { get; }
        public IReadOnlyList<AbilitySelectionRuleType> SelectionRules { get; }

        internal ActiveAbilityInfo(
            ActiveAbility ability)
        {
            Guard.NotNull(ability, nameof(ability));

            Id = ability.Id;

            Name = ability.Name;
            Description = ability.Description;

            CooldownRounds = ability.CooldownRounds;
            CurrentCooldown = ability.CurrentCooldown;

            IsReady = ability.IsReady;

            SelectionType = ability.SelectionType;

            SelectionRules =
                ability.SelectionRules
                    .ToList();
        }

        public override string ToString()
        {
            string state =
                IsReady
                    ? "READY"
                    : $"Cooldown: {CurrentCooldown}";

            return $"{Name} [{state}]";
        }
    }
}