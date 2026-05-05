using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RuinGamePDT.Creatures;
using RuinGamePDT.Encounter;
using RuinGamePDT.Resources;

namespace RuinGamePDT.Rendering;

public class EncounterScene(EncounterState state, TurnManager turns, Texture2D pixel)
{
    private const int TileSize = 25;

    private Creature? _selected;
    private Dictionary<(int X, int Y), int> _reachable = new();
    private MouseState _prevMouse;

    public void Update(MouseState mouse)
    {
        if (mouse.LeftButton == ButtonState.Pressed &&
            _prevMouse.LeftButton == ButtonState.Released)
        {
            int gridX = mouse.X / TileSize;
            int gridY = mouse.Y / TileSize;

            if (gridX < 0 || gridX >= state.Map.Width ||
                gridY < 0 || gridY >= state.Map.Height)
            {
                _selected = null;
                _reachable = new();
                _prevMouse = mouse;
                return;
            }

            if (_selected == null)
            {
                var creature = state.GetCreatureAt(gridX, gridY);
                if (creature is Mercenary && turns.CanMove(creature))
                {
                    _selected = creature;
                    _reachable = MovementValidator.GetReachableTiles(creature, state);
                }
            }
            else if (_reachable.ContainsKey((gridX, gridY)))
            {
                state.MoveCreature(_selected, gridX, gridY);
                turns.EndCreatureTurn(_selected);
                _selected = null;
                _reachable = new();
            }
            else
            {
                _selected = null;
                _reachable = new();
            }
        }

        _prevMouse = mouse;
    }

    public void Draw(SpriteBatch sb)
    {
        // Terrain
        for (int x = 0; x < state.Map.Width; x++)
        for (int y = 0; y < state.Map.Height; y++)
        {
            var color = state.Map.GetTile(x, y) switch
            {
                EncounterTileType.Obstacle => new Color(50, 50, 50),
                EncounterTileType.Hazard   => new Color(200, 100, 0),
                _                          => new Color(90, 90, 90)
            };
            sb.Draw(pixel, new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize), color);
        }

        // Reachable highlights
        foreach (var (pos, _) in _reachable)
            sb.Draw(pixel, new Rectangle(pos.X * TileSize, pos.Y * TileSize, TileSize, TileSize), Color.Yellow * 0.4f);

        // Mercenaries
        foreach (var merc in state.Mercenaries)
        {
            var pos = state.GetPosition(merc);
            var color = merc == _selected ? Color.Cyan : Color.DodgerBlue;
            sb.Draw(pixel, new Rectangle(pos.X * TileSize, pos.Y * TileSize, TileSize, TileSize), color);
        }

        // Enemies
        foreach (var enemy in state.Enemies)
        {
            var pos = state.GetPosition(enemy);
            sb.Draw(pixel, new Rectangle(pos.X * TileSize, pos.Y * TileSize, TileSize, TileSize), Color.Crimson);
        }
    }
}
