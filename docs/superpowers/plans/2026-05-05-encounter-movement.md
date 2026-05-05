# Encounter Movement System Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add grid-based creature movement to the encounter map — individual creatures occupy tiles, move by spending movement points, and take turns in a 4-phase loop driven by `EncounterState`, `TurnManager`, and `MovementValidator`.

**Architecture:** `EncounterState` is the single source of truth for creature positions and occupancy (two dictionaries, no position fields on `Creature`). `TurnManager` drives a 4-phase loop (half-mercs → half-enemies → rest-mercs → rest-enemies). `MovementValidator` computes reachable tiles via BFS. MonoGame's `EncounterScene` renders the 50×50 grid at 25px/tile and handles mouse input.

**Tech Stack:** C# 12 / .NET 8, xUnit 2.5, MonoGame.Framework.DesktopGL

---

## File Map

| Action | Path | Responsibility |
|--------|------|----------------|
| Create | `Scripts/Encounter/EncounterResult.cs` | Enum: Ongoing, Victory, Defeat, Fled |
| Create | `Scripts/Encounter/EncounterState.cs` | Creature positions, occupancy, movement execution |
| Create | `Scripts/Encounter/MovementValidator.cs` | BFS reachability |
| Create | `Scripts/Encounter/TurnManager.cs` | 4-phase turn loop |
| Create | `Scripts/Rendering/EncounterScene.cs` | MonoGame rendering + mouse input |
| Create | `Program.cs` | MonoGame entry point |
| Create | `Game1.cs` | MonoGame Game subclass |
| Modify | `RuinGamePDT.csproj` | Add MonoGame package |
| Modify | `Tests/RuinGamePDT.Tests.csproj` | Add Compile includes for new logic files |
| Create | `Tests/EncounterTests/EncounterStateTests.cs` | Unit tests for EncounterState |
| Create | `Tests/EncounterTests/MovementValidatorTests.cs` | Unit tests for MovementValidator |
| Create | `Tests/EncounterTests/TurnManagerTests.cs` | Unit tests for TurnManager |

---

## Task 1: EncounterResult Enum

**Files:**
- Create: `Scripts/Encounter/EncounterResult.cs`

- [ ] **Step 1: Create the file**

```csharp
namespace RuinGamePDT.Encounter;

public enum EncounterResult { Ongoing, Victory, Defeat, Fled }
```

- [ ] **Step 2: Verify the project builds**

Run: `dotnet build RuinGamePDT.csproj`
Expected: Build succeeded, 0 errors

- [ ] **Step 3: Commit**

```bash
git add Scripts/Encounter/EncounterResult.cs
git commit -m "feat: add EncounterResult enum"
```

---

## Task 2: EncounterState — Placement

**Files:**
- Create: `Scripts/Encounter/EncounterState.cs`
- Create: `Tests/EncounterTests/EncounterStateTests.cs`
- Modify: `Tests/RuinGamePDT.Tests.csproj`

- [ ] **Step 1: Add Compile includes to the test project**

In `Tests/RuinGamePDT.Tests.csproj`, inside the last `<ItemGroup>` that has `<Compile Include>` entries, add:

```xml
<Compile Include="..\Scripts\Encounter\EncounterResult.cs" />
<Compile Include="..\Scripts\Encounter\EncounterState.cs" />
<Compile Include="..\Scripts\Encounter\MovementValidator.cs" />
<Compile Include="..\Scripts\Encounter\TurnManager.cs" />
```

Note: `MovementValidator.cs` and `TurnManager.cs` don't exist yet but the test project needs to know about them. Add all four includes now so you don't need to return to this file.

- [ ] **Step 2: Write failing tests**

Create `Tests/EncounterTests/EncounterStateTests.cs`:

```csharp
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
```

- [ ] **Step 3: Run tests to confirm they fail**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj --filter "FullyQualifiedName~EncounterStateTests"`
Expected: Build error — `EncounterState` does not exist

- [ ] **Step 4: Implement EncounterState (placement only)**

Create `Scripts/Encounter/EncounterState.cs`:

```csharp
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
```

- [ ] **Step 5: Run tests to confirm they pass**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj --filter "FullyQualifiedName~EncounterStateTests"`
Expected: All 9 tests pass

- [ ] **Step 6: Commit**

```bash
git add Scripts/Encounter/EncounterState.cs Tests/EncounterTests/EncounterStateTests.cs Tests/RuinGamePDT.Tests.csproj
git commit -m "feat: add EncounterState placement and occupancy"
```

