using RuinGamePDT.Resources;
using RuinGamePDT.World;

namespace RuinGamePDT.Generation;

public class EncounterMapGenerator
{
    private readonly NoiseMapGenerator _noiseGen = new();

    private static (float obstacle, float hazard) GetBiomeRatios(BiomeType biome) => biome switch
    {
        BiomeType.Plains    => (0.10f, 0.01f),
        BiomeType.Forest    => (0.40f, 0.05f),
        BiomeType.Desert    => (0.50f, 0.05f),
        BiomeType.Swamp     => (0.75f, 0.15f),
        BiomeType.Mountains => (0.25f, 0.25f),
        _                   => (0.00f, 0.00f)
    };

    public EncounterMap Generate(BiomeType biome, int seed)
    {
        const int size = 50;
        var (obstaclePct, hazardPct) = GetBiomeRatios(biome);

        // Two independent noise maps so hazards and obstacles have separate spatial patterns
        float[,] obstacleNoise = _noiseGen.GenerateNoiseMap(size, size, seed,        0.08f, 4, 0.5f, 2.0f);
        float[,] hazardNoise   = _noiseGen.GenerateNoiseMap(size, size, seed + 7777, 0.08f, 4, 0.5f, 2.0f);

        float obstacleThreshold = Percentile(obstacleNoise, size, 1.0f - obstaclePct);

        // Hazard threshold is adjusted so that after obstacle tiles are excluded,
        // the remaining hazard tiles still equal hazardPct of total tiles.
        float adjustedHazardPct = hazardPct / (1.0f - obstaclePct);
        float hazardThreshold   = Percentile(hazardNoise,   size, 1.0f - adjustedHazardPct);

        EncounterMap map = new(size, size);
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                if (obstacleNoise[x, y] >= obstacleThreshold)
                    map.SetTile(x, y, EncounterTileType.Obstacle);
                else if (hazardNoise[x, y] >= hazardThreshold)
                    map.SetTile(x, y, EncounterTileType.Hazard);
            }

        return map;
    }

    private static float Percentile(float[,] noise, int size, float fraction)
    {
        var sorted = new float[size * size];
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                sorted[x * size + y] = noise[x, y];
        Array.Sort(sorted);
        return sorted[(int)(sorted.Length * Math.Clamp(fraction, 0f, 1f))];
    }
}
