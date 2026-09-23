using System;

namespace GameEngine
{
    public abstract class RingObject :
        IGameEntity,
        IEffectSource,
        IEffectTarget
    {
        public string Name { get; set; }

        public Guid Id { get; } =
            Guid.NewGuid();
    }
}
