using System;

namespace GameEngine
{
    public abstract class EventEncounter : IGameEntity
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Name { get; protected set; }

        protected EventEncounter (string name)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));

            Name = name;
        }
    }
}