using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    internal static class EventRequirementEvaluator
    {
        internal static bool AreMet(
            Run run,
            IReadOnlyList<EventRequirementDefinition> requirements,
            out string reason)
        {
            Guard.NotNull(run, nameof(run));

            reason = string.Empty;

            if (requirements == null)
                return true;

            foreach (EventRequirementDefinition requirement in requirements)
            {
                if (requirement == null)
                {
                    reason = "Event requirement is missing.";
                    return false;
                }

                switch (requirement.Type)
                {
                    case EventRequirementType.MinimumCurrency:
                        if (run.Currency < requirement.Amount)
                        {
                            reason = $"Requires at least {requirement.Amount} currency.";
                            return false;
                        }
                        break;

                    case EventRequirementType.HasLivingUnit:
                        if (!run.Team.Any(unit => unit.IsAlive))
                        {
                            reason = "Requires at least one living unit.";
                            return false;
                        }
                        break;

                    case EventRequirementType.HasUnitTag:
                        if (!requirement.UnitTag.HasValue)
                        {
                            reason = "Required UnitTag is not configured.";
                            return false;
                        }

                        if (!run.Team.Any(unit =>
                                unit.IsAlive &&
                                unit.HasTag(requirement.UnitTag.Value)))
                        {
                            reason = $"Requires a living unit with tag '{requirement.UnitTag.Value}'.";
                            return false;
                        }
                        break;

                    default:
                        throw new NotSupportedException(
                            $"Event requirement '{requirement.Type}' is not supported.");
                }
            }

            return true;
        }
    }
}
