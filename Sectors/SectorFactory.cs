using GameEngine;

internal static class SectorFactory
{
    internal static Sector CreateHealSector(
        int amount,
        EffectTarget target = EffectTarget.RingObject)
    {
        Sector sector = new Sector(
            "SelfHeal",
            SectorType.Grass);

        sector.AddEffect(
            new ActionEffect(
                EffectFactory.CreateHealEffect(
                    amount,
                    name: "Grass Healing",
                    description: $"Restores {amount} health when the sector activates."),
                target));

        return sector;
    }

    internal static Sector CreateCursedSector(
        int amount,
        EffectTarget target = EffectTarget.RingObject)
    {
        Sector sector = new Sector(
            "Cursed",
            SectorType.Cursed);

        sector.AddEffect(
            new ActionEffect(
                EffectFactory.CreateDamageEffect(
                    amount,
                    DamageType.Magical,
                    name: "Cursed Ground",
                    description: $"Deals {amount} Magical damage when the sector activates."),
                target));

        return sector;
    }
}
