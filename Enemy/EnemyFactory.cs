using System;

namespace GameEngine
{
    public static class EnemyFactory
    {
        internal static Enemy Create(
            EnemyDefinition definition,
            ContentRegistry contentRegistry)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (contentRegistry == null)
            {
                throw new ArgumentNullException(
                    nameof(contentRegistry));
            }

            SectorLayout sectorLayout =
                CreateSectorLayout(
                    definition,
                    contentRegistry);

            Enemy enemy =
                new Enemy(
                    definition.Id,
                    definition.Name,
                    definition.Type,
                    definition.BaseHealth,
                    sectorLayout);

            AddPassives(
                enemy,
                definition,
                contentRegistry);

            AddActions(
                enemy,
                definition,
                contentRegistry);

            AddLoot(
                enemy,
                definition,
                contentRegistry);

            return enemy;
        }


        private static void AddPassives(
            Enemy enemy,
            EnemyDefinition definition,
            ContentRegistry contentRegistry)
        {
            if (definition.PassiveIds == null)
                return;

            foreach (string passiveId
                     in definition.PassiveIds)
            {
                enemy.AddPassive(
                    contentRegistry.CreatePassive(
                        passiveId));
            }
        }


        private static void AddActions(
            Enemy enemy,
            EnemyDefinition definition,
            ContentRegistry contentRegistry)
        {
            if (definition.Actions == null)
                return;

            foreach (EnemyActionDefinition actionDefinition
                     in definition.Actions)
            {
                EnemyAction action =
                    new EnemyAction(
                        actionDefinition.Id,
                        actionDefinition.Name,
                        actionDefinition.Description,
                        actionDefinition.Targeting,
                        actionDefinition.DelayTurns,
                        actionDefinition.Positions);

                if (actionDefinition.Effects != null)
                {
                    foreach (ActionEffectDefinition effectDefinition
                             in actionDefinition.Effects)
                    {
                        action.AddEffect(
                            EffectFactory.CreateActionEffect(
                                effectDefinition,
                                contentRegistry));
                    }
                }

                enemy.AddAction(
                    action);
            }
        }


        private static SectorLayout CreateSectorLayout(
            EnemyDefinition definition,
            ContentRegistry contentRegistry)
        {
            SectorLayout layout =
                new SectorLayout();

            if (definition.Sectors == null)
                return layout;

            foreach (EnemySectorDefinition sectorDefinition
                     in definition.Sectors)
            {
                Sector sector =
                    new Sector(
                        sectorDefinition.Name,
                        sectorDefinition.SectorType);

                if (sectorDefinition.Effects != null)
                {
                    foreach (ActionEffectDefinition effectDefinition
                             in sectorDefinition.Effects)
                    {
                        sector.AddEffect(
                            EffectFactory.CreateActionEffect(
                                effectDefinition,
                                contentRegistry));
                    }
                }

                if (!layout.TrySetSector(
                        sector,
                        sectorDefinition.Position))
                {
                    throw new InvalidOperationException(
                        $"Could not set sector '{sectorDefinition.Name}'.");
                }
            }

            return layout;
        }


        private static void AddLoot(
            Enemy enemy,
            EnemyDefinition definition,
            ContentRegistry contentRegistry)
        {
            if (definition.Loot == null)
                return;

            foreach (EnemyLootDefinition lootDefinition
                     in definition.Loot)
            {
                int weight =
                    lootDefinition.Weight;

                enemy.AddLoot(
                    () =>
                        contentRegistry.CreateReward(
                            lootDefinition),
                    weight);
            }
        }
    }
}
