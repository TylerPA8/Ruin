using RuinGamePDT.Creatures;
using RuinGamePDT.Encounter;
using RuinGamePDT.Resources;
using RuinGamePDT.World;

namespace RuinGamePDT.Tests;

public class MovementValidatorTests
{
    [Fact]
    public void GetReachableTiles_StopsAtMovementBoundary()
    {
        // Mercenary(stamina:0) → MovementPoints = MathF.Round(0*0.5+3) = 3
        var state = new EncounterState(new EncounterMap(10, 10));
        var merc = new Mercenary(stamina: 0);
        state.PlaceCreature(merc, 5, 5);
        var reachable = MovementValidator.GetReachableTiles(merc, state);
        Assert.Contains((5, 8), reachable.Keys);      // 3 steps — in range
        Assert.DoesNotContain((5, 9), reachable.Keys); // 4 steps — out of range
    }

    [Fact]
    public void GetReachableTiles_DoesNotIncludeObstacleTiles()
    {
        var map = new EncounterMap(10, 10);
        map.SetTile(5, 6, EncounterTileType.Obstacle);
        var state = new EncounterState(map);
        var merc = new Mercenary(stamina: 6); // MovementPoints = MathF.Round(3+3) = 6
        state.PlaceCreature(merc, 5, 5);
        var reachable = MovementValidator.GetReachableTiles(merc, state);
        Assert.DoesNotContain((5, 6), reachable.Keys);
    }

    [Fact]
    public void GetReachableTiles_DoesNotIncludeOccupiedTiles()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        var merc1 = new Mercenary(stamina: 6);
        var merc2 = new Mercenary(stamina: 6);
        state.PlaceCreature(merc1, 5, 5);
        state.PlaceCreature(merc2, 5, 6);
        var reachable = MovementValidator.GetReachableTiles(merc1, state);
        Assert.DoesNotContain((5, 6), reachable.Keys);
    }

    [Fact]
    public void GetReachableTiles_NavigatesAroundObstacles()
    {
        // Direct path (0,0)→(1,0) blocked. Detour: (0,0)→(0,1)→(1,1)→(2,1)→(2,0) = 4 steps.
        var map = new EncounterMap(10, 10);
        map.SetTile(1, 0, EncounterTileType.Obstacle);
        var state = new EncounterState(map);
        var merc = new Mercenary(stamina: 6); // 6 MP — enough for 4-step detour
        state.PlaceCreature(merc, 0, 0);
        var reachable = MovementValidator.GetReachableTiles(merc, state);
        Assert.Contains((2, 0), reachable.Keys);
        Assert.Equal(4, reachable[(2, 0)]);
    }

    [Fact]
    public void GetReachableTiles_IncludesHazardTiles()
    {
        var map = new EncounterMap(10, 10);
        map.SetTile(1, 0, EncounterTileType.Hazard);
        var state = new EncounterState(map);
        var merc = new Mercenary(stamina: 0); // 3 MP
        state.PlaceCreature(merc, 0, 0);
        var reachable = MovementValidator.GetReachableTiles(merc, state);
        Assert.Contains((1, 0), reachable.Keys);
    }

    [Fact]
    public void GetReachableTiles_RecordsMinimumCost()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        var merc = new Mercenary(stamina: 6); // 6 MP
        state.PlaceCreature(merc, 0, 0);
        var reachable = MovementValidator.GetReachableTiles(merc, state);
        Assert.Equal(1, reachable[(1, 0)]);
        Assert.Equal(2, reachable[(2, 0)]);
        Assert.Equal(3, reachable[(3, 0)]);
    }
}
