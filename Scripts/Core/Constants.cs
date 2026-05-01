namespace RuinGamePDT.Core;

public static class Constants
{
    // Noise generation defaults
    public const float NOISE_SCALE = 0.03f;
    public const int NOISE_OCTAVES = 5;
    public const float NOISE_PERSISTENCE = 0.5f;
    public const float NOISE_LACUNARITY = 2.0f;

    // Biome thresholds (0-1 range)
    public const float HIGH_ELEVATION_THRESHOLD = 0.77f;
    public const float LOW_ELEVATION_THRESHOLD = 0.3f;
    public const float HIGH_HEAT_THRESHOLD = 0.58f;
    public const float LOW_HEAT_THRESHOLD = 0.35f;
    public const float MEDIUM_HEAT_THRESHOLD = 0.50f;
    public const float HIGH_MOISTURE_THRESHOLD = 0.60f;
    public const float MEDIUM_MOISTURE_THRESHOLD = 0.45f;
    public const float LOW_MOISTURE_THRESHOLD = 0.50f;
}
