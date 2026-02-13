using Godot;

namespace RuinGamePDT.Generation;

public class ElevationMapGenerator
{
    private NoiseMapGenerator _noiseGen;

    public ElevationMapGenerator()
    {
        _noiseGen = new NoiseMapGenerator();
    }

    public float[,] GenerateElevationMap(int width, int height, int seed)
    {
        float[,] elevationMap = new float[width, height];

        // Generate primary noise (large features)
        float[,] primaryNoise = _noiseGen.GenerateNoiseMap(width, height, seed, 0.02f, 6, 0.5f, 2.0f);

        // Generate secondary noise (detail)
        float[,] secondaryNoise = _noiseGen.GenerateNoiseMap(width, height, seed + 500, 0.1f, 3, 0.5f, 2.0f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Combine primary (70%) and secondary (30%) noise
                float combinedNoise = (primaryNoise[x, y] * 0.7f) + (secondaryNoise[x, y] * 0.3f);

                // Apply power curve for more dramatic terrain
                float elevation = Mathf.Pow(combinedNoise, 1.2f);

                elevationMap[x, y] = Mathf.Clamp(elevation, 0.0f, 1.0f);
            }
        }

        return elevationMap;
    }
}
