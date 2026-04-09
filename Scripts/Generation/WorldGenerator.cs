using RuinGamePDT.Resources;
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
        Console.WriteLine($"Generating world: {width}x{height}, seed: {seed}");

        Console.WriteLine("Generating heat map...");
        float[,] heatMap = _heatGenerator.GenerateHeatMap(width, height, seed);

        Console.WriteLine("Generating elevation map...");
        float[,] elevationMap = _elevationGenerator.GenerateElevationMap(width, height, seed);

        Console.WriteLine("Mapping biomes...");
        BiomeType[,] biomeMap = _biomeMapper.MapBiomes(heatMap, elevationMap);

        Console.WriteLine("Creating tile data...");
        WorldData world = new WorldData(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                BiomeType biome = biomeMap[x, y];
                WorldTile tile = new WorldTile(x, y, biome);
                world.SetTile(x, y, tile);
            }
        }

        PrintBiomeStats(biomeMap);

        Console.WriteLine("World generation complete!");
        return world;
    }

    private void PrintBiomeStats(BiomeType[,] biomeMap)
    {
        int width = biomeMap.GetLength(0);
        int height = biomeMap.GetLength(1);
        int totalTiles = width * height;

        int forestCount = 0, plainsCount = 0, desertCount = 0, swampCount = 0, mountainsCount = 0;

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

        Console.WriteLine("=== Biome Distribution ===");
        Console.WriteLine($"Forest:    {forestCount} ({forestCount * 100.0f / totalTiles:F1}%)");
        Console.WriteLine($"Plains:    {plainsCount} ({plainsCount * 100.0f / totalTiles:F1}%)");
        Console.WriteLine($"Desert:    {desertCount} ({desertCount * 100.0f / totalTiles:F1}%)");
        Console.WriteLine($"Swamp:     {swampCount} ({swampCount * 100.0f / totalTiles:F1}%)");
        Console.WriteLine($"Mountains: {mountainsCount} ({mountainsCount * 100.0f / totalTiles:F1}%)");
        Console.WriteLine("==========================");
    }
}
