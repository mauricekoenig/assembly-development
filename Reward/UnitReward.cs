using System;
using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class UnitReward :
        Reward
    {
        private readonly Unit _unit;


        internal UnitReward(
            Unit unit)
        {
            Guard.NotNull(
                unit,
                nameof(unit));

            _unit = unit;
        }


        internal override List<GameEvent> Apply(
            Run run)
        {
            Guard.NotNull(
                run,
                nameof(run));

            if (!run.TryAddUnit(
                    _unit))
            {
                // A full team is a normal gameplay state, not an exceptional one.
                // Returning no events tells the caller that this reward could not
                // be granted and prevents a misleading reward presentation.
                return new List<GameEvent>();
            }

            return new List<GameEvent>
            {
                new UnitAddedEvent(
                    new UnitInfo(_unit))
            };
        }


        internal override RewardInfo CreateInfo()
        {
            return new RewardInfo(
                RewardType.Unit,
                RewardContentType.Unit,
                _unit.DefinitionId,
                _unit.Name,
                $"Adds {_unit.Name} to your team.");
        }
    }
}
