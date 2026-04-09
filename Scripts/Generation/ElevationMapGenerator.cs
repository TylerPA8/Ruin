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

        float[,] primaryNoise = _noiseGen.GenerateNoiseMap(width, height, seed, 0.02f, 6, 0.5f, 2.0f);
        float[,] secondaryNoise = _noiseGen.GenerateNoiseMap(width, height, seed + 500, 0.1f, 3, 0.5f, 2.0f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float combinedNoise = (primaryNoise[x, y] * 0.7f) + (secondaryNoise[x, y] * 0.3f);
                float elevation = MathF.Pow(combinedNoise, 1.2f);
                elevationMap[x, y] = Math.Clamp(elevation, 0.0f, 1.0f);
            }
        }

        return elevationMap;
    }
}
