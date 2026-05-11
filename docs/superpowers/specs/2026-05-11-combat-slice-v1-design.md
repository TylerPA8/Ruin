# Combat Slice v1 Design

**Date:** 2026-05-11
**Branch:** Combat

---

## Overview

First playable combat slice. Mercenaries get an action-point pool, can select an attack, target an enemy, and resolve the full damage path (accuracy roll → hit/miss → crit roll → damage with AttackPower/PhysicalDefense modifiers → status effects → death). Status effects tick on each creature's turn. Goblin AI is still out of scope and continues to auto-skip.

The slice combines what was originally split as items 1 (selection/targeting) and 2 (resolution) — the full damage path was pulled in so playtesting actually reads as combat rather than empty AP decrement.

---

## 1. Action Point system

### Formula change

`CombatStats.ActionPoints` changes from `b.Agility * 0.25f` (float, fractional) to `b.Agility / 2 + 1` (int).

| Agility | AP/turn |
|---|---|
| 0 | 1 |
| 1 | 1 |
| 5 | 3 |
| 10 | 6 |

The field type changes from `float` to `int`. `RaiseCombatStat` and `LowerCombatStat` updates are required (the cases for `ActionPoints` will need to cast or be reworked).

### Encounter state

`EncounterState` gains a `Dictionary<Creature, int> _remainingActionPoints` parallel to `_remainingMovement`, plus:

- `int GetRemainingActionPoints(Creature)`
- `void SpendActionPoints(Creature, int amount)`
- `void ResetActionPoints(Creature)` — copies `CombatStats.ActionPoints` into the dictionary

`PlaceCreature` initializes `_remainingActionPoints[creature] = creature.CombatStats.ActionPoints` (mirroring movement init).

### Turn flow

`TurnManager.AdvancePhase` and `StartEncounter`, when building a new phase queue, additionally call `state.ResetActionPoints(c)` for each creature in the new queue.

Movement and AP are independent pools. A mercenary can spend their full `MovementPoints` *and* their full AP in the same turn.

---

## 2. Status effect ticking

`Creature.TickStatusEffects()` exists today but is never called. It will now run on every creature in a new phase queue, at the moment AP and movement reset. Effects apply their per-tick damage / stat reduction and decrement their `Duration`; expired effects fall out of `StatusEffects`.

Wiring location: `TurnManager` calls `creature.TickStatusEffects()` for each `c` in the new phase queue, after `ResetActionPoints(c)` / `ResetMovement(c)`.

---

## 3. Targeting

### Distance metric

For this slice: **Chebyshev** — `max(|dx|, |dy|)`. A range-15 attack reaches any tile within a 15×15 box centered on the caster.

> **TODO (post-slice):** switch to a circular range — Euclidean `sqrt(dx² + dy²) ≤ Range`, which produces the "10 square radius around the target" shape (roughly circular) instead of a square box. Will move into `CombatResolver.IsInRange` and the `EncounterScene` targeting overlay together. Tracked in the source as `// TODO(distance):` comments at both sites.

### Target tile + AttackShape

The tile the player clicks is the **target tile**. The attack's `AttackShape` offsets are applied *relative to the target tile* to determine which tiles take damage / get status effects. `Range` and `MinRange` measure the target tile, not the affected tiles.

- **Single-target** (shape == `[(0,0)]`): target tile must have a creature.
- **AOE** (shape has multiple offsets): any in-range tile is valid. The player may aim at empty tiles to position the AOE.

### Validity check

A tile is a valid target iff:
1. Within map bounds.
2. `attack.MinRange ≤ chebyshev(caster, tile) ≤ attack.Range`.
3. If single-target: `state.GetCreatureAt(tile) != null`.

---

## 4. Combat resolution

New class `Scripts/Encounter/CombatResolver.cs`. Pure logic, no MonoGame dependency, fully unit-testable. The class takes an injectable `Random` instance in the constructor so tests can seed deterministically.

