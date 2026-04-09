namespace RuinGamePDT.World;

public class WorldData(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    private readonly WorldTile?[,] _tiles = new WorldTile[width, height];

    public void SetTile(int x, int y, WorldTile tile) => _tiles[x, y] = tile;
    public WorldTile? GetTile(int x, int y) => _tiles[x, y];
}
