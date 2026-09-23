using System;

namespace GameEngine
{
    public static class UnitFactory
    {
        internal static Unit Create(
            UnitDefinition definition,
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

            Unit unit =
                new Unit(
                    definition.Id,
                    definition.BaseHealth,
                    definition.EffectSlotCapacity);

            unit.Name =
                definition.Name;

            AddTags(
                unit,
                definition);

            AddActions(
                unit,
                definition,
                contentRegistry);

            AddPassives(
                unit,
                definition,
                contentRegistry);

            AddAbilities(
                unit,
                definition,
                contentRegistry);

            return unit;
        }


        private static void AddTags(
            Unit unit,
            UnitDefinition definition)
        {
            if (definition.Tags == null)
                return;

            foreach (UnitTag tag
                     in definition.Tags)
            {
                unit.AddTag(
                    tag);
            }
        }


        private static void AddActions(
            Unit unit,
            UnitDefinition definition,
            ContentRegistry contentRegistry)
        {
            if (definition.Actions == null)
                return;

            foreach (UnitActionDefinition actionDefinition
                     in definition.Actions)
            {
                UnitAction action =
                    UnitActionFactory.Create(
                        actionDefinition,
                        contentRegistry);

                // Starting Actions use the exact same runtime collection
                // and resolution pipeline as run-added Actions. They only
                // differ in that they do not consume an Effect Slot.
                if (!unit.TryAddAction(
                        action,
                        consumesEffectSlot: false))
                {
                    throw new InvalidOperationException(
                        $"Could not add starting UnitAction '{actionDefinition.Id}' " +
                        $"to Unit '{definition.Id}'.");
                }
            }
        }


        private static void AddPassives(
            Unit unit,
            UnitDefinition definition,
            ContentRegistry contentRegistry)
        {
            if (definition.PassiveIds == null)
                return;

            foreach (string passiveId
                     in definition.PassiveIds)
            {
                Passive passive =
                    contentRegistry.CreatePassive(
                        passiveId);

                unit.AddInnatePassive(
                    passive);
            }
        }


        private static void AddAbilities(
            Unit unit,
            UnitDefinition definition,
            ContentRegistry contentRegistry)
        {
            if (definition.AbilityIds == null ||
                definition.AbilityIds.Count == 0)
            {
                return;
            }

            if (definition.AbilityIds.Count > 1)
            {
                throw new InvalidOperationException(
                    $"Unit '{definition.Id}' defines more than one Active Ability. " +
                    "Units have exactly one Active Ability slot.");
            }

            ActiveAbility ability =
                contentRegistry.CreateAbility(
                    definition.AbilityIds[0]);

            if (!unit.TrySetAbility(
                    ability))
            {
                throw new InvalidOperationException(
                    $"Unit '{definition.Id}' already has an Active Ability.");
            }
        }
    }
}
