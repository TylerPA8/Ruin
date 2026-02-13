using System;
using System.Collections.Generic;
using System.Linq;
using RuinGamePDT.Resources;

namespace RuinGamePDT.World;

public class BiomeDefinition
{
    public BiomeType Type { get; set; }
    public string Name { get; set; }
    public Dictionary<ResourceType, float> ResourceMultipliers { get; set; }
    public Dictionary<MonsterType, float> MonsterMultipliers { get; set; }

    public BiomeDefinition()
    {
        Name = string.Empty;
        ResourceMultipliers = new Dictionary<ResourceType, float>();
        MonsterMultipliers = new Dictionary<MonsterType, float>();
    }

    public int GetRandomResourceValue(ResourceType type, Random rng)
    {
        float baseValue = rng.Next(0, 11); // 0-10
        float multiplier = ResourceMultipliers.TryGetValue(type, out float value) ? value : 1.0f;
        int finalValue = (int)Math.Round(baseValue * multiplier);
        return Math.Clamp(finalValue, 0, 10);
    }

    public MonsterType SelectRandomMonster(Random rng)
    {
        // Filter out None type and get valid monsters
        var validMonsters = MonsterMultipliers.Where(kvp => kvp.Key != MonsterType.None).ToList();

        if (validMonsters.Count == 0)
            return MonsterType.None;

        float totalWeight = validMonsters.Sum(kvp => kvp.Value);

        if (totalWeight <= 0)
            return MonsterType.None;

        float randomValue = (float)rng.NextDouble() * totalWeight;
        float cumulative = 0;

        foreach (var kvp in validMonsters)
        {
            cumulative += kvp.Value;
            if (randomValue <= cumulative)
                return kvp.Key;
        }

        return validMonsters.Last().Key;
    }
}
