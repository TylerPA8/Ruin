using Godot;
using RuinGamePDT.Generation;
using RuinGamePDT.World;
using RuinGamePDT.Party;
using System;

namespace RuinGamePDT.Core;

public partial class GameManager : Node
{
    private static GameManager? _instance;
    public static GameManager Instance => _instance!;

    private WorldData? _currentWorld;
    private PartyData? _playerParty;
    private WorldGenerator? _worldGenerator;

    public WorldData? CurrentWorld => _currentWorld;
    public PartyData? PlayerParty => _playerParty;

    public override void _Ready()
    {
        if (_instance != null && _instance != this)
        {
            QueueFree();
            return;
        }

        _instance = this;
        _worldGenerator = new WorldGenerator();

        // Initialize biome database
        BiomeDatabase.Initialize();

        GD.Print("GameManager initialized");
    }

    public void StartNewGame(int worldSize = Constants.DEFAULT_WORLD_SIZE)
    {
        GD.Print($"Starting new game with world size {worldSize}x{worldSize}");

        // Generate world
        int seed = (int)DateTime.Now.Ticks;
        _currentWorld = _worldGenerator!.GenerateWorld(worldSize, worldSize, seed);

        // Create party at world center
        Vector2I startPos = new Vector2I(worldSize / 2, worldSize / 2);
        _playerParty = new PartyData(startPos);
        _currentWorld.PartyPosition = startPos;

        // Reveal starting tile
        var startTile = _currentWorld.GetTile(startPos);
        if (startTile != null)
        {
            startTile.Reveal();
            GD.Print($"Starting at {startPos}: {startTile}");
        }

        GD.Print("Game started!");
    }

    public override void _ExitTree()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
