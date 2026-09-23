using System;

namespace GameEngine
{
    public abstract class Commander : IGameEntity
    {
        public string Name { get; protected set; }

        public Guid Id { get; } =
            Guid.NewGuid();

        public abstract string StartUnitId { get; }
    }

    public sealed class TestCommander : Commander
    {
        public override string StartUnitId =>
            "test_unit";

        public TestCommander()
        {
            Name = "Test Commander";
        }
    }
}