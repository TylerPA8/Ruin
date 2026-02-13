using Godot;
using RuinGamePDT.Resources;
using System;

namespace RuinGamePDT.World;

public partial class Tile : Node2D
{
    [Signal]
    public delegate void TileClickedEventHandler(Tile tile);

    private WorldTile? _data;
    private Sprite2D? _sprite;
    private Area2D? _clickArea;

    private bool _isHighlighted = false;

    // Sprite paths
    private const string SPRITE_FOREST = "res://Assets/Sprites/Tiles/tile_forest.svg";
    private const string SPRITE_PLAINS = "res://Assets/Sprites/Tiles/tile_plains.svg";
    private const string SPRITE_DESERT = "res://Assets/Sprites/Tiles/tile_desert.svg";
    private const string SPRITE_SWAMP = "res://Assets/Sprites/Tiles/tile_swamp.svg";
    private const string SPRITE_MOUNTAINS = "res://Assets/Sprites/Tiles/tile_mountains.svg";

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _clickArea = GetNode<Area2D>("Area2D");

        _clickArea.InputEvent += OnAreaInputEvent;
    }

    public void Initialize(WorldTile tileData)
    {
        _data = tileData;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (_data == null || _sprite == null)
            return;

        // Only show the biome sprite if the tile is revealed
        if (_data.IsRevealed)
        {
            _sprite.Texture = GD.Load<Texture2D>(GetBiomeSpritePath(_data.Biome));
            _sprite.Visible = true;
        }
        else
        {
            _sprite.Visible = false;  // Hide sprite until revealed
        }
    }

    public void ShowHighlight(bool enabled)
    {
        _isHighlighted = enabled;

        if (_sprite == null)
            return;

        if (enabled)
        {
            _sprite.Modulate = new Color(1.2f, 1.2f, 1.2f);
        }
        else
        {
            _sprite.Modulate = Colors.White;
        }
    }

    private void OnAreaInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
            {
                EmitSignal(SignalName.TileClicked, this);
            }
        }
    }

    private string GetBiomeSpritePath(BiomeType biome)
    {
        return biome switch
        {
            BiomeType.Forest => SPRITE_FOREST,
            BiomeType.Plains => SPRITE_PLAINS,
            BiomeType.Desert => SPRITE_DESERT,
            BiomeType.Swamp => SPRITE_SWAMP,
            BiomeType.Mountains => SPRITE_MOUNTAINS,
            _ => SPRITE_FOREST  // Default to forest if unknown
        };
    }

    public WorldTile? GetWorldTile()
    {
        return _data;
    }
}
