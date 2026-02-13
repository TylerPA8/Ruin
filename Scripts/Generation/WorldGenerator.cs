using Godot;
using RuinGamePDT.World;

namespace RuinGamePDT.Generation;

public class WorldGenerator
{
    private NoiseMapGenerator _noiseGenerator;
    private HeatMapGenerator _heatGenerator;
    private ElevationMapGenerator _elevationGenerator;
    private BiomeMapper _biomeMapper;

    public WorldGenerator()
    {
        _noiseGenerator = new NoiseMapGenerator();
        _heatGenerator = new HeatMapGenerator();
        _elevationGenerator = new ElevationMapGenerator();
        _biomeMapper = new BiomeMapper();
    }

    public WorldData GenerateWorld(int width, int height, int seed)
    {
        GD.Print($"Generating world: {width}x{height}, seed: {seed}");

        // Stage 1: Generate heat map
        GD.Print("Generating heat map...");
        float[,] heatMap = _heatGenerator.GenerateHeatMap(width, height, seed);

        // Stage 2: Generate elevation map
        GD.Print("Generating elevation map...");
        float[,] elevationMap = _elevationGenerator.GenerateElevationMap(width, height, seed);

        // Stage 3: Map biomes based on heat and elevation
        GD.Print("Mapping biomes...");
        BiomeType[,] biomeMap = _biomeMapper.MapBiomes(heatMap, elevationMap);

        // Stage 4: Create WorldData and populate with WorldTile
        GD.Print("Creating tile data...");
        WorldData world = new WorldData(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2I gridPos = new Vector2I(x, y);
                BiomeType biome = biomeMap[x, y];
                WorldTile tile = new WorldTile(gridPos, biome);
                world.SetTile(x, y, tile);
            }
        }

        // Print biome distribution statistics
        PrintBiomeStats(biomeMap);

        GD.Print("World generation complete!");
        return world;
    }

    private void PrintBiomeStats(BiomeType[,] biomeMap)
    {
        int width = biomeMap.GetLength(0);
        int height = biomeMap.GetLength(1);
        int totalTiles = width * height;

        int forestCount = 0;
        int plainsCount = 0;
        int desertCount = 0;
        int swampCount = 0;
        int mountainsCount = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                switch (biomeMap[x, y])
                {
                    case BiomeType.Forest: forestCount++; break;
                    case BiomeType.Plains: plainsCount++; break;
                    case BiomeType.Desert: desertCount++; break;
                    case BiomeType.Swamp: swampCount++; break;
                    case BiomeType.Mountains: mountainsCount++; break;
                }
            }
        }

        GD.Print("=== Biome Distribution ===");
        GD.Print($"Forest: {forestCount} ({forestCount * 100.0f / totalTiles:F1}%)");
        GD.Print($"Plains: {plainsCount} ({plainsCount * 100.0f / totalTiles:F1}%)");
        GD.Print($"Desert: {desertCount} ({desertCount * 100.0f / totalTiles:F1}%)");
        GD.Print($"Swamp: {swampCount} ({swampCount * 100.0f / totalTiles:F1}%)");
        GD.Print($"Mountains: {mountainsCount} ({mountainsCount * 100.0f / totalTiles:F1}%)");
        GD.Print("========================");
    }
}
