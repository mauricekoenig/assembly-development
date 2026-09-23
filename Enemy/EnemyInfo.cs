using System;

namespace GameEngine
{
    public sealed class EnemyInfo
    {
        public Guid Id { get; }

        public string DefinitionId { get; }

        public string Name { get; }

        public int CurrentHealth { get; }

        public int BaseHealth { get; }


        internal EnemyInfo(
            Enemy enemy)
        {
            Guard.NotNull(
                enemy,
                nameof(enemy));

            Id =
                enemy.Id;

            DefinitionId =
                enemy.DefinitionId;

            Name =
                enemy.Name;

            CurrentHealth =
                enemy.CurrentHealth;

            BaseHealth =
                enemy.BaseHealth;
        }


        public override string ToString()
        {
            return
                $"Name:   {Name}\n" +
                $"Health: {CurrentHealth}/{BaseHealth}";
        }
    }
}
