using RuinGamePDT.Creatures;
using RuinGamePDT.Encounter;
using RuinGamePDT.Resources;
using RuinGamePDT.World;

namespace RuinGamePDT.Tests;

public class EncounterStateTests
{
    [Fact]
    public void PlaceCreature_ReturnsTrue_WhenTileIsEmpty()
    {
        var map = new EncounterMap(10, 10);
        var state = new EncounterState(map);
        var merc = new Mercenary();
        Assert.True(state.PlaceCreature(merc, 0, 0));
    }

    [Fact]
    public void PlaceCreature_ReturnsFalse_WhenTileIsObstacle()
    {
        var map = new EncounterMap(10, 10);
        map.SetTile(2, 2, EncounterTileType.Obstacle);
        var state = new EncounterState(map);
        Assert.False(state.PlaceCreature(new Mercenary(), 2, 2));
    }

    [Fact]
    public void PlaceCreature_ReturnsFalse_WhenTileIsOccupied()
    {
        var map = new EncounterMap(10, 10);
        var state = new EncounterState(map);
        state.PlaceCreature(new Mercenary(), 3, 3);
        Assert.False(state.PlaceCreature(new Mercenary(), 3, 3));
    }

    [Fact]
    public void GetCreatureAt_ReturnsNull_WhenTileIsEmpty()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        Assert.Null(state.GetCreatureAt(0, 0));
    }

    [Fact]
    public void GetCreatureAt_ReturnsCreature_AfterPlacement()
    {
        var map = new EncounterMap(10, 10);
        var state = new EncounterState(map);
        var merc = new Mercenary();
        state.PlaceCreature(merc, 1, 1);
        Assert.Equal(merc, state.GetCreatureAt(1, 1));
    }

    [Fact]
    public void IsOccupied_ReturnsFalse_WhenEmpty()
    {
        Assert.False(new EncounterState(new EncounterMap(10, 10)).IsOccupied(0, 0));
    }

    [Fact]
    public void IsOccupied_ReturnsTrue_AfterPlacement()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        state.PlaceCreature(new Mercenary(), 4, 4);
        Assert.True(state.IsOccupied(4, 4));
    }

    [Fact]
    public void GetPosition_ReturnsPlacedCoordinates()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        var merc = new Mercenary();
        state.PlaceCreature(merc, 3, 7);
        Assert.Equal((3, 7), state.GetPosition(merc));
    }

    [Fact]
    public void GetRemainingMovement_EqualsMovementPoints_AfterPlacement()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        var merc = new Mercenary(stamina: 4); // MovementPoints = MathF.Round(4*0.5+3) = 5
        state.PlaceCreature(merc, 0, 0);
        Assert.Equal(5f, state.GetRemainingMovement(merc));
    }
}
