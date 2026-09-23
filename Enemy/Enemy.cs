using System;
using System.Collections.Generic;

namespace GameEngine
{
    public sealed class Enemy :
        ICombatant,
        IEffectSource,
        IHasPassives
    {
        private readonly List<EnemyAction> _actions =
            new List<EnemyAction>();

        private readonly List<Passive> _passives =
            new List<Passive>();

        private readonly LootTable _lootTable =
            new LootTable();

        private readonly SectorLayout _sectorLayout;


        public Guid Id { get; } =
            Guid.NewGuid();

        public string DefinitionId { get; }

        public string Name { get; }

        public EnemyType Type { get; }

        public int BaseHealth { get; }

        public int CurrentHealth { get; private set; }

        public bool IsAlive =>
            CurrentHealth > 0;

        public IReadOnlyList<Passive> Passives =>
            _passives;

        internal IReadOnlyList<EnemyAction> Actions =>
            _actions;


        internal Enemy(
            string definitionId,
            string name,
            EnemyType type,
            int baseHealth,
            SectorLayout sectorLayout)
        {
            Guard.NotNullOrWhiteSpace(
                definitionId,
                nameof(definitionId));

            Guard.NotNullOrWhiteSpace(
                name,
                nameof(name));

            if (baseHealth <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(baseHealth));
            }

            Guard.NotNull(
                sectorLayout,
                nameof(sectorLayout));

            DefinitionId =
                definitionId;

            Name =
                name;

            Type =
                type;

            BaseHealth =
                baseHealth;

            CurrentHealth =
                baseHealth;

            _sectorLayout =
                sectorLayout;
        }


        internal void AddPassive(
            Passive passive)
        {
            Guard.NotNull(
                passive,
                nameof(passive));

            _passives.Add(
                passive);
        }


        internal void AddAction(
            EnemyAction action)
        {
            Guard.NotNull(
                action,
                nameof(action));

            _actions.Add(
                action);
        }


        internal void AddLoot(
            Func<Reward> rewardFactory,
            int weight)
        {
            Guard.NotNull(
                rewardFactory,
                nameof(rewardFactory));

            _lootTable.Add(
                rewardFactory,
                weight);
        }


        internal Reward RollReward(
            Random random)
        {
            Guard.NotNull(
                random,
                nameof(random));

            return _lootTable.Roll(
                random);
        }


        internal SectorLayout CreateSectorLayout()
        {
            return _sectorLayout;
        }


        public void TakeDamage(
            int amount)
        {
            if (amount <= 0)
                return;

            CurrentHealth =
                Math.Max(
                    0,
                    CurrentHealth - amount);
        }


        public void Heal(
            int amount)
        {
            if (amount <= 0)
                return;

            CurrentHealth =
                Math.Min(
                    BaseHealth,
                    CurrentHealth + amount);
        }
    }
}
