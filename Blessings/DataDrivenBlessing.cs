using System;
using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class DataDrivenBlessing : BaseBlessing
    {
        private readonly IReadOnlyList<BlessingEffectDefinition>
            _effects;


        internal DataDrivenBlessing(
            BlessingDefinition definition)
            : base(
                GetDefinitionId(definition),
                GetName(definition),
                definition.Description)
        {
            _effects =
                definition.Effects ??
                new List<BlessingEffectDefinition>();
        }


        public override void Apply(
            Run run)
        {
            Guard.NotNull(
                run,
                nameof(run));

            foreach (BlessingEffectDefinition effect
                     in _effects)
            {
                ApplyEffect(
                    run,
                    effect);
            }
        }


        private static void ApplyEffect(
            Run run,
            BlessingEffectDefinition effect)
        {
            Guard.NotNull(
                effect,
                nameof(effect));

            switch (effect.Type)
            {
                case BlessingEffectType.GainCurrency:
                    run.AddCurrency(
                        effect.Amount);
                    return;

                case BlessingEffectType.ModifySectorValue:
                    if (!effect.Sector.HasValue)
                    {
                        throw new InvalidOperationException(
                            "ModifySectorValue Blessing effect requires a Sector.");
                    }

                    ApplySectorValue(
                        run,
                        effect.Sector.Value,
                        effect.Amount);
                    return;

                default:
                    throw new NotSupportedException(
                        $"Blessing effect type '{effect.Type}' is not supported.");
            }
        }


        private static void ApplySectorValue(
            Run run,
            SectorType sector,
            int amount)
        {
            switch (sector)
            {
                case SectorType.Fire:
                    run.SectorModifiers.AddFireValue(amount);
                    return;

                case SectorType.Water:
                    run.SectorModifiers.AddWaterValue(amount);
                    return;

                case SectorType.Grass:
                    run.SectorModifiers.AddGrassValue(amount);
                    return;

                case SectorType.Cursed:
                    run.SectorModifiers.AddCursedValue(amount);
                    return;

                default:
                    throw new NotSupportedException(
                        $"Sector '{sector}' is not supported by Blessings.");
            }
        }


        private static string GetDefinitionId(
            BlessingDefinition definition)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNullOrWhiteSpace(
                definition.Id,
                nameof(definition.Id));

            return definition.Id;
        }


        private static string GetName(
            BlessingDefinition definition)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            Guard.NotNullOrWhiteSpace(
                definition.Name,
                nameof(definition.Name));

            return definition.Name;
        }
    }
}
