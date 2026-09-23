using System.Collections.Generic;

namespace GameEngine
{
    public abstract class EffectAction
    {
        public abstract List<GameEvent> Execute(
            EffectContext context,
            IEffectTarget target);
    }
}