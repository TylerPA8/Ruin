using Godot;
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
        _noise.Seed = seed;
        _noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex;
        _noise.Frequency = scale;
        _noise.FractalOctaves = octaves;
        _noise.FractalGain = persistence;
        _noise.FractalLacunarity = lacunarity;

        float[,] noiseMap = new float[width, height];
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        // Generate raw noise values
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = _noise.GetNoise2D(x, y);
                noiseMap[x, y] = noiseValue;

                if (noiseValue < minValue)
                    minValue = noiseValue;
                if (noiseValue > maxValue)
                    maxValue = noiseValue;
            }
        }

        // Normalize to 0-1 range
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minValue, maxValue, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

    public float[,] GenerateNoiseMap(int width, int height, int seed)
    {
        return GenerateNoiseMap(width, height, seed, Constants.NOISE_SCALE, Constants.NOISE_OCTAVES, Constants.NOISE_PERSISTENCE, Constants.NOISE_LACUNARITY);
    }
}
