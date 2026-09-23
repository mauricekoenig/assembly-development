using System;

namespace GameEngine
{
    public sealed class ActiveAbilityUsedEvent : GameEvent
    {
        public Guid AbilityId { get; }
        public string AbilityName { get; }
        public UnitInfo Unit { get; }

        internal ActiveAbilityUsedEvent(
            Unit unit,
            ActiveAbility ability)
        {
            Guard.NotNull(unit, nameof(unit));
            Guard.NotNull(ability, nameof(ability));

            AbilityId = ability.Id;
            AbilityName = ability.Name;
            Unit = new UnitInfo(unit);
        }
    }
}