# Enemy AI Slice Design

**Date:** 2026-05-12
**Branch:** Combat

---

## Overview

First playable enemy turn. During enemy phases (2 and 4), each enemy moves toward the closest mercenary and attacks if any in-range targets exist. The slice replaces the placeholder auto-skip in `Game1.Update` with a deterministic AI driver that uses the existing `MovementValidator` and `CombatResolver`. No new combat machinery — only orchestration.

---

## 1. Decision flow

`EnemyAI.TakeTurn(Creature enemy, EncounterState state)` runs once per enemy whose phase is active.

1. **Guard rails.** If `enemy` is no longer placed (died from a Bleed tick or earlier attack this phase), or if `state.Mercenaries` is empty, return immediately.
2. **Find target.** Pick the mercenary with the smallest Chebyshev distance to the enemy's current tile. Ties broken by list order (first in `state.Mercenaries`).
3. **Move toward target.**
   - Compute `MovementValidator.GetReachableTiles(enemy, state)`.
   - From that set (plus the enemy's current tile, since staying is a valid "move"), pick the tile with the minimum Chebyshev distance to the target merc.
   - If the chosen tile differs from current position, call `state.MoveCreature(enemy, ...)`. Movement points are deducted normally.
4. **Attack loop.**
   - Repeat:
     - For each `attack` in `enemy.Attacks`, find the best target tile such that:
       - `state.GetRemainingActionPoints(enemy) >= attack.ActionPointCost`
       - `resolver.IsInRange(enemy, attack, tile, state)` returns true
       - For single-target shapes: `tile` contains a mercenary
       - For AOE shapes: at least one mercenary lies at `tile + offset` for some offset in `AttackShape`
     - Among all `(attack, tile)` candidates, pick the one with the highest `(MinDamage + MaxDamage) / 2`. Ties: first attack found.
     - If no candidate exists → break.
     - Otherwise `resolver.Resolve(enemy, attack, tile, state)`.
     - After resolve, re-check the guard rails (enemy could die from a future reaction, mercenaries list could be empty after a fatal hit).
5. **End.** No call to `EndCreatureTurn` from inside `TakeTurn` — that stays in the caller (`Game1`) for symmetry with the player flow.

### Target tile selection (per attack)

- **Single-target** (`AttackShape == [(0,0)]`): iterate over in-range tiles that contain a mercenary; pick the merc with the lowest current HP (focus-fire). Ties: closest by Chebyshev. Final ties: list order.
- **AOE**: iterate over every in-range tile; for each, count how many mercs land on `tile + offset` for some offset. Pick the tile that catches the most. Ties: list order of tiles.

### Distance metric

Chebyshev throughout (matches `CombatResolver.IsInRange`). The same TODO for switching to circular Euclidean range applies — when that lands, AI targeting flips with it.

---

## 2. New code

| Action | Path | Responsibility |
|---|---|---|
| Create | `Scripts/Encounter/EnemyAI.cs` | Pure-logic AI driver. Ctor takes a `CombatResolver`. |
| Modify | `Game1.cs` | Replace the phase-2/4 auto-skip with `_ai.TakeTurn(enemy, state)`. Instantiate `_ai` alongside `_scene`, sharing the same `CombatResolver`. |
| Create | `Tests/EncounterTests/EnemyAITests.cs` | Unit tests for AI decision flow. |

### `EnemyAI` shape

```csharp
public class EnemyAI(CombatResolver resolver)
{
    public void TakeTurn(Creature enemy, EncounterState state) { /* ... */ }
}
```

The resolver carries its own RNG (Func<int,int,int>), so tests can seed it as they do for `CombatResolverTests`. No additional injection points.

---

## 3. Tests

`EnemyAITests` uses the same `Rolls(...)` queue-RNG helper as `CombatResolverTests`. Each test sets up a known scenario, calls `TakeTurn`, and asserts the resulting state.

Coverage:

- **MovesTowardNearestOfMultipleMercs** — goblin equidistant from two mercs picks one and moves; verify it ends closer to that one.
- **DoesNotMoveIfAlreadyAdjacent** — when the closest merc is already at the goblin's optimal attacking tile, the goblin doesn't move (or moves to a tile that doesn't increase distance).
- **AttacksWhenInRange** — if Scratch is in range, goblin attacks rather than spending the whole turn moving.
- **PicksHighestAverageDamageAttack** — with all three goblin attacks in range and full AP, the goblin chooses Skewer (avg 3) over Scratch (avg 2) and Quill Spray (avg 0.5).
- **ChainsAttacksUntilAPExhausted** — Prickleback (4 AP) chains four Skewers until out of AP, given an immortal target dummy.
- **EndsCleanlyWhenNoMercInRangeAfterMove** — goblin moves as far as it can but no attack reaches; `TakeTurn` returns without errors.
- **StopsIfGoblinDies** — if the goblin's HP hits 0 mid-turn (e.g., from a Bleed tick wired up before the call), `TakeTurn` doesn't try to attack from a corpse.
- **StopsIfAllMercsDie** — after the last merc is removed by an attack, the loop exits.
- **AOEPicksTileCatchingMostMercs** — for Quill Spray (3-tile shape), the goblin picks the center tile that catches 2 mercs over a center that catches 1.

---

## 4. Integration in `Game1.cs`

```csharp
if (_turnManager.CurrentPhase == 2 || _turnManager.CurrentPhase == 4)
{
    foreach (var e in _encounterState.Enemies.Where(_turnManager.CanMove).ToList())
    {
        _ai.TakeTurn(e, _encounterState);
        _turnManager.EndCreatureTurn(e);
    }
}
```

All enemy actions resolve in a single frame. No staggering, no animation, no delay — the player will simply see HP bars drop and possibly the enemy ending up in a new tile when the next frame draws. Adding a staggered playback is a separate UX pass.

The `CombatResolver` instance is now shared between `EncounterScene` and `EnemyAI` so RNG state is consistent (single Random source) — `Game1` owns it and passes it to both.

---

## 5. Spec notes (deferred — no code change in this slice)

These two items are *intentional design decisions* for a future slice. Capturing them here so they don't get lost.

### Note A — Enemy AP default = 1

All non-special enemies should have exactly 1 AP per turn regardless of their `Agility`. Currently `CombatStats.ActionPoints = b.Agility / 2 + 1` applies uniformly to mercs and enemies; PricklebackGoblin (Agility 6) rolls 4 AP per turn and will feel overtuned. Fix path: add a virtual `MaxActionPoints` property on `Creature` (default returns `CombatStats.ActionPoints`), overridden in enemy subclasses to return 1 (or a higher number for "special" enemies).

### Note B — Enemy attack `ActionPointCost` → cooldown semantics

For enemies, `Attack.ActionPointCost` should be reinterpreted as a per-attack **cooldown counter**. When an enemy uses an attack, it goes on cooldown for `ActionPointCost` rounds. The AI then picks the most damaging attack that is not currently on cooldown. Cooldown tracking lives on `EncounterState` (e.g., `Dictionary<(Creature, Attack), int>`) and decrements once per round.

This changes the AI's selection criterion in step 4 above from "AP-affordable + in-range" to "off-cooldown + in-range."

---

## 6. Out of scope

- Smarter pathfinding. Current model is greedy (one-tile lookahead via reachable set), good enough for an open map; will struggle with concave obstacle layouts.
- AI prioritization beyond "closest merc, focus-fire by HP." No threat assessment, status-effect awareness, or kiting.
- Staggered / animated enemy turns. All enemy actions resolve instantly in a single frame.
- Enemy cooldown system itself (deferred per Note B).
- Per-enemy AP override (deferred per Note A).
- Reactions (`Reaction` field on `Attack` — Parry's counter still doesn't fire on miss).
