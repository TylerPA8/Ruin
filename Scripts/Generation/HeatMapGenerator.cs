using Godot;

namespace RuinGamePDT.Generation;

public class HeatMapGenerator
{
    private NoiseMapGenerator _noiseGen;

    public HeatMapGenerator()
    {
        _noiseGen = new NoiseMapGenerator();
    }

    public float[,] GenerateHeatMap(int width, int height, int seed)
    {
        float[,] heatMap = new float[width, height];

        // Generate noise variation
        float[,] noiseMap = _noiseGen.GenerateNoiseMap(width, height, seed + 1000, 0.05f, 4, 0.5f, 2.0f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Create latitude gradient (higher heat at equator/center, lower at poles)
                float latitudeGradient = 1.0f - Mathf.Abs((y / (float)height) - 0.5f) * 2.0f;

                // Combine latitude gradient (70%) with noise variation (30%)
                float heat = (latitudeGradient * 0.7f) + (noiseMap[x, y] * 0.3f);

                // Ensure 0-1 range
                heatMap[x, y] = Mathf.Clamp(heat, 0.0f, 1.0f);
            }
        }

        return heatMap;
    }
}
