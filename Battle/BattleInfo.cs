using GameEngine;

public sealed class BattleInfo
{
    public EnemyInfo Enemy { get; }
    public RingInfo Ring { get; }
    public SectorLayoutInfo Sectors { get; }

    public int RepositionsRemaining { get; }
    public int ActiveAbilityUsesRemaining { get; }

    internal BattleInfo(Battle battle)
    {
        Guard.NotNull(battle, nameof(battle));

        Enemy = new EnemyInfo(battle.Enemy);
        Ring = new RingInfo(battle.Ring);
        Sectors = new SectorLayoutInfo(battle.SectorLayout);

        RepositionsRemaining =
            battle.RepositionsRemaining;

        ActiveAbilityUsesRemaining =
            battle.ActiveAbilityUsesRemaining;
    }

    public override string ToString()
    {
        return
            $"ENEMY\n{Enemy}\n\n" +
            $"RING\n{Ring}\n\n" +
            $"SECTORS\n{Sectors}\n\n" +
            $"PLAYER WINDOW\n" +
            $"Repositions Remaining: " +
            $"{(RepositionsRemaining == int.MaxValue ? "Unlimited" : RepositionsRemaining.ToString())}\n" +
            $"Active Ability Uses Remaining: {ActiveAbilityUsesRemaining}";
    }
}