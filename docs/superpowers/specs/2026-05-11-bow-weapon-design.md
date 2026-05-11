# Bow Weapon Design

**Date:** 2026-05-11
**Branch:** Combat

---

## Overview

Add the `Bow` weapon and its four attacks: Loose, Deadeye, Suppression, Pincushion. The bow is the first ranged weapon in the game — high damage, crowd control, accuracy penalties, and a minimum firing distance. Three new fields are added to `Attack` to support the new mechanics, with defaults that leave existing weapons untouched.

---

## Schema changes

### `Scripts/Combat/Attack.cs` — four new fields

| Field | Type | Default | Purpose |
|---|---|---|---|
| `AutoCrit` | `bool` | `false` | Every successful hit is treated as a crit. Used by Deadeye. |
| `MinHits` | `int` | `1` | Lower bound on attack rolls per cast. |
| `MaxHits` | `int` | `1` | Upper bound on attack rolls per cast. Combat resolution rolls `[MinHits, MaxHits]`; each hit re-rolls accuracy, damage, crit, and `OnHit`/`OnCrit`. |
| `MinRange` | `int` | `0` | Cannot target tiles closer than this. Used by all bow attacks. |

All four fields are optional constructor parameters at the end of the parameter list, so every existing `new Attack(...)` call in the codebase continues to compile and behave identically.

---

## `Scripts/Weapons/Bow.cs`

Subclass of `Weapon` with `WeaponType.Bow` (a new enum value). Adds the following attacks:

| Attack | MinDmg | MaxDmg | Range | MinRange | Acc | AP | Shape | OnHit | OnCrit | Notes |
|---|---|---|---|---|---|---|---|---|---|---|
| **Loose** | 2 | 7 | 15 | 3 | 95 | 1 | `(0,0)` | — | `Bleed(HP, 3, dur 2)` | Reliable single-target. |
| **Deadeye** | 5 | 10 | 20 | 3 | 60 | 2 | `(0,0)` | — | — | `AutoCrit = true`. Trades accuracy for guaranteed crits. |
| **Suppression** | 1 | 2 | 20 | 3 | 200 | 2 | 3×3 (9 tiles) | — | `StatReduction(MovementPoints, 2, dur 1)` | Crowd-control AOE; near-guaranteed hit. |
| **Pincushion** | 1 | 2 | 15 | 3 | 70 | 3 | `(0,0)` | `Bleed(HP, 2, dur 2)` per hit | — | `MinHits=3, MaxHits=5`. |

### `WeaponType` enum

Add `Bow` to `Scripts/Resources/WeaponType.cs`. Order: `Unarmed, Sword, Bow`.

---

## Out of scope

This spec defines the **data shape** of the bow and its attacks. The following runtime behaviors are NOT implemented in this slice:

- The crit-roll mechanic (`1-100` roll, widened by `CritChance`, with `AutoCrit` collapsing the window to 1-100).
- The multi-hit loop (resolve the attack roll `[MinHits, MaxHits]` times).
- `MinRange` enforcement during targeting.

These all belong to combat resolution, which is a separate workstream. The tests in this slice verify the bow is *declared* correctly — the same way Sword tests verify Parry's reaction wiring even though the reaction system isn't wired up yet.

---

## Testing

`Tests/WeaponTests/BowTests.cs` mirrors the Sword/Unarmed structure:

- `Bow_HasCorrectType` — `WeaponType.Bow`
- `Bow_HasFourAttacks`
- `Loose_HasCorrectProperties` — damage, range, MinRange, accuracy, AP, shape, OnCrit Bleed payload (Type, TargetStat=HitPoints, amounts, duration). `AutoCrit` is false. `MinHits`/`MaxHits` are 1.
- `Deadeye_HasCorrectProperties` — damage, range, MinRange, accuracy, AP, shape, `AutoCrit = true`. `OnHit`/`OnCrit` are null. `MinHits`/`MaxHits` are 1.
- `Suppression_HasCorrectProperties` — damage, range, MinRange, accuracy, AP, 9-tile shape, OnCrit StatReduction payload (Type, TargetStat=MovementPoints, amount 2, duration 1).
- `Pincushion_HasCorrectProperties` — damage, range, MinRange, accuracy, AP, single-tile shape, OnHit Bleed payload, `MinHits=3`, `MaxHits=5`. `AutoCrit` is false.

Tests for existing weapons (`SwordTests`, `UnarmedTests`, `PricklebackTests`) require no changes — the new `Attack` parameters all default to backward-compatible values.

---

## File layout

```
Scripts/
  Combat/
    Attack.cs              (modify: add AutoCrit, MinHits, MaxHits, MinRange)
  Resources/
    WeaponType.cs          (modify: add Bow)
  Weapons/
    Bow.cs                 (create)

Tests/
  WeaponTests/
    BowTests.cs            (create)
```
