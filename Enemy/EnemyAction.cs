using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    internal sealed class EnemyAction : IGameEntity
    {
        public Guid Id { get; } =
            Guid.NewGuid();

        internal string DefinitionId { get; }

        internal string Name { get; }

        internal string Description { get; }

        internal EnemyActionTargeting Targeting { get; }

        internal int DelayTurns { get; }

        internal bool IsScheduled =>
            DelayTurns > 0;

        private readonly List<RingPosition> _positions =
            new List<RingPosition>();

        internal IReadOnlyList<RingPosition> Positions =>
            _positions;

        private readonly List<ActionEffect> _effects =
            new List<ActionEffect>();

        internal IReadOnlyList<ActionEffect> Effects =>
            _effects;

        private readonly List<Passive> _passives =
            new List<Passive>();

        public IReadOnlyList<Passive> Passives =>
            _passives;

        internal EnemyAction(
            string definitionId,
            string name,
            string description,
            EnemyActionTargeting targeting = EnemyActionTargeting.None,
            int delayTurns = 0,
            IEnumerable<RingPosition> positions = null)
        {
            Guard.NotNullOrWhiteSpace(
                definitionId,
                nameof(definitionId));

            Guard.NotNullOrWhiteSpace(
                name,
                nameof(name));

            Guard.NotNullOrWhiteSpace(
                description,
                nameof(description));

            if (delayTurns < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(delayTurns));
            }

            DefinitionId =
                definitionId;

            Name =
                name;

            Description =
                description;

            Targeting =
                targeting;

            DelayTurns =
                delayTurns;

            if (positions != null)
            {
                _positions.AddRange(
                    positions.Distinct());
            }

            ValidateTargeting();
        }

        private void ValidateTargeting()
        {
            if (Targeting == EnemyActionTargeting.RingPositions)
            {
                if (_positions.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"Enemy action '{Name}' uses " +
                        $"{EnemyActionTargeting.RingPositions} " +
                        "but has no positions.");
                }

                return;
            }

            if (_positions.Count > 0)
            {
                throw new InvalidOperationException(
                    $"Enemy action '{Name}' has ring positions " +
                    $"but uses targeting '{Targeting}'.");
            }
        }

        internal void AddEffect(
            ActionEffect effect)
        {
            Guard.NotNull(
                effect,
                nameof(effect));

            _effects.Add(
                effect);
        }
    }
}
