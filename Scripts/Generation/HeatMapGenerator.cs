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
        float[,] noiseMap = _noiseGen.GenerateNoiseMap(width, height, seed + 1000, 0.05f, 4, 0.5f, 2.0f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float latitudeGradient = 1.0f - MathF.Abs((y / (float)height) - 0.5f) * 2.0f;
                float heat = (latitudeGradient * 0.7f) + (noiseMap[x, y] * 0.3f);
                heatMap[x, y] = Math.Clamp(heat, 0.0f, 1.0f);
            }
        }

        return heatMap;
    }
}
