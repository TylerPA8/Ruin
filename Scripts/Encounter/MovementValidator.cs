using RuinGamePDT.Creatures;
using RuinGamePDT.Resources;

namespace RuinGamePDT.Encounter;

public static class MovementValidator
{
    private static readonly (int dx, int dy)[] Directions = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    public static Dictionary<(int X, int Y), int> GetReachableTiles(Creature creature, EncounterState state)
    {
        var (startX, startY) = state.GetPosition(creature);
        int maxSteps = (int)state.GetRemainingMovement(creature);

        var result = new Dictionary<(int X, int Y), int>();
        var queue = new Queue<((int X, int Y) pos, int steps)>();
        var visited = new HashSet<(int X, int Y)> { (startX, startY) };

        queue.Enqueue(((startX, startY), 0));

        while (queue.Count > 0)
        {
            var (pos, steps) = queue.Dequeue();
            foreach (var (dx, dy) in Directions)
            {
                int nx = pos.X + dx;
                int ny = pos.Y + dy;
                int nextSteps = steps + 1;

                if (nx < 0 || nx >= state.Map.Width || ny < 0 || ny >= state.Map.Height) continue;
                if (visited.Contains((nx, ny))) continue;
                if (state.Map.GetTile(nx, ny) == EncounterTileType.Obstacle) continue;
                if (state.IsOccupied(nx, ny)) continue;
                if (nextSteps > maxSteps) continue;

                visited.Add((nx, ny));
                result[(nx, ny)] = nextSteps;
                queue.Enqueue(((nx, ny), nextSteps));
            }
        }

        return result;
    }
}
