using RuinGamePDT.Creatures;
using RuinGamePDT.Resources;
using RuinGamePDT.World;

namespace RuinGamePDT.Encounter;

public class EncounterState(EncounterMap map)
{
    public EncounterMap Map { get; } = map;
    public List<Creature> Mercenaries { get; } = [];
    public List<Creature> Enemies { get; } = [];

    private readonly Dictionary<(int X, int Y), Creature> _occupancy = new();
    private readonly Dictionary<Creature, (int X, int Y)> _positions = new();
    private readonly Dictionary<Creature, float> _remainingMovement = new();

    public bool PlaceCreature(Creature creature, int x, int y)
    {
        if (Map.GetTile(x, y) == EncounterTileType.Obstacle) return false;
        if (_occupancy.ContainsKey((x, y))) return false;
        _occupancy[(x, y)] = creature;
        _positions[creature] = (x, y);
        _remainingMovement[creature] = creature.CombatStats.MovementPoints;
        return true;
    }

    public Creature? GetCreatureAt(int x, int y) =>
        _occupancy.TryGetValue((x, y), out var c) ? c : null;

    public bool IsOccupied(int x, int y) => _occupancy.ContainsKey((x, y));

    public (int X, int Y) GetPosition(Creature creature) => _positions[creature];

    public float GetRemainingMovement(Creature creature) => _remainingMovement[creature];

    public void ResetMovement(Creature creature) =>
        _remainingMovement[creature] = creature.CombatStats.MovementPoints;
}
