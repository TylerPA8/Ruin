using Godot;
using RuinGamePDT.Core;
using System.Collections.Generic;

namespace RuinGamePDT.World;

public partial class WorldMap : Node2D
{
    private WorldData? _worldData;
    private Dictionary<Vector2I, Tile> _tileNodes = new();
    private PackedScene? _tileScene;

    public override void _Ready()
    {
        _tileScene = GD.Load<PackedScene>("res://Scenes/World/Tile.tscn");
    }

    public void Initialize(WorldData worldData)
    {
        _worldData = worldData;
        CreateTileNodes();
    }

    private void CreateTileNodes()
    {
        if (_worldData == null || _tileScene == null)
            return;

        GD.Print($"Creating tile nodes for {_worldData.Width}x{_worldData.Height} world...");

        for (int x = 0; x < _worldData.Width; x++)
        {
            for (int y = 0; y < _worldData.Height; y++)
            {
                var tileData = _worldData.GetTile(x, y);
                if (tileData != null)
                {
                    CreateTileNode(tileData);
                }
            }
        }

        GD.Print($"Created {_tileNodes.Count} tile nodes");
    }

    private void CreateTileNode(WorldTile data)
    {
        if (_tileScene == null)
            return;

        var tileNode = _tileScene.Instantiate<Tile>();
        tileNode.Initialize(data);

        // Calculate hexagonal world position (flat-top hexagons)
        int x = data.GridPosition.X;
        int y = data.GridPosition.Y;

        float xPos = x * Constants.HEX_HORIZONTAL_SPACING;
        float yPos = y * Constants.HEX_VERTICAL_SPACING;

        // Offset odd rows to create hexagonal pattern
        if (y % 2 == 1)
        {
            xPos += Constants.HEX_ODD_ROW_OFFSET;
        }

        Vector2 worldPos = new Vector2(xPos, yPos);

        tileNode.Position = worldPos;
        AddChild(tileNode);
        _tileNodes[data.GridPosition] = tileNode;

        // Connect to tile clicked signal
        tileNode.TileClicked += OnTileClicked;
    }

    private void OnTileClicked(Tile tile)
    {
        var tileData = tile.GetWorldTile();
        if (tileData != null)
        {
            GD.Print($"Tile clicked at {tileData.GridPosition}");
            // This will be handled by InputController
        }
    }

    public Tile? GetTileNode(Vector2I gridPos)
    {
        _tileNodes.TryGetValue(gridPos, out var tile);
        return tile;
    }

    public void UpdateAllVisuals()
    {
        foreach (var tile in _tileNodes.Values)
        {
            tile.UpdateVisual();
        }
    }

    public WorldData? GetWorldData()
    {
        return _worldData;
    }
}
