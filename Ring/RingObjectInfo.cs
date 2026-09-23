using System;

namespace GameEngine
{
    public class RingObjectInfo
    {
        public Guid Id { get; }
        public string Name { get; }

        internal RingObjectInfo(RingObject ringObject)
        {
            Guard.NotNull(
                ringObject,
                nameof(ringObject));

            Id =
                ringObject.Id;

            Name =
                ringObject.Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