---

## Task 3: MovementValidator — BFS

**Files:**
- Create: `Scripts/Encounter/MovementValidator.cs`
- Create: `Tests/EncounterTests/MovementValidatorTests.cs`

- [ ] **Step 1: Write failing tests**

Create `Tests/EncounterTests/MovementValidatorTests.cs`:

```csharp
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
```

- [ ] **Step 2: Run tests to confirm they fail**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj --filter "FullyQualifiedName~MovementValidatorTests"`
Expected: Build error — `MovementValidator` does not exist

- [ ] **Step 3: Implement MovementValidator**

Create `Scripts/Encounter/MovementValidator.cs`:

```csharp
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
```

- [ ] **Step 4: Run tests to confirm they pass**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj --filter "FullyQualifiedName~MovementValidatorTests"`
Expected: All 6 tests pass

- [ ] **Step 5: Commit**

```bash
git add Scripts/Encounter/MovementValidator.cs Tests/EncounterTests/MovementValidatorTests.cs
git commit -m "feat: add MovementValidator BFS"
```

---

## Task 4: EncounterState — MoveCreature

**Files:**
- Modify: `Scripts/Encounter/EncounterState.cs`
- Modify: `Tests/EncounterTests/EncounterStateTests.cs`

- [ ] **Step 1: Add failing tests to EncounterStateTests.cs**

Append these tests inside the `EncounterStateTests` class in `Tests/EncounterTests/EncounterStateTests.cs`:

```csharp
    [Fact]
    public void MoveCreature_ReturnsTrue_AndUpdatesBothDictionaries()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        var merc = new Mercenary(stamina: 4); // 5 MP
        state.PlaceCreature(merc, 0, 0);
        Assert.True(state.MoveCreature(merc, 1, 0));
        Assert.Equal(merc, state.GetCreatureAt(1, 0));
        Assert.Null(state.GetCreatureAt(0, 0));
        Assert.Equal((1, 0), state.GetPosition(merc));
    }

    [Fact]
    public void MoveCreature_DeductsCorrectMovementCost()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        var merc = new Mercenary(stamina: 4); // MovementPoints = MathF.Round(2+3) = 5
        state.PlaceCreature(merc, 0, 0);
        state.MoveCreature(merc, 2, 0); // 2 steps
        Assert.Equal(3f, state.GetRemainingMovement(merc));
    }

    [Fact]
    public void MoveCreature_ReturnsFalse_WhenDestinationIsOccupied()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        var merc1 = new Mercenary(stamina: 4);
        var merc2 = new Mercenary(stamina: 4);
        state.PlaceCreature(merc1, 0, 0);
        state.PlaceCreature(merc2, 1, 0);
        Assert.False(state.MoveCreature(merc1, 1, 0));
    }

    [Fact]
    public void MoveCreature_ReturnsFalse_WhenDestinationIsObstacle()
    {
        var map = new EncounterMap(10, 10);
        map.SetTile(1, 0, EncounterTileType.Obstacle);
        var state = new EncounterState(map);
        var merc = new Mercenary(stamina: 4);
        state.PlaceCreature(merc, 0, 0);
        Assert.False(state.MoveCreature(merc, 1, 0));
    }

    [Fact]
    public void MoveCreature_ReturnsFalse_WhenDestinationOutOfMovementRange()
    {
        var state = new EncounterState(new EncounterMap(10, 10));
        var merc = new Mercenary(stamina: 0); // 3 MP
        state.PlaceCreature(merc, 0, 0);
        Assert.False(state.MoveCreature(merc, 0, 9)); // 9 steps away
    }
```

- [ ] **Step 2: Run tests to confirm they fail**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj --filter "FullyQualifiedName~EncounterStateTests.MoveCreature"`
Expected: Build error — `MoveCreature` does not exist on `EncounterState`

- [ ] **Step 3: Add MoveCreature to EncounterState**

Add this method to the `EncounterState` class in `Scripts/Encounter/EncounterState.cs`, after `ResetMovement`:

```csharp
    public bool MoveCreature(Creature creature, int toX, int toY)
    {
        if (!_positions.ContainsKey(creature)) return false;
        var reachable = MovementValidator.GetReachableTiles(creature, this);
        if (!reachable.ContainsKey((toX, toY))) return false;
        var from = _positions[creature];
        _occupancy.Remove(from);
        _occupancy[(toX, toY)] = creature;
        _positions[creature] = (toX, toY);
        _remainingMovement[creature] -= reachable[(toX, toY)];
        return true;
    }
