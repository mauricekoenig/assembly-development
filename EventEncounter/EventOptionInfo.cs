using System;

namespace GameEngine
{
    public sealed class EventOptionInfo
    {
        public Guid Id { get; }
        public string DefinitionId { get; }
        public string Name { get; }
        public string Description { get; }
        public bool IsAvailable { get; }
        public string UnavailableReason { get; }

        internal EventOptionInfo(
            EventOption option,
            Run run)
        {
            Guard.NotNull(option, nameof(option));
            Guard.NotNull(run, nameof(run));

            Id = option.Id;
            DefinitionId = option.DefinitionId;
            Name = option.Name;
            Description = option.Description;

            if (!EventRequirementEvaluator.AreMet(
                    run,
                    option.Definition.Requirements,
                    out string reason))
            {
                IsAvailable = false;
                UnavailableReason = reason;
                return;
            }

            if (!EventOutcomeResolver.CanPayCosts(
                    run,
                    option.Definition.Costs,
                    out reason))
            {
                IsAvailable = false;
                UnavailableReason = reason;
                return;
            }

            IsAvailable = true;
            UnavailableReason = string.Empty;
        }
    }
}
