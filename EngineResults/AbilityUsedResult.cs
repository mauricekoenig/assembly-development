using System;

namespace GameEngine
{
    public sealed class AbilityUsedResult :
        EngineSuccessResult
    {
        public Guid AbilityId { get; }

        internal AbilityUsedResult(Guid abilityId)
            : base(
                "The active ability was used successfully.")
        {
            AbilityId = abilityId;
        }
    }
}