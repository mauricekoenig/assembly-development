using System.Collections.Generic;

namespace GameEngine
{
    public sealed class EnemyDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public EnemyType Type { get; set; }

        public int BaseHealth { get; set; }

        public List<string> PassiveIds { get; set; }

        public List<EnemyActionDefinition> Actions { get; set; }

        public List<EnemySectorDefinition> Sectors { get; set; }

        public List<EnemyLootDefinition> Loot { get; set; }
    }
}