```

- [ ] **Step 4: Run tests to confirm they pass**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj --filter "FullyQualifiedName~EncounterStateTests"`
Expected: All 14 tests pass

- [ ] **Step 5: Commit**

```bash
git add Scripts/Encounter/EncounterState.cs Tests/EncounterTests/EncounterStateTests.cs
git commit -m "feat: add EncounterState.MoveCreature with BFS validation"
```

---

## Task 5: TurnManager

**Files:**
- Create: `Scripts/Encounter/TurnManager.cs`
- Create: `Tests/EncounterTests/TurnManagerTests.cs`

- [ ] **Step 1: Write failing tests**

Create `Tests/EncounterTests/TurnManagerTests.cs`:

```csharp
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
```

- [ ] **Step 2: Run tests to confirm they fail**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj --filter "FullyQualifiedName~TurnManagerTests"`
Expected: Build error — `TurnManager` does not exist

- [ ] **Step 3: Implement TurnManager**

Create `Scripts/Encounter/TurnManager.cs`:

```csharp
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
```

- [ ] **Step 4: Run tests to confirm they pass**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj --filter "FullyQualifiedName~TurnManagerTests"`
Expected: All 10 tests pass

- [ ] **Step 5: Run all tests to confirm nothing regressed**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj`
Expected: All tests pass

- [ ] **Step 6: Commit**

```bash
git add Scripts/Encounter/TurnManager.cs Tests/EncounterTests/TurnManagerTests.cs
git commit -m "feat: add TurnManager with 4-phase turn loop"
```

---

## Task 6: MonoGame Setup + Game1

**Files:**
- Modify: `RuinGamePDT.csproj`
- Create: `Program.cs`
- Create: `Game1.cs`

No unit tests — MonoGame classes require a GPU/window and are tested manually.

- [ ] **Step 1: Add MonoGame to the main project**

Run: `dotnet add RuinGamePDT.csproj package MonoGame.Framework.DesktopGL`
Expected: Package added successfully

- [ ] **Step 2: Set the output type to WinExe**

In `RuinGamePDT.csproj`, inside the `<PropertyGroup>`, add:

```xml
<OutputType>WinExe</OutputType>
```

- [ ] **Step 3: Create Program.cs**

```csharp
using var game = new RuinGamePDT.Game1();
game.Run();
```

- [ ] **Step 4: Create Game1.cs**

```csharp
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
        _encounterState.PlaceCreature(enemy, 49, 49);

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
```

- [ ] **Step 5: Verify the project builds**

Run: `dotnet build RuinGamePDT.csproj`
Expected: Build succeeded, 0 errors (EncounterScene missing is expected at this stage — fix by creating it next)

Note: The build will fail until Task 7 creates `EncounterScene`. That is expected. Proceed to Task 7.

- [ ] **Step 6: Commit**

```bash
git add RuinGamePDT.csproj Program.cs Game1.cs
git commit -m "feat: add MonoGame entry point and Game1 scaffold"
```

---

## Task 7: EncounterScene

**Files:**
- Create: `Scripts/Rendering/EncounterScene.cs`

No unit tests — rendering and input depend on a live MonoGame window. Test manually.

- [ ] **Step 1: Create EncounterScene**

Create `Scripts/Rendering/EncounterScene.cs`:

```csharp
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
```

- [ ] **Step 2: Build the main project**

Run: `dotnet build RuinGamePDT.csproj`
Expected: Build succeeded, 0 errors

- [ ] **Step 3: Run all tests to confirm nothing regressed**

Run: `dotnet test Tests/RuinGamePDT.Tests.csproj`
Expected: All tests pass

- [ ] **Step 4: Manual test — launch the game**

Run: `dotnet run --project RuinGamePDT.csproj`

Verify:
- A 1250×1250 window opens showing the encounter map
- Terrain tiles are visible (grey=empty, dark=obstacle, orange=hazard)
- Two blue squares (mercenaries) appear at top-left
- One red square (enemy) appears at bottom-right
- Clicking a blue square highlights reachable tiles in yellow
- Clicking a highlighted tile moves the mercenary there
- After both phase-1 mercenaries move, phase advances (enemy skips, then phase 3 begins)
- Clicking the wrong mercenary for the current phase does nothing

- [ ] **Step 5: Commit**

```bash
git add Scripts/Rendering/EncounterScene.cs
git commit -m "feat: add EncounterScene MonoGame rendering and mouse input"
```
