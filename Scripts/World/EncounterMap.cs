using RuinGamePDT.Resources;

namespace RuinGamePDT.World;

public class EncounterMap(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    private readonly EncounterTileType[,] _tiles = new EncounterTileType[width, height];

    public EncounterTileType GetTile(int x, int y) => _tiles[x, y];
    public void SetTile(int x, int y, EncounterTileType type) => _tiles[x, y] = type;
}
