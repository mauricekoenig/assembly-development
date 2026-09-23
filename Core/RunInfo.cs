using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public sealed class RunInfo
{
    public Guid Id { get; }

    public string RunPlanId { get; }

    public string RunPlanName { get; }

    public int StageCount { get; }

    public int CurrentStageNumber { get; }

    public int Currency { get; }

    public int TeamCount { get; }

    public int BlessingCount { get; }

    public int ItemCount { get; }

    public IReadOnlyList<ItemInfo> Items { get; }

    public int RelicCount { get; }

    public IReadOnlyList<RelicInfo> Relics { get; }

    public RunState State { get; }


    internal RunInfo(
        Run run)
    {
        Guard.NotNull(run, nameof(run));

        Id = run.Id;
        RunPlanId = run.RunPlanId;
        RunPlanName = run.RunPlanName;
        StageCount = run.StageCount;
        CurrentStageNumber = run.CurrentStageNumber;
        Currency = run.Currency;
        TeamCount = run.Team.Count;
        BlessingCount = run.BlessingCount;
        ItemCount = run.ItemCount;
        State = run.State;

        List<ItemInfo> items = new List<ItemInfo>();

        foreach (Item item in run.Items)
        {
            items.Add(
                new ItemInfo(item));
        }

        Items = items.AsReadOnly();

        RelicCount = run.RelicCount;

        Relics = run.Relics
            .Select(relic =>
                new RelicInfo(relic))
            .ToList();
    }


    public override string ToString()
    {
        string stageText =
            CurrentStageNumber > 0
                ? $"{CurrentStageNumber}/{StageCount}"
                : $"-/ {StageCount}";

        return
            $"Run {Id}\n" +
            $"Plan: {RunPlanName} [{RunPlanId}]\n" +
            $"Stage: {stageText}\n" +
            $"Currency: {Currency}\n" +
            $"Team: {TeamCount}\n" +
            $"Blessings: {BlessingCount}\n" +
            $"Items: {ItemCount}\n" +
            $"State: {State}\n" +
            $"Relics: {RelicCount}\n";
    }
}
