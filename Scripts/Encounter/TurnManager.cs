using RuinGamePDT.Creatures;

namespace RuinGamePDT.Encounter;

public class TurnManager(EncounterState state)
{
    public int CurrentPhase { get; private set; }

    private readonly HashSet<Creature> _movedThisTurn = [];
    private List<Creature> _phaseQueue = [];

    public void StartEncounter()
    {
        CurrentPhase = 1;
        _movedThisTurn.Clear();
        _phaseQueue = BuildPhaseQueue(1);
        foreach (var c in _phaseQueue) state.ResetMovement(c);
    }

    public bool CanMove(Creature creature) =>
        _phaseQueue.Contains(creature) && !_movedThisTurn.Contains(creature);

    public void EndCreatureTurn(Creature creature)
    {
        _movedThisTurn.Add(creature);
        if (_phaseQueue.All(_movedThisTurn.Contains))
            AdvancePhase();
    }

    public void AdvancePhase()
    {
        CurrentPhase = CurrentPhase == 4 ? 1 : CurrentPhase + 1;
        _movedThisTurn.Clear();
        _phaseQueue = BuildPhaseQueue(CurrentPhase);
        foreach (var c in _phaseQueue) state.ResetMovement(c);
    }

    public EncounterResult CheckEndCondition()
    {
        if (state.Enemies.Count == 0) return EncounterResult.Victory;
        if (state.Mercenaries.Count == 0) return EncounterResult.Defeat;
        return EncounterResult.Ongoing;
    }

    private List<Creature> BuildPhaseQueue(int phase)
    {
        int mercHalf = (int)Math.Ceiling(state.Mercenaries.Count / 2.0);
        int enemyHalf = (int)Math.Ceiling(state.Enemies.Count / 2.0);
        return phase switch
        {
            1 => state.Mercenaries.Take(mercHalf).ToList(),
            2 => state.Enemies.Take(enemyHalf).ToList(),
            3 => state.Mercenaries.Skip(mercHalf).ToList(),
            4 => state.Enemies.Skip(enemyHalf).ToList(),
            _ => []
        };
    }
}