```
Resolve(attacker, attack, targetTile, state):
  state.SpendActionPoints(attacker, attack.ActionPointCost)
  hitCount = rng.Next(attack.MinHits, attack.MaxHits + 1)

  for each offset in attack.AttackShape.Offsets:
    defender = state.GetCreatureAt(targetTile + offset)
    if defender == null: continue

    for i in 1..hitCount:
      // ── Hit roll ────────────────────────────────────────────
      hitThreshold = 100 - attack.Accuracy + defender.Evasion
      hitRoll = rng.Next(1, 101)
      if hitRoll < hitThreshold: continue           // miss

      // ── Crit roll ───────────────────────────────────────────
      isCrit = attack.AutoCrit
      if !isCrit:
        critRoll = rng.Next(1, 101)
        isCrit = critRoll >= 100 - attacker.CritChance

      // ── Damage ──────────────────────────────────────────────
      baseDmg = rng.Next(attack.MinDamage, attack.MaxDamage + 1)
      dmg = max(0, baseDmg + attacker.AttackPower - defender.PhysicalDefense)
      if isCrit: dmg *= 2
      defender.CurrentHp -= dmg

      // ── Effects ─────────────────────────────────────────────
      if attack.OnHit  != null:  defender.ApplyStatusEffect(attack.OnHit)
      if attack.OnCrit != null && isCrit: defender.ApplyStatusEffect(attack.OnCrit)

      // ── Death ───────────────────────────────────────────────
      if defender.CurrentHp <= 0:
        state.RemoveCreature(defender)
        break out of inner hit loop  // don't keep rolling against a dead target
```

### Formulas

| Roll | Formula |
|---|---|
| Hit | `roll ≥ 100 - Accuracy + Evasion` |
| Crit | `AutoCrit` OR `roll ≥ 100 - CritChance` |
| Damage | `max(0, RandomDmg + AttackPower - PhysicalDefense)`; doubled on crit |

