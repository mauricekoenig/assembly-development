using System.Collections.Generic;

namespace GameEngine
{
    public interface IHasTriggeredEffects
    {
        IReadOnlyList<Effect> Effects { get; }
    }
}