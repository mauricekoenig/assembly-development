using System;
using System.Collections.Generic;

namespace GameEngine
{
    public sealed class Sector :
        IGameEntity,
        IEffectSource
    {
        private readonly List<ActionEffect> _effects =
            new List<ActionEffect>();

        public Guid Id { get; } =
            Guid.NewGuid();

        public string Name { get; }

        public SectorType SectorType { get; private set; }

        public IReadOnlyList<ActionEffect> Effects =>
            _effects;

        public Sector(
            string name,
            SectorType sectorType)
        {
            Name = name;
            SectorType = sectorType;
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
