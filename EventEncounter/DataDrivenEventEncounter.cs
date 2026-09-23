using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    internal sealed class DataDrivenEventEncounter : EventEncounter
    {
        private const int MaxAutomaticSteps = 100;

        private readonly EventDefinition _definition;
        private readonly Dictionary<string, EventNodeDefinition> _nodes;
        private readonly List<EventOption> _options = new List<EventOption>();

        internal string DefinitionId => _definition.Id;
        internal string Description => _definition.Description ?? string.Empty;

        internal EventNodeDefinition CurrentNode { get; private set; }

        internal string CurrentNodeId => CurrentNode?.Id;

        internal EventNodeType CurrentNodeType =>
            CurrentNode?.Type ?? EventNodeType.End;

        internal IReadOnlyList<EventOption> Options => _options;

        internal bool IsComplete =>
            CurrentNode == null ||
            CurrentNode.Type == EventNodeType.End;

        internal string ActivityId =>
            CurrentNode?.Type == EventNodeType.Activity
                ? CurrentNode.ActivityId ?? string.Empty
                : string.Empty;

        internal string ActivityContentId =>
            CurrentNode?.Type == EventNodeType.Activity
                ? CurrentNode.ActivityContentId ?? string.Empty
                : string.Empty;

        internal DataDrivenEventEncounter(
            EventDefinition definition)
            : base(definition?.Name)
        {
            Guard.NotNull(definition, nameof(definition));

            _definition = definition;

            _nodes =
                (definition.Nodes ?? new List<EventNodeDefinition>())
                    .ToDictionary(
                        node => node.Id,
                        StringComparer.OrdinalIgnoreCase);

            MoveToNode(definition.StartNodeId);
        }

        internal bool TryGetOption(
            Guid optionId,
            out EventOption option)
        {
            option = _options.FirstOrDefault(candidate => candidate.Id == optionId);
            return option != null;
        }

        internal bool CanChooseOption(
            Run run,
            EventOption option,
            out string reason)
        {
            Guard.NotNull(run, nameof(run));
            Guard.NotNull(option, nameof(option));

            reason = string.Empty;

            if (CurrentNode == null ||
                CurrentNode.Type != EventNodeType.Choice)
            {
                reason = "The event is not currently waiting for a choice.";
                return false;
            }

            if (option.Definition == null)
            {
                reason = "The selected option is not data-driven.";
                return false;
            }

            if (!EventRequirementEvaluator.AreMet(
                    run,
                    option.Definition.Requirements,
                    out reason))
            {
                return false;
            }

            if (!EventOutcomeResolver.CanPayCosts(
                    run,
                    option.Definition.Costs,
                    out reason))
            {
                return false;
            }

            return true;
        }

        internal List<GameEvent> ContinuePresentation(
    Run run,
    ContentRegistry contentRegistry)
        {
            Guard.NotNull(
                run,
                nameof(run));

            Guard.NotNull(
                contentRegistry,
                nameof(contentRegistry));


            if (CurrentNode == null ||
                CurrentNode.Type != EventNodeType.Presentation)
            {
                throw new InvalidOperationException(
                    "The event is not currently waiting on a Presentation node.");
            }


            EventNodeDefinition presentationNode =
                CurrentNode;


            List<GameEvent> events =
                EventOutcomeResolver.Apply(
                    run,
                    presentationNode.Outcomes,
                    contentRegistry);


            MoveToNode(
                presentationNode.NextNodeId);


            events.AddRange(
                ResolveAutomaticNodes(
                    run,
                    contentRegistry));


            return events;
        }

        internal List<GameEvent> ChooseOption(
            Run run,
            EventOption option,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(run, nameof(run));
            Guard.NotNull(option, nameof(option));
            Guard.NotNull(contentRegistry, nameof(contentRegistry));

            if (!CanChooseOption(run, option, out string reason))
            {
                throw new InvalidOperationException(reason);
            }

            List<GameEvent> events = new List<GameEvent>();

            events.AddRange(
                EventOutcomeResolver.Apply(
                    run,
                    option.Definition.Costs,
                    contentRegistry));

            events.AddRange(
                EventOutcomeResolver.Apply(
                    run,
                    option.Definition.Outcomes,
                    contentRegistry));

            MoveToNode(option.Definition.NextNodeId);

            events.AddRange(
                ResolveAutomaticNodes(
                    run,
                    contentRegistry));

            return events;
        }

        internal bool TryGetActivityResult(
            string resultId,
            out EventActivityResultDefinition result)
        {
            result = null;

            if (CurrentNode == null ||
                CurrentNode.Type != EventNodeType.Activity ||
                string.IsNullOrWhiteSpace(resultId))
            {
                return false;
            }

            result =
                (CurrentNode.ActivityResults ??
                    new List<EventActivityResultDefinition>())
                    .FirstOrDefault(candidate =>
                        candidate != null &&
                        string.Equals(
                            candidate.ResultId,
                            resultId,
                            StringComparison.OrdinalIgnoreCase));

            return result != null;
        }

        internal List<GameEvent> CompleteActivity(
            Run run,
            EventActivityResultDefinition result,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(run, nameof(run));
            Guard.NotNull(result, nameof(result));
            Guard.NotNull(contentRegistry, nameof(contentRegistry));

            if (CurrentNode == null ||
                CurrentNode.Type != EventNodeType.Activity)
            {
                throw new InvalidOperationException(
                    "The event is not currently waiting for an activity result.");
            }

            List<GameEvent> events =
                EventOutcomeResolver.Apply(
                    run,
                    result.Outcomes,
                    contentRegistry);

            MoveToNode(result.NextNodeId);

            events.AddRange(
                ResolveAutomaticNodes(
                    run,
                    contentRegistry));

            return events;
        }

        internal List<GameEvent> ResolveAutomaticNodes(
            Run run,
            ContentRegistry contentRegistry)
        {
            Guard.NotNull(run, nameof(run));
            Guard.NotNull(contentRegistry, nameof(contentRegistry));

            List<GameEvent> events = new List<GameEvent>();
            int automaticSteps = 0;

            while (CurrentNode != null &&
                   CurrentNode.Type == EventNodeType.Automatic)
            {
                automaticSteps++;

                if (automaticSteps > MaxAutomaticSteps)
                {
                    throw new InvalidOperationException(
                        $"Event '{DefinitionId}' exceeded {MaxAutomaticSteps} automatic steps. " +
                        "The event flow likely contains an automatic loop.");
                }

                EventNodeDefinition automaticNode = CurrentNode;

                events.AddRange(
                    EventOutcomeResolver.Apply(
                        run,
                        automaticNode.Outcomes,
                        contentRegistry));

                MoveToNode(automaticNode.NextNodeId);
            }

            return events;
        }

        private void MoveToNode(string nodeId)
        {
            if (string.IsNullOrWhiteSpace(nodeId))
            {
                throw new InvalidOperationException(
                    $"Event '{DefinitionId}' attempted to move to an empty node id.");
            }

            if (!_nodes.TryGetValue(nodeId, out EventNodeDefinition node))
            {
                throw new InvalidOperationException(
                    $"Event '{DefinitionId}' node '{nodeId}' could not be found.");
            }

            CurrentNode = node;

            RebuildRuntimeOptions();
        }

        private void RebuildRuntimeOptions()
        {
            _options.Clear();

            if (CurrentNode == null ||
                CurrentNode.Type != EventNodeType.Choice)
            {
                return;
            }

            foreach (EventOptionDefinition optionDefinition
                     in CurrentNode.Options ??
                        new List<EventOptionDefinition>())
            {
                _options.Add(
                    new EventOption(optionDefinition));
            }
        }
    }
}
