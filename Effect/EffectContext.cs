using GameEngine;

public class EffectContext
{
    internal Run Run { get; set; }

    public IEffectSource Source { get; set; }

    public Enemy Enemy { get; set; }

    public Ring Ring { get; set; }

    public RingObject RingObject { get; set; }

    public RingObject SecondRingObject { get; set; }

    public RingPosition? TargetPosition { get; set; }

}