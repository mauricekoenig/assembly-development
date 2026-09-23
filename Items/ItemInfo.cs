using System;

namespace GameEngine
{
    public sealed class ItemInfo
    {
        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }


        internal ItemInfo(
            Item item)
        {
            Guard.NotNull(item, nameof(item));

            Id = item.Id;
            Name = item.Name;
            Description = item.Description;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}