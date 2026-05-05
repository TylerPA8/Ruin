using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RuinGamePDT.Creatures;
using RuinGamePDT.Encounter;
using RuinGamePDT.Generation;
using RuinGamePDT.Rendering;
using RuinGamePDT.Resources;

namespace RuinGamePDT;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = null!;
    private Texture2D _pixel = null!;
    private EncounterState _encounterState = null!;
    private TurnManager _turnManager = null!;
    private EncounterScene _scene = null!;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 1250,
            PreferredBackBufferHeight = 1250
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        var map = new EncounterMapGenerator().Generate(BiomeType.Plains, seed: 42);
        _encounterState = new EncounterState(map);

        var merc1 = new Mercenary(stamina: 6);
        var merc2 = new Mercenary(stamina: 6);
        var enemy = new PricklebackGoblin();

        _encounterState.Mercenaries.Add(merc1);
        _encounterState.Mercenaries.Add(merc2);
        _encounterState.Enemies.Add(enemy);

        _encounterState.PlaceCreature(merc1, 0, 0);
        _encounterState.PlaceCreature(merc2, 1, 0);

        // Try to place enemy; find a free tile if (49,49) is blocked
        bool placed = _encounterState.PlaceCreature(enemy, 49, 49);
        if (!placed)
        {
            for (int x = 49; x >= 0 && !placed; x--)
                for (int y = 49; y >= 0 && !placed; y--)
                    placed = _encounterState.PlaceCreature(enemy, x, y);
        }

        _turnManager = new TurnManager(_encounterState);
        _turnManager.StartEncounter();

        _scene = new EncounterScene(_encounterState, _turnManager, _pixel);
    }

    protected override void Update(GameTime gameTime)
    {
        _scene.Update(Mouse.GetState());

        // Auto-skip enemy phases (placeholder — no AI yet)
        if (_turnManager.CurrentPhase == 2 || _turnManager.CurrentPhase == 4)
        {
            var toSkip = _encounterState.Enemies.Where(_turnManager.CanMove).ToList();
            foreach (var e in toSkip) _turnManager.EndCreatureTurn(e);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();
        _scene.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
