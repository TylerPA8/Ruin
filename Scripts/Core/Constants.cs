namespace RuinGamePDT.Core;

public static class Constants
{
    // Tile Configuration
    public const int HEX_RADIUS = 75;  // Radius from center to vertex
    public const int HEX_HEIGHT = HEX_RADIUS * 2;  // 150px (point to point vertically)
    public const float HEX_WIDTH = HEX_RADIUS * 1.732051f;  // ~129.9px (√3 * radius, widest horizontal span)
    public const int DEFAULT_WORLD_SIZE = 50;
    public const float TILE_PADDING = 5f;  // Padding between tiles on all sides

    // Hexagonal Grid Configuration (Pointy-top hexagons)
    public const float HEX_HORIZONTAL_SPACING = HEX_WIDTH + TILE_PADDING;  // 134.904
    public const float HEX_VERTICAL_SPACING = HEX_HEIGHT * 0.75f + TILE_PADDING;  // 117.5
    public const float HEX_ODD_ROW_OFFSET = HEX_HORIZONTAL_SPACING / 2f;  // 67.452

    // Gameplay Configuration
    public const int SCOUT_RANGE = 1;

    // Animation Configuration
    public const float TILE_FLIP_DURATION = 0.3f;
    public const float PARTY_MOVE_DURATION = 0.4f;

    // Resource Value Configuration
    public const int MIN_RESOURCE_VALUE = 0;
    public const int MAX_RESOURCE_VALUE = 10;

    // World Generation Configuration
    public const float NOISE_SCALE = 0.03f;
    public const int NOISE_OCTAVES = 6;
    public const float NOISE_PERSISTENCE = 0.5f;
    public const float NOISE_LACUNARITY = 2.0f;

    // Biome Threshold Configuration
    public const float HIGH_ELEVATION_THRESHOLD = 0.7f;
    public const float LOW_ELEVATION_THRESHOLD = 0.3f;
    public const float HIGH_HEAT_THRESHOLD = 0.6f;
    public const float MEDIUM_HEAT_THRESHOLD = 0.5f;
    public const float LOW_HEAT_THRESHOLD = 0.4f;
}
