using RuinGamePDT.Creatures;
using RuinGamePDT.Encounter;
using RuinGamePDT.World;

namespace RuinGamePDT.Tests;

public class TurnManagerTests
{
    private static EncounterState MakeState(int mercCount, int enemyCount)
    {
        var state = new EncounterState(new EncounterMap(20, 20));
        for (int i = 0; i < mercCount; i++)
        {
            var m = new Mercenary();
            state.Mercenaries.Add(m);
            state.PlaceCreature(m, i, 0);
        }
        for (int i = 0; i < enemyCount; i++)
        {
            var e = new PricklebackGoblin();
            state.Enemies.Add(e);
            state.PlaceCreature(e, i, 1);
        }
        return state;
    }

    [Fact]
    public void StartEncounter_Phase1_ContainsCeilHalfOfMercenaries()
    {
        var state = MakeState(mercCount: 3, enemyCount: 1);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        int canMove = state.Mercenaries.Count(m => tm.CanMove(m));
        Assert.Equal(2, canMove); // ceil(3/2) = 2
    }

    [Fact]
    public void StartEncounter_EvenSplit_Phase1ContainsHalfOfMercenaries()
    {
        var state = MakeState(mercCount: 4, enemyCount: 1);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        int canMove = state.Mercenaries.Count(m => tm.CanMove(m));
        Assert.Equal(2, canMove); // 4/2 = 2
    }

    [Fact]
    public void CanMove_ReturnsFalse_ForMercenaryInPhase3_WhenPhase1Active()
    {
        var state = MakeState(mercCount: 3, enemyCount: 1);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        // The 3rd mercenary is in phase 3, not phase 1
        var phase3Merc = state.Mercenaries.Last();
        Assert.False(tm.CanMove(phase3Merc));
    }

    [Fact]
    public void CanMove_ReturnsFalse_ForMercenaryThatAlreadyMoved()
    {
        var state = MakeState(mercCount: 2, enemyCount: 1);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        var merc = state.Mercenaries.First();
        tm.EndCreatureTurn(merc);
        Assert.False(tm.CanMove(merc));
    }

    [Fact]
    public void EndCreatureTurn_AdvancesPhase_WhenQueueExhausted()
    {
        var state = MakeState(mercCount: 1, enemyCount: 1);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        Assert.Equal(1, tm.CurrentPhase);
        tm.EndCreatureTurn(state.Mercenaries[0]); // phase 1 queue exhausted → advance
        Assert.Equal(2, tm.CurrentPhase);
    }

    [Fact]
    public void EndCreatureTurn_DoesNotAdvancePhase_WhenOtherCreaturesRemain()
    {
        var state = MakeState(mercCount: 4, enemyCount: 1);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        tm.EndCreatureTurn(state.Mercenaries[0]); // only 1 of 2 phase-1 mercs moved
        Assert.Equal(1, tm.CurrentPhase);
    }

    [Fact]
    public void Phase3_ContainsRemainingMercenaries()
    {
        var state = MakeState(mercCount: 3, enemyCount: 1);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        // Exhaust phase 1 (2 mercs) and phase 2 (1 enemy) to reach phase 3
        foreach (var m in state.Mercenaries.Take(2)) tm.EndCreatureTurn(m);
        foreach (var e in state.Enemies.Take(1)) tm.EndCreatureTurn(e);
        Assert.Equal(3, tm.CurrentPhase);
        int canMove = state.Mercenaries.Count(m => tm.CanMove(m));
        Assert.Equal(1, canMove); // 1 remaining merc in phase 3
    }

    [Fact]
    public void CheckEndCondition_ReturnsVictory_WhenEnemiesListIsEmpty()
    {
        var state = MakeState(mercCount: 1, enemyCount: 0);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        Assert.Equal(EncounterResult.Victory, tm.CheckEndCondition());
    }

    [Fact]
    public void CheckEndCondition_ReturnsDefeat_WhenMercenariesListIsEmpty()
    {
        var state = MakeState(mercCount: 0, enemyCount: 1);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        Assert.Equal(EncounterResult.Defeat, tm.CheckEndCondition());
    }

    [Fact]
    public void CheckEndCondition_ReturnsOngoing_WhenBothSidesHaveCreatures()
    {
        var state = MakeState(mercCount: 2, enemyCount: 2);
        var tm = new TurnManager(state);
        tm.StartEncounter();
        Assert.Equal(EncounterResult.Ongoing, tm.CheckEndCondition());
    }
}
