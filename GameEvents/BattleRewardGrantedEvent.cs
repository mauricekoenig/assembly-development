using GameEngine;

public sealed class BattleRewardGrantedEvent :
    GameEvent
{
    public RewardInfo Reward { get; }

    internal BattleRewardGrantedEvent(
        RewardInfo reward)
    {
        Guard.NotNull(reward, nameof(reward));

        Reward = reward;
    }
}