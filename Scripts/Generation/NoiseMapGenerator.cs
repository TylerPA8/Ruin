using RuinGamePDT.Core;

namespace RuinGamePDT.Generation;

public class NoiseMapGenerator
{
    private FastNoiseLite _noise;

    public NoiseMapGenerator()
    {
        _noise = new FastNoiseLite();
    }

    public float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity)
    {
        _noise.SetSeed(seed);
        _noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        _noise.SetFrequency(scale);
        _noise.SetFractalOctaves(octaves);
        _noise.SetFractalGain(persistence);
        _noise.SetFractalLacunarity(lacunarity);

        float[,] noiseMap = new float[width, height];
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = _noise.GetNoise(x, y);
                noiseMap[x, y] = noiseValue;

                if (noiseValue < minValue) minValue = noiseValue;
                if (noiseValue > maxValue) maxValue = noiseValue;
            }
        }

        // Normalize to 0-1 range
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                noiseMap[x, y] = (noiseMap[x, y] - minValue) / (maxValue - minValue);

        return noiseMap;
    }

    public float[,] GenerateNoiseMap(int width, int height, int seed)
    {
        return GenerateNoiseMap(width, height, seed, Constants.NOISE_SCALE, Constants.NOISE_OCTAVES, Constants.NOISE_PERSISTENCE, Constants.NOISE_LACUNARITY);
    }
}
