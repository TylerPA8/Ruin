using System;
using System.Collections.Generic;
using RuinGamePDT.Resources;

namespace RuinGamePDT.World;

public static class BiomeDatabase
{
    private static Dictionary<BiomeType, BiomeDefinition>? _biomes;

    public static void Initialize()
    {
        _biomes = new Dictionary<BiomeType, BiomeDefinition>();

        // Forest: wood×2, stone×1.5, metal×0.5, gold×1.5, gems×1
        // bandits×2, beasts×1.5, monstrosities×0.5
        _biomes[BiomeType.Forest] = new BiomeDefinition
        {
            Type = BiomeType.Forest,
            Name = "Forest",
            ResourceMultipliers = new Dictionary<ResourceType, float>
            {
                { ResourceType.Wood, 2.0f },
                { ResourceType.Stone, 1.5f },
                { ResourceType.Metal, 0.5f },
                { ResourceType.Gold, 1.5f },
                { ResourceType.Gems, 1.0f }
            },
            MonsterMultipliers = new Dictionary<MonsterType, float>
            {
                { MonsterType.Bandits, 2.0f },
                { MonsterType.Beasts, 1.5f },
                { MonsterType.Monstrosities, 0.5f }
            }
        };

        // Plains: wood×0.01, stone×1, metal×0.01, gold×0.01, gems×1
        // bandits×1.5, beasts×2, monstrosities×0.5
        _biomes[BiomeType.Plains] = new BiomeDefinition
        {
            Type = BiomeType.Plains,
            Name = "Plains",
            ResourceMultipliers = new Dictionary<ResourceType, float>
            {
                { ResourceType.Wood, 0.01f },
                { ResourceType.Stone, 1.0f },
                { ResourceType.Metal, 0.01f },
                { ResourceType.Gold, 0.01f },
                { ResourceType.Gems, 1.0f }
            },
            MonsterMultipliers = new Dictionary<MonsterType, float>
            {
                { MonsterType.Bandits, 1.5f },
                { MonsterType.Beasts, 2.0f },
                { MonsterType.Monstrosities, 0.5f }
            }
        };

        // Desert: wood×0.01, stone×0.01, metal×0.01, gold×2, gems×1
        // bandits×1, beasts×0.01, monstrosities×2
        _biomes[BiomeType.Desert] = new BiomeDefinition
        {
            Type = BiomeType.Desert,
            Name = "Desert",
            ResourceMultipliers = new Dictionary<ResourceType, float>
            {
                { ResourceType.Wood, 0.01f },
                { ResourceType.Stone, 0.01f },
                { ResourceType.Metal, 0.01f },
                { ResourceType.Gold, 2.0f },
                { ResourceType.Gems, 1.0f }
            },
            MonsterMultipliers = new Dictionary<MonsterType, float>
            {
                { MonsterType.Bandits, 1.0f },
                { MonsterType.Beasts, 0.01f },
                { MonsterType.Monstrosities, 2.0f }
            }
        };

        // Swamp: wood×1.5, stone×0.01, metal×0.01, gold×1.5, gems×1
        // bandits×0.01, beasts×1, monstrosities×1.5
        _biomes[BiomeType.Swamp] = new BiomeDefinition
        {
            Type = BiomeType.Swamp,
            Name = "Swamp",
            ResourceMultipliers = new Dictionary<ResourceType, float>
            {
                { ResourceType.Wood, 1.5f },
                { ResourceType.Stone, 0.01f },
                { ResourceType.Metal, 0.01f },
                { ResourceType.Gold, 1.5f },
                { ResourceType.Gems, 1.0f }
            },
            MonsterMultipliers = new Dictionary<MonsterType, float>
            {
                { MonsterType.Bandits, 0.01f },
                { MonsterType.Beasts, 1.0f },
                { MonsterType.Monstrosities, 1.5f }
            }
        };

        // Mountains: wood×0.5, stone×2, metal×2, gold×2, gems×1
        // bandits×1, beasts×1.5, monstrosities×2
        _biomes[BiomeType.Mountains] = new BiomeDefinition
        {
            Type = BiomeType.Mountains,
            Name = "Mountains",
            ResourceMultipliers = new Dictionary<ResourceType, float>
            {
                { ResourceType.Wood, 0.5f },
                { ResourceType.Stone, 2.0f },
                { ResourceType.Metal, 2.0f },
                { ResourceType.Gold, 2.0f },
                { ResourceType.Gems, 1.0f }
            },
            MonsterMultipliers = new Dictionary<MonsterType, float>
            {
                { MonsterType.Bandits, 1.0f },
                { MonsterType.Beasts, 1.5f },
                { MonsterType.Monstrosities, 2.0f }
            }
        };
    }

    public static BiomeDefinition GetBiome(BiomeType type)
    {
        if (_biomes == null)
            Initialize();

        return _biomes![type];
    }

    public static Dictionary<BiomeType, BiomeDefinition> GetAllBiomes()
    {
        if (_biomes == null)
            Initialize();

        return _biomes!;
    }
}
