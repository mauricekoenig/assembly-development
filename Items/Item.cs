using System;

namespace GameEngine
{
    public abstract class Item :
        IGameEntity
    {
        public Guid Id { get; } =
            Guid.NewGuid();

        public string Name { get; }

        public string Description { get; }


        protected Item(
            string name,
            string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(
                    "Item name cannot be empty.",
                    nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException(
                    "Item description cannot be empty.",
                    nameof(description));

            Name = name;
            Description = description;
        }
    }
}