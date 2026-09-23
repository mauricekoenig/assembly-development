using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEngine
{
    public sealed class ContentRegistry
    {
        internal const string LegacyGoldBlessingId =
            "blessing:gold_50";

        internal const string HeartOfTheDeepRelicId =
            "relic:heart_of_the_deep";

        private const string EffectsResource =
            "GameEngine.Content.effects.json";

        private const string PassivesResource =
            "GameEngine.Content.passives.json";

        private const string UnitsResource =
            "GameEngine.Content.units.json";

        private const string AbilitiesResource =
            "GameEngine.Content.abilities.json";

        private const string EnemiesResource =
            "GameEngine.Content.enemies.json";

        private const string EventsResource =
            "GameEngine.Content.events.json";

        private const string VendorsResource =
            "GameEngine.Content.vendors.json";

        private const string BlessingsResource =
            "GameEngine.Content.blessings.json";

        private const string RelicsResource =
            "GameEngine.Content.relics.json";

        private const string PoolsResource =
            "GameEngine.Content.pools.json";

        private const string RunPlansResource =
            "GameEngine.Content.run_plans.json";

        private readonly string _contentDirectory;

        private readonly Dictionary<string, EnemyDefinition>
            _enemies =
                new Dictionary<string, EnemyDefinition>();

        private readonly Dictionary<string, EventDefinition>
            _events =
                new Dictionary<string, EventDefinition>();

        private readonly Dictionary<string, BlessingDefinition>
            _blessings =
                new Dictionary<string, BlessingDefinition>();

        private readonly Dictionary<string, RelicDefinition>
            _relics =
                new Dictionary<string, RelicDefinition>();

        private readonly Dictionary<string, VendorDefinition>
            _vendors =
                new Dictionary<string, VendorDefinition>();

        private readonly Dictionary<string, PoolDefinition>
            _pools =
                new Dictionary<string, PoolDefinition>();

        private readonly Dictionary<string, RunPlanDefinition>
            _runPlans =
                new Dictionary<string, RunPlanDefinition>();


        private readonly Dictionary<string, EffectDefinition>
            _effects =
                new Dictionary<string, EffectDefinition>();

        private readonly Dictionary<string, PassiveDefinition>
            _passives =
                new Dictionary<string, PassiveDefinition>();

        private readonly Dictionary<string, UnitDefinition>
            _units =
                new Dictionary<string, UnitDefinition>();

        private readonly Dictionary<string, AbilityDefinition>
            _abilities =
                new Dictionary<string, AbilityDefinition>();


        public ContentRegistry()
            : this(null)
        {
        }


        public ContentRegistry(
            string contentDirectory)
        {
            if (!string.IsNullOrWhiteSpace(
                    contentDirectory))
            {
                string fullPath =
                    Path.GetFullPath(
                        contentDirectory);

                if (!Directory.Exists(fullPath))
                {
                    throw new DirectoryNotFoundException(
                        $"Content directory was not found: {fullPath}");
                }

                _contentDirectory =
                    fullPath;
            }

            LoadEffects();
            LoadPassives();
            LoadAbilities();
            LoadUnits();
            LoadBlessings();
            LoadRelics();
            LoadEnemies();
            LoadEvents();
            LoadVendors();
            LoadPools();
            LoadRunPlans();
            ValidateRunPlans();
        }


        private List<T> LoadContent<T>(
            string resourceName,
            string fileName)
        {
            if (string.IsNullOrWhiteSpace(
                    _contentDirectory))
            {
                return JsonContentLoader.Load<T>(
                    resourceName);
            }

            return JsonContentLoader.LoadFile<T>(
                Path.Combine(
                    _contentDirectory,
                    fileName));
        }


        // =========================================================
        // LOAD
        // =========================================================

        private void LoadBlessings()
        {
            List<BlessingDefinition> definitions =
                LoadContent<BlessingDefinition>(
                    BlessingsResource,
                    "blessings.json");

            foreach (BlessingDefinition definition
                     in definitions)
            {
                ValidateId(
                    definition?.Id,
                    "Blessing");

                if (!_blessings.TryAdd(
                        definition.Id,
                        definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Blessing Id: {definition.Id}");
                }

                ValidateBlessing(
                    definition);
            }
        }

        private void LoadRelics()
        {
            List<RelicDefinition> definitions =
                LoadContent<RelicDefinition>(
                    RelicsResource,
                    "relics.json");

            foreach (RelicDefinition definition
                     in definitions)
            {
                ValidateId(
                    definition?.Id,
                    "Relic");

                if (!_relics.TryAdd(
                        definition.Id,
                        definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Relic Id: {definition.Id}");
                }

                ValidateRelic(
                    definition);
            }
        }


        private void LoadVendors()
        {
            List<VendorDefinition> definitions =
                LoadContent<VendorDefinition>(
                    VendorsResource,
                    "vendors.json");

            foreach (VendorDefinition definition in definitions)
            {
                ValidateId(
                    definition?.Id,
                    "Vendor");

                if (!_vendors.TryAdd(
                        definition.Id,
                        definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Vendor Id: {definition.Id}");
                }

                ValidateVendor(
                    definition);
            }
        }


        private void LoadPools()
        {
            List<PoolDefinition> definitions =
                LoadContent<PoolDefinition>(
                    PoolsResource,
                    "pools.json");

            foreach (PoolDefinition definition in definitions)
            {
                ValidateId(definition?.Id, "Pool");

                if (!_pools.TryAdd(definition.Id, definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Pool Id: {definition.Id}");
                }

                NormalizeLegacyPoolKind(definition);
                ValidatePool(definition);
            }
        }

        private void LoadRunPlans()
        {
            List<RunPlanDefinition> definitions =
                LoadContent<RunPlanDefinition>(
                    RunPlansResource,
                    "run_plans.json");

            foreach (RunPlanDefinition definition in definitions)
            {
                ValidateId(definition?.Id, "Run Plan");

                if (!_runPlans.TryAdd(definition.Id, definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Run Plan Id: {definition.Id}");
                }
            }
        }

        private void LoadEnemies()
        {
            List<EnemyDefinition> definitions =
                LoadContent<EnemyDefinition>(
                    EnemiesResource,
                    "enemies.json");

            foreach (EnemyDefinition definition in definitions)
            {
                ValidateId(
                    definition?.Id,
                    "Enemy");

                if (!_enemies.TryAdd(
                        definition.Id,
                        definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Enemy Id: {definition.Id}");
                }

                ValidateEnemy(
                    definition);
            }
        }

        private void LoadEvents()
        {
            List<EventDefinition> definitions =
                LoadContent<EventDefinition>(
                    EventsResource,
                    "events.json");

            foreach (EventDefinition definition in definitions)
            {
                ValidateId(
                    definition?.Id,
                    "Event");

                if (!_events.TryAdd(
                        definition.Id,
                        definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Event Id: {definition.Id}");
                }

                ValidateEvent(definition);
            }
        }

        internal EventDefinition GetEvent(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id) ||
                !_events.TryGetValue(
                    id,
                    out EventDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Event definition not found: {id}");
            }

            return definition;
        }

        internal DataDrivenEventEncounter CreateEventFromPool(
            string poolId,
            Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            PoolDefinition pool =
                GetPool(poolId);

            if (pool.Kind != PoolKind.Event)
            {
                throw new InvalidOperationException(
                    $"Pool '{pool.Id}' has Kind '{pool.Kind}', but 'Event' is required.");
            }

            List<PoolEntryDefinition> candidates =
                (pool.Entries ?? new List<PoolEntryDefinition>())
                    .Where(entry =>
                        entry != null &&
                        entry.Type == PoolEntryType.Event &&
                        !string.IsNullOrWhiteSpace(entry.ContentId) &&
                        entry.Weight > 0)
                    .GroupBy(entry =>
                        entry.ContentId,
                        StringComparer.OrdinalIgnoreCase)
                    .Select(group => group.First())
                    .ToList();

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Event pool '{pool.Id}' contains no weighted Event entries.");
            }

            int totalWeight =
                candidates.Sum(entry => entry.Weight);

            int roll =
                random.Next(totalWeight);

            int cumulativeWeight = 0;
            PoolEntryDefinition selected = candidates[0];

            foreach (PoolEntryDefinition candidate in candidates)
            {
                cumulativeWeight += candidate.Weight;

                if (roll < cumulativeWeight)
                {
                    selected = candidate;
                    break;
                }
            }

            return new DataDrivenEventEncounter(
                GetEvent(selected.ContentId));
        }

        internal VendorDefinition GetVendor(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id) ||
                !_vendors.TryGetValue(
                    id,
                    out VendorDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Vendor definition not found: {id}");
            }

            return definition;
        }


        internal ShopEncounter CreateVendorFromPool(
            string poolId,
            Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            PoolDefinition pool =
                GetPool(
                    poolId);

            if (pool.Kind != PoolKind.Vendor)
            {
                throw new InvalidOperationException(
                    $"Pool '{pool.Id}' has Kind '{pool.Kind}', but 'Vendor' is required.");
            }

            List<PoolEntryDefinition> candidates =
                (pool.Entries ??
                    new List<PoolEntryDefinition>())
                    .Where(entry =>
                        entry != null &&
                        entry.Type == PoolEntryType.Vendor &&
                        !string.IsNullOrWhiteSpace(
                            entry.ContentId) &&
                        entry.Weight > 0)
                    .GroupBy(
                        entry => entry.ContentId,
                        StringComparer.OrdinalIgnoreCase)
                    .Select(group =>
                        group.First())
                    .ToList();

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Vendor pool '{pool.Id}' contains no weighted Vendor entries.");
            }

            int totalWeight =
                candidates.Sum(entry =>
                    entry.Weight);

            int roll =
                random.Next(
                    totalWeight);

            int cumulativeWeight = 0;

            PoolEntryDefinition selected =
                candidates[0];

            foreach (PoolEntryDefinition candidate
                     in candidates)
            {
                cumulativeWeight +=
                    candidate.Weight;

                if (roll < cumulativeWeight)
                {
                    selected =
                        candidate;

                    break;
                }
            }

            return new DataDrivenVendor(
                GetVendor(
                    selected.ContentId),
                random,
                this);
        }


        internal EnemyDefinition GetEnemy(
            string id)
        {
            if (!_enemies.TryGetValue(
                    id,
                    out EnemyDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Enemy definition not found: {id}");
            }

            return definition;
        }

        public Enemy CreateEnemy(
            string id)
        {
            return EnemyFactory.Create(
                GetEnemy(id),
                this);
        }

        internal Enemy CreateRandomEnemy(
            EnemyType enemyType,
            Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            List<EnemyDefinition> candidates =
                _enemies.Values
                    .Where(definition =>
                        definition.Type == enemyType)
                    .OrderBy(definition =>
                        definition.Id)
                    .ToList();

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No Enemy definitions of type '{enemyType}' are available.");
            }

            EnemyDefinition selected =
                candidates[random.Next(candidates.Count)];

            return EnemyFactory.Create(
                selected,
                this);
        }


        internal Enemy CreateEnemyFromPool(
            string poolId,
            EnemyType enemyType,
            Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            PoolDefinition pool =
                GetPool(
                    poolId);

            PoolKind expectedKind =
                enemyType == EnemyType.Boss
                    ? PoolKind.BossEnemy
                    : PoolKind.NormalEnemy;

            if (pool.Kind != expectedKind)
            {
                throw new InvalidOperationException(
                    $"Pool '{pool.Id}' has Kind '{pool.Kind}', " +
                    $"but '{expectedKind}' is required for {enemyType} enemies.");
            }

            List<PoolEntryDefinition> candidates =
                (pool.Entries ??
                    new List<PoolEntryDefinition>())
                    .Where(entry =>
                        entry != null &&
                        entry.Type == PoolEntryType.Enemy &&
                        !string.IsNullOrWhiteSpace(
                            entry.ContentId) &&
                        entry.Weight > 0)
                    .Where(entry =>
                        GetEnemy(entry.ContentId).Type ==
                            enemyType)
                    .GroupBy(entry =>
                        entry.ContentId)
                    .Select(group =>
                        group.First())
                    .ToList();

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Enemy pool '{pool.Id}' contains no weighted " +
                    $"{enemyType} Enemy entries.");
            }

            int totalWeight =
                candidates.Sum(entry =>
                    entry.Weight);

            int roll =
                random.Next(
                    totalWeight);

            int cumulativeWeight =
                0;

            PoolEntryDefinition selected =
                candidates[0];

            foreach (PoolEntryDefinition candidate
                     in candidates)
            {
                cumulativeWeight +=
                    candidate.Weight;

                if (roll <
                    cumulativeWeight)
                {
                    selected =
                        candidate;

                    break;
                }
            }

            return EnemyFactory.Create(
                GetEnemy(
                    selected.ContentId),
                this);
        }

        private void LoadEffects()
        {
            List<EffectDefinition> definitions =
                LoadContent<EffectDefinition>(
                    EffectsResource,
                    "effects.json");

            foreach (EffectDefinition definition in definitions)
            {
                ValidateId(
                    definition?.Id,
                    "Effect");

                if (!_effects.TryAdd(
                    definition.Id,
                    definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Effect Id: {definition.Id}");
                }
            }
        }

        private void LoadPassives()
        {
            List<PassiveDefinition> definitions =
                LoadContent<PassiveDefinition>(
                    PassivesResource,
                    "passives.json");

            foreach (PassiveDefinition definition in definitions)
            {
                ValidateId(
                    definition?.Id,
                    "Passive");

                if (!_passives.TryAdd(
                    definition.Id,
                    definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Passive Id: {definition.Id}");
                }
            }
        }

        private void LoadAbilities()
        {
            List<AbilityDefinition> definitions =
                LoadContent<AbilityDefinition>(
                    AbilitiesResource,
                    "abilities.json");

            foreach (AbilityDefinition definition in definitions)
            {
                ValidateId(
                    definition?.Id,
                    "Ability");

                if (!_abilities.TryAdd(
                    definition.Id,
                    definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Ability Id: {definition.Id}");
                }
            }
        }

        private void LoadUnits()
        {
            List<UnitDefinition> definitions =
                LoadContent<UnitDefinition>(
                    UnitsResource,
                    "units.json");

            HashSet<string> unitActionIds =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (UnitDefinition definition in definitions)
            {
                ValidateId(
                    definition?.Id,
                    "Unit");

                if (!_units.TryAdd(
                    definition.Id,
                    definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Unit Id: {definition.Id}");
                }

                ValidateUnit(
                    definition,
                    unitActionIds);
            }
        }


        internal PoolDefinition GetPool(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id) ||
                !_pools.TryGetValue(id, out PoolDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Pool definition not found: {id}");
            }

            return definition;
        }

        internal RunPlanDefinition GetDefaultRunPlan()
        {
            List<RunPlanDefinition> defaults =
                _runPlans.Values
                    .Where(plan => plan.IsDefault)
                    .ToList();

            if (defaults.Count != 1)
            {
                throw new InvalidOperationException(
                    $"Exactly one default Run Plan is required. Found: {defaults.Count}.");
            }

            return defaults[0];
        }

        internal RunPlanDefinition GetRunPlan(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id) ||
                !_runPlans.TryGetValue(
                    id,
                    out RunPlanDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Run Plan definition not found: {id}");
            }

            return definition;
        }

        // =========================================================
        // DEFINITIONS
        // =========================================================

        internal EffectDefinition GetEffect(
            string id)
        {
            if (!_effects.TryGetValue(
                    id,
                    out EffectDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Effect definition not found: {id}");
            }

            return definition;
        }

        internal PassiveDefinition GetPassive(
            string id)
        {
            if (!_passives.TryGetValue(
                    id,
                    out PassiveDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Passive definition not found: {id}");
            }

            return definition;
        }

        internal AbilityDefinition GetAbility(
            string id)
        {
            if (!_abilities.TryGetValue(
                    id,
                    out AbilityDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Ability definition not found: {id}");
            }

            return definition;
        }

        internal UnitDefinition GetUnit(
            string id)
        {
            if (!_units.TryGetValue(
                    id,
                    out UnitDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Unit definition not found: {id}");
            }

            return definition;
        }


        // =========================================================
        // CREATE
        // =========================================================

        public Effect CreateEffect(
            string id)
        {
            return EffectFactory.Create(
                GetEffect(id));
        }

        public Passive CreatePassive(
            string id)
        {
            return PassiveFactory.Create(
                GetPassive(id),
                this);
        }

        public ActiveAbility CreateAbility(
            string id)
        {
            return AbilityFactory.Create(
                GetAbility(id),
                this);
        }

        public Unit CreateUnit(
            string id)
        {
            return UnitFactory.Create(
                GetUnit(id),
                this);
        }


        public Item CreateItem(
            ItemReference itemReference)
        {
            return ItemFactory.Create(
                itemReference,
                this);
        }


        internal Reward CreateReward(
            RewardDefinition definition)
        {
            ValidateRewardDefinition(
                definition,
                "Reward");

            return RewardFactory.Create(
                definition,
                this);
        }

        internal ShopOffer CreateShopOffer(
            VendorOfferDefinition definition)
        {
            Guard.NotNull(
                definition,
                nameof(definition));

            if (string.IsNullOrWhiteSpace(
                    definition.ContentId))
            {
                throw new InvalidOperationException(
                    "Vendor offer ContentId cannot be empty.");
            }

            switch (definition.Type)
            {
                case VendorOfferType.EffectRecipe:
                    return new ItemShopOffer(
                        definition.Type,
                        definition.ContentId,
                        CreateItem(
                            new ItemReference
                            {
                                Type = ItemType.EffectRecipe,
                                ContentId = definition.ContentId
                            }),
                        definition.Price);

                case VendorOfferType.PassiveRecipe:
                    return new ItemShopOffer(
                        definition.Type,
                        definition.ContentId,
                        CreateItem(
                            new ItemReference
                            {
                                Type = ItemType.PassiveRecipe,
                                ContentId = definition.ContentId
                            }),
                        definition.Price);

                case VendorOfferType.AbilityRecipe:
                    return new ItemShopOffer(
                        definition.Type,
                        definition.ContentId,
                        CreateItem(
                            new ItemReference
                            {
                                Type = ItemType.AbilityRecipe,
                                ContentId = definition.ContentId
                            }),
                        definition.Price);

                case VendorOfferType.Blessing:
                    return new BlessingShopOffer(
                        definition.ContentId,
                        CreateBlessing(
                            definition.ContentId),
                        definition.Price);

                case VendorOfferType.Relic:
                    return new RelicShopOffer(
                        definition.ContentId,
                        CreateRelic(
                            definition.ContentId),
                        definition.Price);

                default:
                    throw new NotSupportedException(
                        $"Vendor offer type '{definition.Type}' is not supported.");
            }
        }


        internal IBlessing CreateBlessing(
            string contentId)
        {
            BlessingDefinition definition =
                GetBlessing(
                    contentId);

            return new DataDrivenBlessing(
                definition);
        }


        public BlessingDefinition GetBlessing(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Blessing Id cannot be empty.",
                    nameof(id));
            }

            if (_blessings.TryGetValue(
                    id,
                    out BlessingDefinition definition))
            {
                return definition;
            }

            throw new KeyNotFoundException(
                $"Blessing content not found: {id}");
        }


        public IReadOnlyCollection<BlessingDefinition>
            GetBlessings()
        {
            return _blessings.Values;
        }


        public RelicDefinition GetRelic(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "Relic Id cannot be empty.",
                    nameof(id));
            }

            if (_relics.TryGetValue(
                    id,
                    out RelicDefinition definition))
            {
                return definition;
            }

            throw new KeyNotFoundException(
                $"Relic content not found: {id}");
        }


        public IReadOnlyCollection<RelicDefinition>
            GetRelics()
        {
            return _relics.Values;
        }


        internal Relic CreateRelic(
            string contentId)
        {
            return new DataDrivenRelic(
                GetRelic(
                    contentId),
                this);
        }


        internal Relic CreateRelicFromPool(
            string poolId,
            Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            PoolDefinition pool =
                GetPool(
                    poolId);

            if (pool.Kind != PoolKind.Relic)
            {
                throw new InvalidOperationException(
                    $"Pool '{pool.Id}' has Kind '{pool.Kind}', but 'Relic' is required.");
            }

            List<PoolEntryDefinition> candidates =
                (pool.Entries ??
                    new List<PoolEntryDefinition>())
                    .Where(entry =>
                        entry != null &&
                        entry.Type == PoolEntryType.Relic &&
                        !string.IsNullOrWhiteSpace(
                            entry.ContentId) &&
                        entry.Weight > 0)
                    .GroupBy(
                        entry => entry.ContentId,
                        StringComparer.OrdinalIgnoreCase)
                    .Select(group =>
                        group.First())
                    .ToList();

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Relic pool '{pool.Id}' contains no weighted Relic entries.");
            }

            int totalWeight =
                candidates.Sum(entry =>
                    entry.Weight);

            int roll =
                random.Next(
                    totalWeight);

            int cumulativeWeight = 0;
            PoolEntryDefinition selected = candidates[0];

            foreach (PoolEntryDefinition candidate
                     in candidates)
            {
                cumulativeWeight += candidate.Weight;

                if (roll < cumulativeWeight)
                {
                    selected = candidate;
                    break;
                }
            }

            return CreateRelic(
                selected.ContentId);
        }


        internal IReadOnlyList<ItemReference>
            GetItemizableItemReferences()
        {
            List<ItemReference> references =
                new List<ItemReference>();

            foreach (EffectDefinition definition
                     in _effects.Values
                         .Where(definition =>
                             definition.CanBeItemized)
                         .OrderBy(definition =>
                             definition.Id))
            {
                references.Add(
                    new ItemReference
                    {
                        Type = ItemType.EffectRecipe,
                        ContentId = definition.Id
                    });
            }

            foreach (PassiveDefinition definition
                     in _passives.Values
                         .Where(definition =>
                             definition.CanBeItemized)
                         .OrderBy(definition =>
                             definition.Id))
            {
                references.Add(
                    new ItemReference
                    {
                        Type = ItemType.PassiveRecipe,
                        ContentId = definition.Id
                    });
            }

            foreach (AbilityDefinition definition
                     in _abilities.Values
                         .Where(definition =>
                             definition.CanBeItemized)
                         .OrderBy(definition =>
                             definition.Id))
            {
                references.Add(
                    new ItemReference
                    {
                        Type = ItemType.AbilityRecipe,
                        ContentId = definition.Id
                    });
            }

            return references;
        }


        // =========================================================
        // VALIDATION
        // =========================================================

        private void ValidateVendor(
            VendorDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(
                    definition.Name))
            {
                throw new InvalidOperationException(
                    $"Vendor '{definition.Id}' has no Name.");
            }

            if (definition.OfferCount <= 0)
            {
                throw new InvalidOperationException(
                    $"Vendor '{definition.Id}' must have OfferCount > 0.");
            }

            if (definition.Offers == null ||
                definition.Offers.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Vendor '{definition.Id}' contains no Offers.");
            }

            if (definition.OfferCount >
                definition.Offers.Count)
            {
                throw new InvalidOperationException(
                    $"Vendor '{definition.Id}' requests {definition.OfferCount} offers, " +
                    $"but only defines {definition.Offers.Count}.");
            }

            HashSet<string> references =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (VendorOfferDefinition offer
                     in definition.Offers)
            {
                if (offer == null)
                {
                    throw new InvalidOperationException(
                        $"Vendor '{definition.Id}' contains a null Offer.");
                }

                if (string.IsNullOrWhiteSpace(
                        offer.ContentId))
                {
                    throw new InvalidOperationException(
                        $"Vendor '{definition.Id}' contains an Offer without ContentId.");
                }

                if (offer.Price < 0)
                {
                    throw new InvalidOperationException(
                        $"Vendor '{definition.Id}' Offer '{offer.ContentId}' has a negative Price.");
                }

                if (offer.Weight <= 0)
                {
                    throw new InvalidOperationException(
                        $"Vendor '{definition.Id}' Offer '{offer.ContentId}' must have Weight > 0.");
                }

                string reference =
                    $"{offer.Type}:{offer.ContentId}";

                if (!references.Add(reference))
                {
                    throw new InvalidOperationException(
                        $"Vendor '{definition.Id}' contains duplicate Offer '{reference}'.");
                }

                ValidateVendorOfferReference(
                    definition,
                    offer);
            }
        }


        private void ValidateVendorOfferReference(
            VendorDefinition vendor,
            VendorOfferDefinition offer)
        {
            switch (offer.Type)
            {
                case VendorOfferType.EffectRecipe:
                    {
                        EffectDefinition definition =
                            GetEffect(
                                offer.ContentId);

                        if (!definition.CanBeItemized)
                        {
                            throw new InvalidOperationException(
                                $"Vendor '{vendor.Id}' references Effect '{definition.Id}', " +
                                "but it cannot be itemized.");
                        }

                        break;
                    }

                case VendorOfferType.PassiveRecipe:
                    {
                        PassiveDefinition definition =
                            GetPassive(
                                offer.ContentId);

                        if (!definition.CanBeItemized)
                        {
                            throw new InvalidOperationException(
                                $"Vendor '{vendor.Id}' references Passive '{definition.Id}', " +
                                "but it cannot be itemized.");
                        }

                        break;
                    }

                case VendorOfferType.AbilityRecipe:
                    {
                        AbilityDefinition definition =
                            GetAbility(
                                offer.ContentId);

                        if (!definition.CanBeItemized)
                        {
                            throw new InvalidOperationException(
                                $"Vendor '{vendor.Id}' references Ability '{definition.Id}', " +
                                "but it cannot be itemized.");
                        }

                        break;
                    }

                case VendorOfferType.Blessing:
                    CreateBlessing(
                        offer.ContentId);
                    break;

                case VendorOfferType.Relic:
                    CreateRelic(
                        offer.ContentId);
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Vendor '{vendor.Id}' uses unsupported Offer Type '{offer.Type}'.");
            }
        }


        private void ValidateEffect(
            EffectDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.Name))
            {
                throw new InvalidOperationException(
                    $"Effect '{definition.Id}' has no Name.");
            }

            if (string.IsNullOrWhiteSpace(definition.Description))
            {
                throw new InvalidOperationException(
                    $"Effect '{definition.Id}' has no Description.");
            }

            if (definition.Components == null ||
                definition.Components.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Effect '{definition.Id}' contains no Components.");
            }

            foreach (EffectComponentDefinition component
                     in definition.Components)
            {
                if (component == null)
                {
                    throw new InvalidOperationException(
                        $"Effect '{definition.Id}' contains a null Component.");
                }

                switch (component.Action)
                {
                    case EffectActionType.Damage:
                    case EffectActionType.Heal:
                        if (component.Amount <= 0)
                        {
                            throw new InvalidOperationException(
                                $"Effect '{definition.Id}' uses '{component.Action}' with Amount <= 0.");
                        }
                        break;
                }
            }

            if (definition.CanBeItemized &&
                definition.Itemization == null)
            {
                throw new InvalidOperationException(
                    $"Itemizable Effect '{definition.Id}' must define Itemization metadata with a Target.");
            }
        }


        private void ValidatePassive(
            PassiveDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.Name))
            {
                throw new InvalidOperationException(
                    $"Passive '{definition.Id}' has no Name.");
            }

            bool hasModifiers =
                definition.Modifiers != null &&
                definition.Modifiers.Count > 0;

            bool hasEffects =
                definition.Effects != null &&
                definition.Effects.Count > 0;

            if (!hasModifiers && !hasEffects)
            {
                throw new InvalidOperationException(
                    $"Passive '{definition.Id}' must contain at least one Modifier or triggered Effect.");
            }

            if (definition.Modifiers != null)
            {
                foreach (PassiveModifierDefinition modifier
                         in definition.Modifiers)
                {
                    if (modifier == null)
                    {
                        throw new InvalidOperationException(
                            $"Passive '{definition.Id}' contains a null Modifier.");
                    }

                    if (modifier.Amount <= 0)
                    {
                        throw new InvalidOperationException(
                            $"Passive '{definition.Id}' modifier Amount must be greater than 0.");
                    }
                }
            }

            if (definition.Effects != null)
            {
                foreach (TriggeredEffectDefinition effect
                         in definition.Effects)
                {
                    ValidateTriggeredEffectBinding(
                        $"Passive '{definition.Id}'",
                        effect,
                        allowSelf: true);
                }
            }
        }


        private void ValidateAbility(
            AbilityDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.Name))
            {
                throw new InvalidOperationException(
                    $"Ability '{definition.Id}' has no Name.");
            }

            if (string.IsNullOrWhiteSpace(definition.Description))
            {
                throw new InvalidOperationException(
                    $"Ability '{definition.Id}' has no Description.");
            }

            if (definition.CooldownRounds < 0)
            {
                throw new InvalidOperationException(
                    $"Ability '{definition.Id}' cannot have a negative CooldownRounds value.");
            }

            if (definition.Effects == null ||
                definition.Effects.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Ability '{definition.Id}' contains no Effects.");
            }

            foreach (ActionEffectDefinition effect
                     in definition.Effects)
            {
                ValidateActionEffectBinding(
                    $"Ability '{definition.Id}'",
                    effect);
            }
        }


        private void ValidateEnemy(
            EnemyDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.Name))
            {
                throw new InvalidOperationException(
                    $"Enemy '{definition.Id}' has no Name.");
            }

            if (definition.BaseHealth <= 0)
            {
                throw new InvalidOperationException(
                    $"Enemy '{definition.Id}' must have BaseHealth > 0.");
            }

            if (definition.PassiveIds != null)
            {
                foreach (string passiveId in definition.PassiveIds)
                {
                    GetPassive(passiveId);
                }
            }

            if (definition.Actions != null)
            {
                HashSet<string> actionIds =
                    new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);

                foreach (EnemyActionDefinition action
                         in definition.Actions)
                {
                    if (action == null ||
                        string.IsNullOrWhiteSpace(action.Id))
                    {
                        throw new InvalidOperationException(
                            $"Enemy '{definition.Id}' contains an invalid EnemyAction.");
                    }

                    if (!actionIds.Add(action.Id))
                    {
                        throw new InvalidOperationException(
                            $"Enemy '{definition.Id}' contains duplicate EnemyAction Id '{action.Id}'.");
                    }

                    if (string.IsNullOrWhiteSpace(action.Name) ||
                        string.IsNullOrWhiteSpace(action.Description))
                    {
                        throw new InvalidOperationException(
                            $"EnemyAction '{action.Id}' on Enemy '{definition.Id}' requires Name and Description.");
                    }

                    if (action.DelayTurns < 0)
                    {
                        throw new InvalidOperationException(
                            $"EnemyAction '{action.Id}' cannot have negative DelayTurns.");
                    }

                    if (action.Targeting == EnemyActionTargeting.RingPositions)
                    {
                        if (action.Positions == null ||
                            action.Positions.Count == 0)
                        {
                            throw new InvalidOperationException(
                                $"EnemyAction '{action.Id}' uses RingPositions but defines no Positions.");
                        }
                    }
                    else if (action.Positions != null &&
                             action.Positions.Count > 0)
                    {
                        throw new InvalidOperationException(
                            $"EnemyAction '{action.Id}' defines Positions but uses Targeting '{action.Targeting}'.");
                    }

                    if (action.Effects == null ||
                        action.Effects.Count == 0)
                    {
                        throw new InvalidOperationException(
                            $"EnemyAction '{action.Id}' contains no Effects.");
                    }

                    foreach (ActionEffectDefinition effect
                             in action.Effects)
                    {
                        ValidateActionEffectBinding(
                            $"EnemyAction '{action.Id}'",
                            effect);
                    }
                }
            }

            if (definition.Sectors != null)
            {
                foreach (EnemySectorDefinition sector
                         in definition.Sectors)
                {
                    if (sector?.Effects == null)
                        continue;

                    foreach (ActionEffectDefinition effect
                             in sector.Effects)
                    {
                        ValidateActionEffectBinding(
                            $"Sector '{sector.Name}' on Enemy '{definition.Id}'",
                            effect);
                    }
                }
            }

            if (definition.Loot != null)
            {
                foreach (EnemyLootDefinition loot
                         in definition.Loot)
                {
                    if (loot == null)
                    {
                        throw new InvalidOperationException(
                            $"Enemy '{definition.Id}' contains a null Loot entry.");
                    }

                    if (loot.Weight <= 0)
                    {
                        throw new InvalidOperationException(
                            $"Enemy '{definition.Id}' Loot '{loot.ContentId}' must have Weight > 0.");
                    }

                    ValidateRewardDefinition(
                        loot,
                        $"Enemy '{definition.Id}' Loot");
                }
            }
        }


        private void ValidateUnit(
            UnitDefinition definition,
            HashSet<string> registeredUnitActionIds)
        {
            Guard.NotNull(
                registeredUnitActionIds,
                nameof(registeredUnitActionIds));

            if (string.IsNullOrWhiteSpace(definition.Name))
            {
                throw new InvalidOperationException(
                    $"Unit '{definition.Id}' has no Name.");
            }

            if (definition.BaseHealth <= 0)
            {
                throw new InvalidOperationException(
                    $"Unit '{definition.Id}' must have BaseHealth > 0.");
            }

            if (definition.EffectSlotCapacity < 0)
            {
                throw new InvalidOperationException(
                    $"Unit '{definition.Id}' cannot have a negative EffectSlotCapacity.");
            }

            if (definition.AbilitySlotCapacity < 0)
            {
                throw new InvalidOperationException(
                    $"Unit '{definition.Id}' cannot have a negative AbilitySlotCapacity.");
            }

            if (definition.Actions == null ||
                definition.Actions.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Unit '{definition.Id}' must contain at least one UnitAction.");
            }

            HashSet<string> actionIds =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (UnitActionDefinition action
                     in definition.Actions)
            {
                if (action == null ||
                    string.IsNullOrWhiteSpace(action.Id))
                {
                    throw new InvalidOperationException(
                        $"Unit '{definition.Id}' contains an invalid UnitAction.");
                }

                if (!actionIds.Add(action.Id))
                {
                    throw new InvalidOperationException(
                        $"Unit '{definition.Id}' contains duplicate UnitAction Id '{action.Id}'.");
                }

                if (!registeredUnitActionIds.Add(action.Id))
                {
                    throw new InvalidOperationException(
                        $"UnitAction Id '{action.Id}' is used by more than one Unit. " +
                        "UnitAction Ids must be globally unique so presentation/FX can map them reliably.");
                }

                if (string.IsNullOrWhiteSpace(action.Name) ||
                    string.IsNullOrWhiteSpace(action.Description))
                {
                    throw new InvalidOperationException(
                        $"UnitAction '{action.Id}' on Unit '{definition.Id}' requires Name and Description.");
                }

                if (action.Effects == null ||
                    action.Effects.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"UnitAction '{action.Id}' on Unit '{definition.Id}' contains no Effects.");
                }

                foreach (ActionEffectDefinition effect
                         in action.Effects)
                {
                    ValidateActionEffectBinding(
                        $"UnitAction '{action.Id}' on Unit '{definition.Id}'",
                        effect);
                }
            }

            if (definition.PassiveIds != null)
            {
                foreach (string passiveId in definition.PassiveIds)
                {
                    GetPassive(passiveId);
                }
            }

            if (definition.AbilityIds != null)
            {
                foreach (string abilityId in definition.AbilityIds)
                {
                    GetAbility(abilityId);
                }
            }
        }


        private void ValidateRelic(
            RelicDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.Name))
            {
                throw new InvalidOperationException(
                    $"Relic '{definition.Id}' has no Name.");
            }

            bool hasModifiers =
                definition.Modifiers != null &&
                definition.Modifiers.Count > 0;

            bool hasEffects =
                definition.Effects != null &&
                definition.Effects.Count > 0;

            if (!hasModifiers && !hasEffects)
            {
                throw new InvalidOperationException(
                    $"Relic '{definition.Id}' must contain at least one Modifier or triggered Effect.");
            }

            if (definition.Modifiers != null)
            {
                foreach (RelicModifierDefinition modifier
                         in definition.Modifiers)
                {
                    if (modifier == null)
                    {
                        throw new InvalidOperationException(
                            $"Relic '{definition.Id}' contains a null Modifier.");
                    }

                    if (modifier.Amount <= 0)
                    {
                        throw new InvalidOperationException(
                            $"Relic '{definition.Id}' modifier Amount must be greater than 0.");
                    }
                }
            }

            if (definition.Effects != null)
            {
                foreach (TriggeredEffectDefinition effect
                         in definition.Effects)
                {
                    ValidateTriggeredEffectBinding(
                        $"Relic '{definition.Id}'",
                        effect,
                        allowSelf: false);

                    switch (effect.Trigger)
                    {
                        case EffectTrigger.SectorActivated:
                        case EffectTrigger.RingObjectActivated:
                        case EffectTrigger.RingRotated:
                        case EffectTrigger.UnitActionActivated:
                        case EffectTrigger.ActiveAbilityActivated:
                        case EffectTrigger.EnemyActionActivated:
                        case EffectTrigger.TurnEnded:
                            break;

                        default:
                            throw new InvalidOperationException(
                                $"Relic '{definition.Id}' uses Trigger '{effect.Trigger}', which is not wired for Relics yet.");
                    }
                }
            }
        }


        private void ValidateActionEffectBinding(
            string owner,
            ActionEffectDefinition binding)
        {
            if (binding == null ||
                string.IsNullOrWhiteSpace(binding.EffectId))
            {
                throw new InvalidOperationException(
                    $"{owner} contains an invalid ActionEffect.");
            }

            EffectDefinition effect =
                GetEffect(
                    binding.EffectId);

            if (effect.Components == null ||
                effect.Components.Count == 0)
            {
                throw new InvalidOperationException(
                    $"{owner} references Effect '{binding.EffectId}' without Components.");
            }
        }


        private void ValidateTriggeredEffectBinding(
            string owner,
            TriggeredEffectDefinition binding,
            bool allowSelf)
        {
            if (binding == null ||
                string.IsNullOrWhiteSpace(binding.EffectId))
            {
                throw new InvalidOperationException(
                    $"{owner} contains an invalid TriggeredEffect.");
            }

            GetEffect(
                binding.EffectId);

            if (!allowSelf &&
                binding.Target == EffectTarget.Self)
            {
                throw new InvalidOperationException(
                    $"{owner} cannot use Target Self because its owner is not a combat target.");
            }
        }


        private static void ValidateBlessing(
            BlessingDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(
                    definition.Name))
            {
                throw new InvalidOperationException(
                    $"Blessing '{definition.Id}' has no Name.");
            }

            if (definition.Effects == null ||
                definition.Effects.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Blessing '{definition.Id}' must contain at least one Effect.");
            }

            for (int index = 0;
                 index < definition.Effects.Count;
                 index++)
            {
                BlessingEffectDefinition effect =
                    definition.Effects[index];

                if (effect == null)
                {
                    throw new InvalidOperationException(
                        $"Blessing '{definition.Id}' contains a null Effect.");
                }

                switch (effect.Type)
                {
                    case BlessingEffectType.GainCurrency:
                        if (effect.Amount <= 0)
                        {
                            throw new InvalidOperationException(
                                $"Blessing '{definition.Id}' GainCurrency amount must be greater than 0.");
                        }
                        break;

                    case BlessingEffectType.ModifySectorValue:
                        if (!effect.Sector.HasValue)
                        {
                            throw new InvalidOperationException(
                                $"Blessing '{definition.Id}' ModifySectorValue requires a Sector.");
                        }

                        if (effect.Amount == 0)
                        {
                            throw new InvalidOperationException(
                                $"Blessing '{definition.Id}' ModifySectorValue amount cannot be 0.");
                        }
                        break;

                    default:
                        throw new InvalidOperationException(
                            $"Blessing '{definition.Id}' uses unsupported Effect Type '{effect.Type}'.");
                }
            }
        }


        private void ValidateEvent(
            EventDefinition definition)
        {
            if (string.IsNullOrWhiteSpace(definition.Name))
            {
                throw new InvalidOperationException(
                    $"Event '{definition.Id}' has no Name.");
            }

            if (definition.Nodes == null || definition.Nodes.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Event '{definition.Id}' has no Nodes.");
            }

            Dictionary<string, EventNodeDefinition> nodes =
                new Dictionary<string, EventNodeDefinition>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (EventNodeDefinition node in definition.Nodes)
            {
                if (node == null || string.IsNullOrWhiteSpace(node.Id))
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' contains a Node without Id.");
                }

                if (!nodes.TryAdd(node.Id, node))
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' contains duplicate Node Id '{node.Id}'.");
                }
            }

            if (string.IsNullOrWhiteSpace(definition.StartNodeId) ||
                !nodes.ContainsKey(definition.StartNodeId))
            {
                throw new InvalidOperationException(
                    $"Event '{definition.Id}' has invalid StartNodeId '{definition.StartNodeId}'.");
            }

            foreach (EventNodeDefinition node in definition.Nodes)
            {
                switch (node.Type)
                {
                    case EventNodeType.Choice:
                        ValidateChoiceNode(definition, node, nodes);
                        break;

                    case EventNodeType.Activity:
                        ValidateActivityNode(definition, node, nodes);
                        break;

                    case EventNodeType.Automatic:
                        ValidateEventOutcomes(definition, node.Id, node.Outcomes, false);
                        ValidateNextEventNode(definition, node.Id, node.NextNodeId, nodes);
                        break;

                    case EventNodeType.Presentation:
                        ValidateEventOutcomes(definition, node.Id, node.Outcomes, false);
                        ValidateNextEventNode(definition, node.Id, node.NextNodeId, nodes);
                        break;

                    case EventNodeType.End:
                        break;

                    default:
                        throw new InvalidOperationException(
                            $"Event '{definition.Id}' Node '{node.Id}' has unsupported Type '{node.Type}'.");
                }
            }
        }

        private void ValidateChoiceNode(
            EventDefinition definition,
            EventNodeDefinition node,
            IReadOnlyDictionary<string, EventNodeDefinition> nodes)
        {
            if (node.Options == null || node.Options.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Event '{definition.Id}' Choice Node '{node.Id}' has no Options.");
            }

            HashSet<string> optionIds =
                new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (EventOptionDefinition option in node.Options)
            {
                if (option == null || string.IsNullOrWhiteSpace(option.Id))
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' Choice Node '{node.Id}' contains an Option without Id.");
                }

                if (!optionIds.Add(option.Id))
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' Choice Node '{node.Id}' contains duplicate Option Id '{option.Id}'.");
                }

                if (string.IsNullOrWhiteSpace(option.Name))
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' Option '{option.Id}' has no Name.");
                }

                ValidateEventRequirements(definition, option.Id, option.Requirements);
                ValidateEventOutcomes(definition, option.Id, option.Costs, true);
                ValidateEventOutcomes(definition, option.Id, option.Outcomes, false);
                ValidateNextEventNode(definition, option.Id, option.NextNodeId, nodes);
            }
        }

        private void ValidateActivityNode(
            EventDefinition definition,
            EventNodeDefinition node,
            IReadOnlyDictionary<string, EventNodeDefinition> nodes)
        {
            if (string.IsNullOrWhiteSpace(node.ActivityId))
            {
                throw new InvalidOperationException(
                    $"Event '{definition.Id}' Activity Node '{node.Id}' has no ActivityId.");
            }

            if (node.ActivityResults == null || node.ActivityResults.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Event '{definition.Id}' Activity Node '{node.Id}' has no ActivityResults.");
            }

            HashSet<string> resultIds =
                new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (EventActivityResultDefinition result in node.ActivityResults)
            {
                if (result == null || string.IsNullOrWhiteSpace(result.ResultId))
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' Activity Node '{node.Id}' contains a Result without ResultId.");
                }

                if (!resultIds.Add(result.ResultId))
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' Activity Node '{node.Id}' contains duplicate ResultId '{result.ResultId}'.");
                }

                ValidateEventOutcomes(definition, result.ResultId, result.Outcomes, false);
                ValidateNextEventNode(definition, result.ResultId, result.NextNodeId, nodes);
            }
        }

        private void ValidateEventRequirements(
            EventDefinition definition,
            string ownerId,
            IReadOnlyList<EventRequirementDefinition> requirements)
        {
            if (requirements == null)
                return;

            foreach (EventRequirementDefinition requirement in requirements)
            {
                if (requirement == null)
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' '{ownerId}' contains a null Requirement.");
                }

                if (requirement.Type == EventRequirementType.MinimumCurrency &&
                    requirement.Amount < 0)
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' '{ownerId}' has a negative currency requirement.");
                }

                if (requirement.Type == EventRequirementType.HasUnitTag &&
                    !requirement.UnitTag.HasValue)
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' '{ownerId}' requires HasUnitTag but has no UnitTag.");
                }
            }
        }

        private void ValidateEventOutcomes(
            EventDefinition definition,
            string ownerId,
            IReadOnlyList<EventOutcomeDefinition> outcomes,
            bool isCost)
        {
            if (outcomes == null)
                return;

            foreach (EventOutcomeDefinition outcome in outcomes)
            {
                if (outcome == null)
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' '{ownerId}' contains a null Outcome.");
                }

                if (isCost &&
                    outcome.Type != EventOutcomeType.LoseCurrency &&
                    outcome.Type != EventOutcomeType.DamageFirstLivingUnit &&
                    outcome.Type != EventOutcomeType.DamageAllLivingUnits)
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' '{ownerId}' uses '{outcome.Type}' as a Cost, " +
                        "but only LoseCurrency and Unit damage outcomes are valid costs.");
                }

                if (outcome.Type != EventOutcomeType.GainItem &&
                    outcome.Type != EventOutcomeType.GainReward &&
                    outcome.Amount <= 0)
                {
                    throw new InvalidOperationException(
                        $"Event '{definition.Id}' '{ownerId}' Outcome '{outcome.Type}' requires Amount > 0.");
                }

                if (outcome.Type == EventOutcomeType.GainItem)
                {
                    if (outcome.Item == null)
                    {
                        throw new InvalidOperationException(
                            $"Event '{definition.Id}' '{ownerId}' GainItem Outcome has no Item reference.");
                    }

                    CreateItem(outcome.Item);
                }

                if (outcome.Type == EventOutcomeType.GainReward)
                {
                    if (outcome.Reward == null)
                    {
                        throw new InvalidOperationException(
                            $"Event '{definition.Id}' '{ownerId}' GainReward Outcome has no Reward definition.");
                    }

                    ValidateRewardDefinition(
                        outcome.Reward,
                        $"Event '{definition.Id}' '{ownerId}' GainReward");
                }
            }
        }

        private void ValidateRewardDefinition(
            RewardDefinition reward,
            string owner)
        {
            Guard.NotNull(
                reward,
                nameof(reward));

            if (string.IsNullOrWhiteSpace(
                    reward.ContentId))
            {
                throw new InvalidOperationException(
                    $"{owner} has an empty Reward ContentId.");
            }

            switch (reward.Type)
            {
                case RewardContentType.EffectRecipe:
                    {
                        EffectDefinition definition =
                            GetEffect(
                                reward.ContentId);

                        if (!definition.CanBeItemized)
                        {
                            throw new InvalidOperationException(
                                $"{owner} references Effect '{definition.Id}', but it cannot be itemized.");
                        }

                        return;
                    }

                case RewardContentType.PassiveRecipe:
                    {
                        PassiveDefinition definition =
                            GetPassive(
                                reward.ContentId);

                        if (!definition.CanBeItemized)
                        {
                            throw new InvalidOperationException(
                                $"{owner} references Passive '{definition.Id}', but it cannot be itemized.");
                        }

                        return;
                    }

                case RewardContentType.AbilityRecipe:
                    {
                        AbilityDefinition definition =
                            GetAbility(
                                reward.ContentId);

                        if (!definition.CanBeItemized)
                        {
                            throw new InvalidOperationException(
                                $"{owner} references Ability '{definition.Id}', but it cannot be itemized.");
                        }

                        return;
                    }

                case RewardContentType.Unit:
                    {
                        UnitDefinition definition =
                            GetUnit(
                                reward.ContentId);

                        if (!definition.CanBeRewarded)
                        {
                            throw new InvalidOperationException(
                                $"{owner} references Unit '{definition.Id}', but it cannot be rewarded.");
                        }

                        return;
                    }

                case RewardContentType.Relic:
                    {
                        RelicDefinition definition =
                            GetRelic(
                                reward.ContentId);

                        if (!definition.CanBeRewarded)
                        {
                            throw new InvalidOperationException(
                                $"{owner} references Relic '{definition.Id}', but it cannot be rewarded.");
                        }

                        return;
                    }

                case RewardContentType.Blessing:
                    {
                        BlessingDefinition definition =
                            GetBlessing(
                                reward.ContentId);

                        if (!definition.CanBeRewarded)
                        {
                            throw new InvalidOperationException(
                                $"{owner} references Blessing '{definition.Id}', but it cannot be rewarded.");
                        }

                        return;
                    }

                default:
                    throw new InvalidOperationException(
                        $"{owner} uses unsupported Reward Type '{reward.Type}'.");
            }
        }


        private static void ValidateNextEventNode(
            EventDefinition definition,
            string ownerId,
            string nextNodeId,
            IReadOnlyDictionary<string, EventNodeDefinition> nodes)
        {
            if (string.IsNullOrWhiteSpace(nextNodeId) ||
                !nodes.ContainsKey(nextNodeId))
            {
                throw new InvalidOperationException(
                    $"Event '{definition.Id}' '{ownerId}' references invalid NextNodeId '{nextNodeId}'.");
            }
        }

        private void ValidateRunPlans()
        {
            foreach (RunPlanDefinition runPlan in _runPlans.Values)
            {
                if (runPlan.StartingUnitSelection == null)
                {
                    throw new InvalidOperationException(
                        $"Run Plan '{runPlan.Id}' has no StartingUnitSelection.");
                }

                if (runPlan.StartingUnitSelection.ChoiceCount <= 0)
                {
                    throw new InvalidOperationException(
                        $"Run Plan '{runPlan.Id}' has an invalid Starting Unit choice count.");
                }

                PoolDefinition startingUnitPool =
                    GetPool(
                        runPlan.StartingUnitSelection.PoolId);

                if (startingUnitPool.Kind != PoolKind.Unit)
                {
                    throw new InvalidOperationException(
                        $"Starting Unit pool '{startingUnitPool.Id}' has Kind " +
                        $"'{startingUnitPool.Kind}', but 'Unit' is required.");
                }

                bool hasUnit =
                    startingUnitPool.Entries != null &&
                    startingUnitPool.Entries.Any(entry =>
                        entry != null &&
                        entry.Type == PoolEntryType.Unit &&
                        entry.Weight > 0);

                if (!hasUnit)
                {
                    throw new InvalidOperationException(
                        $"Starting Unit pool '{startingUnitPool.Id}' contains no weighted Unit entries.");
                }

                if (runPlan.Stages == null ||
                    runPlan.Stages.Count == 0)
                {
                    throw new InvalidOperationException(
                        $"Run Plan '{runPlan.Id}' must contain at least one Stage.");
                }

                for (int stageIndex = 0;
                     stageIndex < runPlan.Stages.Count;
                     stageIndex++)
                {
                    RunStageDefinition stage =
                        runPlan.Stages[stageIndex];

                    if (stage == null)
                    {
                        throw new InvalidOperationException(
                            $"Run Plan '{runPlan.Id}' contains a null Stage at index {stageIndex}.");
                    }

                    ValidateStageEnemyPool(
                        runPlan,
                        stageIndex,
                        stage.EnemyPoolId,
                        EnemyType.Normal);

                    ValidateStageEnemyPool(
                        runPlan,
                        stageIndex,
                        stage.BossPoolId,
                        EnemyType.Boss);

                    if (stage.FloorTypes == null ||
                        stage.FloorTypes.Count == 0)
                    {
                        throw new InvalidOperationException(
                            $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} has no Floors.");
                    }


                    if (stage.FloorTypes.Last() !=
                        FloorType.Boss)
                    {
                        throw new InvalidOperationException(
                            $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} " +
                            "must end with a Boss floor.");
                    }

                    if (stage.FloorTypes.Any(
                            floorType =>
                                floorType == FloorType.Elite ||
                                floorType == FloorType.Rest))
                    {
                        throw new InvalidOperationException(
                            $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} " +
                            "contains a Floor type that is not implemented yet.");
                    }

                    if (stage.FloorTypes.Contains(FloorType.Event))
                    {
                        ValidateStageEventPool(
                            runPlan,
                            stageIndex,
                            stage.EventPoolId);
                    }

                    if (stage.FloorTypes.Contains(FloorType.Shop))
                    {
                        ValidateStageVendorPool(
                            runPlan,
                            stageIndex,
                            stage.VendorPoolId);
                    }
                }
            }

            int defaultCount =
                _runPlans.Values.Count(plan => plan.IsDefault);

            if (defaultCount != 1)
            {
                throw new InvalidOperationException(
                    $"Exactly one default Run Plan is required. Found: {defaultCount}.");
            }
        }


        private void ValidateStageEnemyPool(
            RunPlanDefinition runPlan,
            int stageIndex,
            string poolId,
            EnemyType enemyType)
        {
            if (string.IsNullOrWhiteSpace(
                    poolId))
            {
                throw new InvalidOperationException(
                    $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} " +
                    $"has no {enemyType} Enemy pool.");
            }

            PoolDefinition pool =
                GetPool(
                    poolId);

            PoolKind expectedKind =
                enemyType == EnemyType.Boss
                    ? PoolKind.BossEnemy
                    : PoolKind.NormalEnemy;

            if (pool.Kind != expectedKind)
            {
                throw new InvalidOperationException(
                    $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} uses pool " +
                    $"'{pool.Id}' as a {enemyType} Enemy pool, but its Kind is " +
                    $"'{pool.Kind}' instead of '{expectedKind}'.");
            }

            List<PoolEntryDefinition> weightedEnemyEntries =
                (pool.Entries ??
                    new List<PoolEntryDefinition>())
                    .Where(entry =>
                        entry != null &&
                        entry.Type == PoolEntryType.Enemy &&
                        entry.Weight > 0)
                    .ToList();

            if (weightedEnemyEntries.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Stage pool '{pool.Id}' contains no weighted Enemy entries.");
            }

            foreach (PoolEntryDefinition entry
                     in weightedEnemyEntries)
            {
                EnemyDefinition enemy =
                    GetEnemy(
                        entry.ContentId);

                if (enemy.Type !=
                    enemyType)
                {
                    throw new InvalidOperationException(
                        $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} uses pool " +
                        $"'{pool.Id}' as a {enemyType} pool, but Enemy " +
                        $"'{enemy.Id}' is '{enemy.Type}'.");
                }
            }
        }


        private void ValidateStageEventPool(
            RunPlanDefinition runPlan,
            int stageIndex,
            string poolId)
        {
            if (string.IsNullOrWhiteSpace(poolId))
            {
                throw new InvalidOperationException(
                    $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} has an Event floor but no Event pool.");
            }

            PoolDefinition pool = GetPool(poolId);

            if (pool.Kind != PoolKind.Event)
            {
                throw new InvalidOperationException(
                    $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} uses pool " +
                    $"'{pool.Id}' as an Event pool, but its Kind is '{pool.Kind}'.");
            }

            List<PoolEntryDefinition> weightedEventEntries =
                (pool.Entries ?? new List<PoolEntryDefinition>())
                    .Where(entry =>
                        entry != null &&
                        entry.Type == PoolEntryType.Event &&
                        entry.Weight > 0)
                    .ToList();

            if (weightedEventEntries.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Event pool '{pool.Id}' contains no weighted Event entries.");
            }

            foreach (PoolEntryDefinition entry in weightedEventEntries)
            {
                GetEvent(entry.ContentId);
            }
        }

        private void ValidateStageVendorPool(
            RunPlanDefinition runPlan,
            int stageIndex,
            string poolId)
        {
            if (string.IsNullOrWhiteSpace(
                    poolId))
            {
                throw new InvalidOperationException(
                    $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} has a Shop floor but no Vendor pool.");
            }

            PoolDefinition pool =
                GetPool(
                    poolId);

            if (pool.Kind != PoolKind.Vendor)
            {
                throw new InvalidOperationException(
                    $"Run Plan '{runPlan.Id}' Stage {stageIndex + 1} uses pool " +
                    $"'{pool.Id}' as a Vendor pool, but its Kind is '{pool.Kind}'.");
            }

            List<PoolEntryDefinition> weightedVendorEntries =
                (pool.Entries ??
                    new List<PoolEntryDefinition>())
                    .Where(entry =>
                        entry != null &&
                        entry.Type == PoolEntryType.Vendor &&
                        entry.Weight > 0)
                    .ToList();

            if (weightedVendorEntries.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Vendor pool '{pool.Id}' contains no weighted Vendor entries.");
            }

            foreach (PoolEntryDefinition entry
                     in weightedVendorEntries)
            {
                GetVendor(
                    entry.ContentId);
            }
        }


        private void NormalizeLegacyPoolKind(
            PoolDefinition pool)
        {
            if (pool == null ||
                pool.Kind != PoolKind.Unspecified)
            {
                return;
            }

            pool.Kind =
                InferLegacyPoolKind(pool);
        }


        private PoolKind InferLegacyPoolKind(
            PoolDefinition pool)
        {
            List<PoolEntryDefinition> entries =
                (pool?.Entries ??
                    new List<PoolEntryDefinition>())
                    .Where(entry => entry != null)
                    .ToList();

            if (entries.Count == 0)
                return PoolKind.Unspecified;

            PoolEntryType entryType =
                entries[0].Type;

            if (entries.Any(entry =>
                    entry.Type != entryType))
            {
                return PoolKind.Unspecified;
            }

            switch (entryType)
            {
                case PoolEntryType.Unit:
                    return PoolKind.Unit;

                case PoolEntryType.Enemy:
                    EnemyType? inferredEnemyType = null;

                    foreach (PoolEntryDefinition entry
                             in entries)
                    {
                        if (string.IsNullOrWhiteSpace(
                                entry.ContentId) ||
                            !_enemies.TryGetValue(
                                entry.ContentId,
                                out EnemyDefinition enemy))
                        {
                            return PoolKind.Unspecified;
                        }

                        if (inferredEnemyType == null)
                        {
                            inferredEnemyType =
                                enemy.Type;
                        }
                        else if (inferredEnemyType.Value !=
                                 enemy.Type)
                        {
                            return PoolKind.Unspecified;
                        }
                    }

                    return inferredEnemyType == EnemyType.Boss
                        ? PoolKind.BossEnemy
                        : PoolKind.NormalEnemy;

                case PoolEntryType.Event:
                    return PoolKind.Event;

                case PoolEntryType.Vendor:
                    return PoolKind.Vendor;

                case PoolEntryType.Relic:
                    return PoolKind.Relic;

                case PoolEntryType.EffectRecipe:
                    return PoolKind.EffectRecipe;

                case PoolEntryType.PassiveRecipe:
                    return PoolKind.PassiveRecipe;

                case PoolEntryType.AbilityRecipe:
                    return PoolKind.AbilityRecipe;

                case PoolEntryType.StagePlan:
                    return PoolKind.StagePlan;

                case PoolEntryType.RunPlan:
                    return PoolKind.RunPlan;

                default:
                    return PoolKind.Unspecified;
            }
        }


        private static PoolEntryType GetExpectedEntryType(
            PoolKind kind)
        {
            switch (kind)
            {
                case PoolKind.Unit:
                    return PoolEntryType.Unit;

                case PoolKind.NormalEnemy:
                case PoolKind.BossEnemy:
                    return PoolEntryType.Enemy;

                case PoolKind.Event:
                    return PoolEntryType.Event;

                case PoolKind.Vendor:
                    return PoolEntryType.Vendor;

                case PoolKind.Relic:
                    return PoolEntryType.Relic;

                case PoolKind.EffectRecipe:
                    return PoolEntryType.EffectRecipe;

                case PoolKind.PassiveRecipe:
                    return PoolEntryType.PassiveRecipe;

                case PoolKind.AbilityRecipe:
                    return PoolEntryType.AbilityRecipe;

                case PoolKind.StagePlan:
                    return PoolEntryType.StagePlan;

                case PoolKind.RunPlan:
                    return PoolEntryType.RunPlan;

                default:
                    throw new InvalidOperationException(
                        $"Pool Kind '{kind}' is not valid.");
            }
        }


        private void ValidatePool(
            PoolDefinition pool)
        {
            if (pool.Kind == PoolKind.Unspecified)
            {
                throw new InvalidOperationException(
                    $"Pool '{pool.Id}' has no explicit Kind and its legacy Kind could not be inferred.");
            }

            if (pool.Entries == null || pool.Entries.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Pool '{pool.Id}' contains no entries.");
            }

            PoolEntryType expectedEntryType =
                GetExpectedEntryType(
                    pool.Kind);

            HashSet<string> references = new HashSet<string>();

            foreach (PoolEntryDefinition entry in pool.Entries)
            {
                if (entry == null)
                {
                    throw new InvalidOperationException(
                        $"Pool '{pool.Id}' contains a null entry.");
                }

                if (entry.Type != expectedEntryType)
                {
                    throw new InvalidOperationException(
                        $"Pool '{pool.Id}' has Kind '{pool.Kind}', but entry " +
                        $"'{entry.ContentId}' has Type '{entry.Type}' instead of " +
                        $"'{expectedEntryType}'.");
                }

                if (string.IsNullOrWhiteSpace(entry.ContentId))
                {
                    throw new InvalidOperationException(
                        $"Pool '{pool.Id}' contains an entry without ContentId.");
                }

                if (entry.Weight <= 0)
                {
                    throw new InvalidOperationException(
                        $"Pool '{pool.Id}' entry '{entry.ContentId}' must have Weight > 0.");
                }

                string referenceKey = $"{entry.Type}:{entry.ContentId}";
                if (!references.Add(referenceKey))
                {
                    throw new InvalidOperationException(
                        $"Pool '{pool.Id}' contains duplicate entry '{referenceKey}'.");
                }

                if (entry.Type == PoolEntryType.Unit)
                    GetUnit(entry.ContentId);

                if (entry.Type == PoolEntryType.Enemy)
                {
                    EnemyDefinition enemy =
                        GetEnemy(entry.ContentId);

                    if (pool.Kind == PoolKind.NormalEnemy &&
                        enemy.Type != EnemyType.Normal)
                    {
                        throw new InvalidOperationException(
                            $"Pool '{pool.Id}' is a NormalEnemy pool, but Enemy " +
                            $"'{enemy.Id}' is '{enemy.Type}'.");
                    }

                    if (pool.Kind == PoolKind.BossEnemy &&
                        enemy.Type != EnemyType.Boss)
                    {
                        throw new InvalidOperationException(
                            $"Pool '{pool.Id}' is a BossEnemy pool, but Enemy " +
                            $"'{enemy.Id}' is '{enemy.Type}'.");
                    }
                }

                if (entry.Type == PoolEntryType.Event)
                {
                    GetEvent(entry.ContentId);
                }

                if (entry.Type == PoolEntryType.Vendor)
                {
                    GetVendor(entry.ContentId);
                }

                if (entry.Type == PoolEntryType.Relic)
                {
                    GetRelic(entry.ContentId);
                }

                if (entry.Type == PoolEntryType.EffectRecipe)
                {
                    EffectDefinition effect =
                        GetEffect(entry.ContentId);

                    if (!effect.CanBeItemized)
                    {
                        throw new InvalidOperationException(
                            $"Pool '{pool.Id}' references Effect '{effect.Id}', " +
                            "but it cannot be itemized.");
                    }
                }

                if (entry.Type == PoolEntryType.PassiveRecipe)
                {
                    PassiveDefinition passive =
                        GetPassive(entry.ContentId);

                    if (!passive.CanBeItemized)
                    {
                        throw new InvalidOperationException(
                            $"Pool '{pool.Id}' references Passive '{passive.Id}', " +
                            "but it cannot be itemized.");
                    }
                }

                if (entry.Type == PoolEntryType.AbilityRecipe)
                {
                    AbilityDefinition ability =
                        GetAbility(entry.ContentId);

                    if (!ability.CanBeItemized)
                    {
                        throw new InvalidOperationException(
                            $"Pool '{pool.Id}' references Ability '{ability.Id}', " +
                            "but it cannot be itemized.");
                    }
                }
            }
        }

        private static void ValidateId(
            string id,
            string contentType)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidOperationException(
                    $"{contentType} definition has no Id.");
            }
        }
    }
}