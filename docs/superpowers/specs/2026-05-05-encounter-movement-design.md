# Encounter Movement System Design

**Date:** 2026-05-05
**Branch:** StatusEffects

---

## Overview

Add grid-based movement for individual creatures on the encounter map. Every creature (mercenary or enemy) occupies a single 25×25px tile. No two creatures may share a tile. Movement costs 1 movement point per tile. Turns follow a 4-phase loop.

---

## Architecture

Three new classes in `Scripts/Encounter/`:

| Class | Responsibility |
|---|---|
| `EncounterState` | Owns creature positions, occupancy, and movement execution |
| `TurnManager` | Owns the 4-phase turn loop and tracks who has moved |
| `MovementValidator` | Computes reachable tiles via BFS |

One new MonoGame class in `Scripts/Rendering/` (or similar):

| Class | Responsibility |
|---|---|
| `EncounterScene` | Renders map + creatures, handles mouse input |

No changes to `Creature`. No changes to `EncounterMap`.

---

## EncounterState

```
EncounterState
├── EncounterMap Map
├── Dictionary<(int X, int Y), Creature>  OccupancyMap
├── Dictionary<Creature, (int X, int Y)>  CreaturePositions
├── Dictionary<Creature, float>           RemainingMovement
├── List<Creature>                        Mercenaries
└── List<Creature>                        Enemies
```

### Methods

- `PlaceCreature(Creature, int x, int y)` — validates tile is empty and not an Obstacle; adds to both dictionaries
- `MoveCreature(Creature, int toX, int toY)` — validates via BFS that destination is reachable; updates both dictionaries; deducts movement cost from `RemainingMovement`
- `GetPosition(Creature)` → `(int X, int Y)`
- `GetCreatureAt(int x, int y)` → `Creature?`
- `IsOccupied(int x, int y)` → `bool`
- `ResetMovement(Creature)` — copies `CombatStats.MovementPoints` into `RemainingMovement` at the start of a creature's turn

`Creature` is never mutated by movement — all encounter-specific state lives in `EncounterState`.

---

## TurnManager

### Phase Order

```
Phase 1 → First ceil(n/2) mercenaries     (player chooses order)
Phase 2 → First ceil(n/2) enemies         (AI)
Phase 3 → Remaining mercenaries           (player chooses order)
Phase 4 → Remaining enemies               (AI)
→ Repeat
```

Odd rosters round up for the first half: 3 mercs → 2 in phase 1, 1 in phase 3.

### State

- `CurrentPhase` (1–4)
- `HashSet<Creature> MovedThisTurn` — creatures that have already moved in the current phase
- `List<Creature> PhaseQueue` — creatures eligible to move in the current phase

### Methods

- `StartEncounter()` — builds initial phase queues, resets all movement
- `CanMove(Creature)` → `bool` — creature is in the current phase queue and has not yet moved
- `EndCreatureTurn(Creature)` — marks creature as moved; if phase queue exhausted, calls `AdvancePhase()`
- `AdvancePhase()` — increments `CurrentPhase` (wraps 4→1), rebuilds `PhaseQueue`, resets `MovedThisTurn`
- `CheckEndCondition()` → `EncounterResult` (`Victory` / `Defeat` / `Fled` / `Ongoing`)

---

## MovementValidator

Static class. Single public method:

```csharp
public static HashSet<(int X, int Y)> GetReachableTiles(Creature creature, EncounterState state)
```

### BFS Rules

- Starts from the creature's current position
- Expands one tile at a time in 4 directions (up, down, left, right)
- Each step costs 1 movement point
- Stops when accumulated cost exceeds `state.RemainingMovement[creature]`
- Skips tiles that are: out of bounds, `EncounterTileType.Obstacle`, or occupied by another creature
- `Hazard` tiles are walkable at normal cost (damage is a separate system)
- Returns every tile the creature can land on

BFS is used for both UI highlighting and move validation. There is no Manhattan distance pre-check.

---

## EncounterScene (MonoGame)

### Rendering (drawn each frame, in order)

1. **Terrain** — 25×25px `Rectangle` per tile, colored by `EncounterTileType`:
   - `Empty` → grey
   - `Obstacle` → dark grey / black
   - `Hazard` → orange
2. **Reachable highlights** — semi-transparent overlay on BFS result when a creature is selected
3. **Creatures** — colored 25×25px square drawn at `(X * 25, Y * 25)`, on top of their tile (placeholder until art arrives)

### Mouse Input Flow

1. Player clicks → convert: `gridX = mouseX / 25`, `gridY = mouseY / 25`
2. **No selection:** if `GetCreatureAt(gridX, gridY)` returns a friendly creature and `TurnManager.CanMove(creature)` is true → select it, run BFS, highlight reachable tiles
3. **Creature selected, click on highlighted tile:** call `EncounterState.MoveCreature`, then `TurnManager.EndCreatureTurn`, deselect
4. **Creature selected, click on non-highlighted tile:** deselect, clear highlights

`EncounterScene` is not unit tested (GPU/window dependency). It receives manual testing only.

---

## Testing Strategy (TDD)

All logic classes get tests written before implementation.

### EncounterStateTests

- `PlaceCreature` rejects an occupied tile
- `PlaceCreature` rejects an `Obstacle` tile
- `MoveCreature` updates both dictionaries correctly
- `MoveCreature` rejects an occupied destination
- `MoveCreature` rejects an `Obstacle` destination
- `MoveCreature` deducts the correct movement cost
- `GetCreatureAt` returns null for an empty tile
- `IsOccupied` returns correct results

### TurnManagerTests

- Phase 1 queue contains `ceil(n/2)` mercenaries
- Phase 3 queue contains the remaining mercenaries
- `CanMove` returns false for a creature in the wrong phase
- `CanMove` returns false for a creature that already moved this phase
- `EndCreatureTurn` advances the phase when the queue is exhausted
- Odd rosters split correctly (3 mercs → 2 then 1)

### MovementValidatorTests

- Returns empty set when creature has 0 movement points
- Does not include `Obstacle` tiles
- Does not include occupied tiles
- Correctly navigates around obstacles to find reachable tiles
- Stops at the movement point boundary
- Includes `Hazard` tiles as reachable

---

## File Layout

```
Scripts/
  Encounter/
    EncounterState.cs
    TurnManager.cs
    MovementValidator.cs
    EncounterResult.cs     (enum: Victory, Defeat, Fled, Ongoing)
  Rendering/
    EncounterScene.cs

Tests/
  EncounterTests/
    EncounterStateTests.cs
    TurnManagerTests.cs
    MovementValidatorTests.cs
```
