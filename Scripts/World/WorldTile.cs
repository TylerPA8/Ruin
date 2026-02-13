using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using RuinGamePDT.Resources;

namespace RuinGamePDT.World;

public class WorldTile
{
    public Vector2I GridPosition { get; set; }
    public BiomeType Biome { get; set; }
    public bool IsRevealed { get; set; }
    public bool IsExplored { get; set; }

    // Generated on reveal
    public Dictionary<ResourceType, int> ResourceValues { get; private set; }
    public List<ResourceType> TopTreasures { get; private set; }
    public MonsterType EnemyType { get; private set; }

    public WorldTile(Vector2I gridPosition, BiomeType biome)
    {
        GridPosition = gridPosition;
        Biome = biome;
        IsRevealed = false;
        IsExplored = false;
        ResourceValues = new Dictionary<ResourceType, int>();
        TopTreasures = new List<ResourceType>();
        EnemyType = MonsterType.None;
    }

    public void Reveal()
    {
        if (IsRevealed)
            return;

        IsRevealed = true;

        var biomeDefinition = BiomeDatabase.GetBiome(Biome);
        var rng = new Random();

        // Step 1: Generate resource values (0-10) with biome multipliers
        ResourceValues.Clear();
        foreach (ResourceType resType in Enum.GetValues<ResourceType>())
        {
            int value = biomeDefinition.GetRandomResourceValue(resType, rng);
            ResourceValues[resType] = value;
        }

        // Step 2: Select top 2 treasures based on highest values
        TopTreasures = ResourceValues
            .OrderByDescending(kvp => kvp.Value)
            .ThenBy(kvp => kvp.Key) // Secondary sort for consistency
            .Take(2)
            .Select(kvp => kvp.Key)
            .ToList();

        // Step 3: Select 1 enemy type based on biome weights
        EnemyType = biomeDefinition.SelectRandomMonster(rng);
    }

    public override string ToString()
    {
        if (!IsRevealed)
            return $"Tile at {GridPosition} (Hidden)";

        var treasure1 = TopTreasures.Count > 0 ? TopTreasures[0].ToString() : "None";
        var treasure2 = TopTreasures.Count > 1 ? TopTreasures[1].ToString() : "None";

        return $"{Biome} at {GridPosition}: {EnemyType} guards {treasure1} and {treasure2}";
    }
}