Accuracy > 100 means the attack always hits at zero evasion (Suppression's 200 → threshold = -100 → always passes). Very high evasion can push the threshold above 100, making the attack impossible to land — accepted.

`CombatStats.Evasion`, `AttackPower`, `PhysicalDefense`, `CritChance` are all currently float — `CombatResolver` casts to int at use. (No formula change to those stats in this slice.)

### Death

`EncounterState.RemoveCreature(Creature)`:
- Remove from `_occupancy`, `_positions`, `_remainingMovement`, `_remainingActionPoints`.
- Remove from `Mercenaries` and `Enemies` lists (whichever it's in).

Removing a creature mid-phase: the creature is yanked from the live grid immediately. `TurnManager`'s phase queue may still reference the dead creature — `CanMove` should also check that the creature is still placed in the state. (Or `EndCreatureTurn` is called silently for any dead creature in the queue at advance time. Pick one and document it.) **Decision:** `TurnManager.CanMove` returns false if the creature is not in `_positions`. The phase queue may contain dead creatures but they're skipped naturally when phase resolution checks `CanMove`.

---

## 5. Input flow (`EncounterScene`)

### State machine

```
Idle ──click merc──▶ MovementMode
MovementMode ──number key K──▶ AttackMode { attack = merc.Attacks[K-1] }
                      [only if AP ≥ attack.ActionPointCost]
MovementMode ──click highlighted tile──▶ moves merc, stays in MovementMode
MovementMode ──click invalid tile──▶ Idle
MovementMode ──space──▶ Idle (turn ended)
AttackMode   ──click valid target──▶ resolve attack, back to MovementMode
AttackMode   ──number key K──▶ swap to that attack (if AP sufficient)
AttackMode   ──Esc──▶ MovementMode
AttackMode   ──click invalid tile──▶ MovementMode
```

A merc's turn ends only on Space, or implicitly when the phase advances after all eligible mercs have ended their turn.

**No-op cases:**
- Number key K pressed but the merc has fewer than K attacks → no-op (no state change, no sound).
- Number key K pressed but `RemainingActionPoints < merc.Attacks[K-1].ActionPointCost` → no-op (stay in MovementMode).

### Rendering layers (in order)

1. Terrain (existing).
2. Yellow overlay on movement-reachable tiles (in MovementMode).
3. Red overlay on valid target tiles (in AttackMode).
4. Cyan border on hover tile + AOE preview tiles (in AttackMode, hover only).
5. Creatures.
6. HP bar above each creature — 25px wide, 3px tall, green proportional to `CurrentHp / CombatStats.HitPoints`, red for missing.
7. AP indicator top-left: a row of `attack.ActionPoints` small filled squares; spent AP rendered as outlines only. Shown only when a mercenary is selected.

No SpriteFont in this slice — all UI is `_pixel`-stamped rectangles.

---

## 6. File map

| Action | Path | Responsibility |
|---|---|---|
| Modify | `Scripts/Creatures/Creature.cs` | Change `CombatStats.ActionPoints` to `int`, formula `Agility/2 + 1`. `RaiseCombatStat` / `LowerCombatStat` keep their `float amount` signature; the `ActionPoints` cases cast via `(int)MathF.Round(amount)`. |
| Modify | `Scripts/Encounter/EncounterState.cs` | Add `RemainingActionPoints` dict, `GetRemainingActionPoints`, `SpendActionPoints`, `ResetActionPoints`, `RemoveCreature`. Initialize AP in `PlaceCreature`. |
| Modify | `Scripts/Encounter/TurnManager.cs` | Call `ResetActionPoints` and `TickStatusEffects` on each creature in a new phase queue. `CanMove` returns false if the creature is no longer in `_positions`. |
| Create | `Scripts/Encounter/CombatResolver.cs` | Pure resolution logic with injected `Random`. |
| Modify | `Scripts/Rendering/EncounterScene.cs` | State machine (Idle / MovementMode / AttackMode), number-key + Esc + Space input, target highlighting, AOE preview, HP bars, AP pips. |
| Create | `Tests/EncounterTests/CombatResolverTests.cs` | Hit/miss, crit/autocrit, damage math, AOE coverage, multi-hit, death, OnHit/OnCrit firing. Uses seeded `Random`. |
| Modify | `Tests/EncounterTests/EncounterStateTests.cs` | Tests for `ResetActionPoints`, `SpendActionPoints`, `RemoveCreature`. |
| Modify | `Tests/EncounterTests/TurnManagerTests.cs` | Test that phase start resets AP and ticks status effects; `CanMove` false for removed creature. |
| Modify | `Tests/CreatureTests.cs` | Update the `CombatStats_ScaledFromBaseStats` test for the new `ActionPoints` formula and integer type. |

---

## 7. Testing strategy

`CombatResolver` is the meat. Tests inject a `Random` with a fixed seed, or wrap rolls in a `Func<int,int,int>` for full determinism. Each test fixes the scenario (attacker stats, defender stats, attack params) and asserts the post-state.

Coverage:
- **Hit roll:** roll at threshold hits, roll below misses, accuracy > 100 always hits, evasion that pushes threshold > 100 never hits.
- **Crit roll:** `AutoCrit` true always crits, `roll ≥ 100 - CritChance` boundary, separate roll from hit.
- **Damage:** `MaxDmg + AttackPower - PhysicalDefense` matches expectation; clamped at 0; doubled on crit.
- **AOE:** all creatures in the resulting offset positions take a hit; empty tiles in the shape are skipped silently.
- **Multi-hit:** `MinHits=MaxHits=3` produces three independent rolls per defender (verifiable with a seeded `Random` and a defender with enough HP to survive).
- **Death:** when `CurrentHp ≤ 0`, defender is removed from state, and remaining hits in the multi-hit loop don't roll against the corpse.
- **OnHit/OnCrit:** OnHit fires on any successful hit; OnCrit fires only on crit; both call `defender.ApplyStatusEffect`.
- **AP cost:** `Resolve` deducts `attack.ActionPointCost` from attacker's `RemainingActionPoints`.

`EncounterScene` remains untested (live MonoGame dependency).

---

## 8. Out of scope

- Enemy AI (next slice). Goblin continues to auto-skip its phase.
- Equipping / swapping weapons mid-encounter.
- Reactions (`Reaction` field on `Attack` exists but is not wired — Parry's counter does not fire).
- Damage numbers, sound, animation.
- The circular-radius range fix (Chebyshev for now, TODO at both call sites).
- Min-range *enforcement during AI* — only the player's targeting respects MinRange this slice.
