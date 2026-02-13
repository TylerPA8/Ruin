using Godot;
using RuinGamePDT.World;
using RuinGamePDT.Party;

namespace RuinGamePDT.Core;

public partial class MainScene : Node2D
{
    private GameManager? _gameManager;
    private WorldMap? _worldMap;
    private Party.Party? _party;
    private HexGrid? _hexGrid;

    public override void _Ready()
    {
        // Get node references
        _gameManager = GetNode<GameManager>("GameManager");
        _worldMap = GetNode<WorldMap>("WorldMap");
        _party = GetNode<Party.Party>("Party");
        _hexGrid = GetNode<HexGrid>("HexGrid");

        // Start a new game
        CallDeferred(nameof(InitializeGame));
    }

    private void InitializeGame()
    {
        if (_gameManager == null || _worldMap == null || _party == null)
        {
            GD.PrintErr("MainScene: Missing required nodes!");
            return;
        }

        // Start new game
        _gameManager.StartNewGame(30); // 30x30 world for testing

        // Initialize world map
        var worldData = _gameManager.CurrentWorld;
        if (worldData != null)
        {
            _worldMap.Initialize(worldData);

            // Initialize hex grid background
            if (_hexGrid != null)
            {
                _hexGrid.Initialize(worldData.Width, worldData.Height);
            }
        }

        // Initialize party
        var partyData = _gameManager.PlayerParty;
        if (partyData != null && worldData != null)
        {
            _party.Initialize(partyData, partyData.CurrentPosition);

            // Center camera on party
            var camera = GetNode<Camera2D>("Camera2D");
            if (camera != null)
            {
                camera.Position = _party.Position;
            }

            // Update the starting tile's visual
            var startTileNode = _worldMap.GetTileNode(partyData.CurrentPosition);
            if (startTileNode != null)
            {
                startTileNode.UpdateVisual();
            }
        }

        GD.Print("Game initialized successfully!");
    }
}
