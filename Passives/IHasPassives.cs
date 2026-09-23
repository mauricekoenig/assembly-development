using System.Collections.Generic;

namespace GameEngine
{
    public interface IHasPassives
    {
        IReadOnlyList<Passive> Passives { get; }
    }
}