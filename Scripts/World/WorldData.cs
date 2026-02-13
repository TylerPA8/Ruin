using System;
using System.Collections.Generic;
using Godot;

namespace RuinGamePDT.World;

public class WorldData
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public WorldTile[,] Tiles { get; private set; }
    public Vector2I PartyPosition { get; set; }

    public WorldData(int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new WorldTile[width, height];
        PartyPosition = new Vector2I(width / 2, height / 2); // Default to center
    }

    public WorldTile? GetTile(int x, int y)
    {
        if (!IsWithinBounds(x, y))
            return null;

        return Tiles[x, y];
    }

    public WorldTile? GetTile(Vector2I pos)
    {
        return GetTile(pos.X, pos.Y);
    }

    public List<WorldTile> GetNeighbors(int x, int y, int range)
    {
        var neighbors = new List<WorldTile>();

        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = -range; dy <= range; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nx = x + dx;
                int ny = y + dy;

                var tile = GetTile(nx, ny);
                if (tile != null)
                {
                    neighbors.Add(tile);
                }
            }
        }

        return neighbors;
    }

    public List<WorldTile> GetNeighbors(Vector2I pos, int range)
    {
        return GetNeighbors(pos.X, pos.Y, range);
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public bool IsWithinBounds(Vector2I pos)
    {
        return IsWithinBounds(pos.X, pos.Y);
    }

    public int GetDistance(Vector2I pos1, Vector2I pos2)
    {
        // Chebyshev distance (max of absolute differences)
        return Math.Max(Math.Abs(pos1.X - pos2.X), Math.Abs(pos1.Y - pos2.Y));
    }

    public void SetTile(int x, int y, WorldTile tile)
    {
        if (IsWithinBounds(x, y))
        {
            Tiles[x, y] = tile;
        }
    }
}
