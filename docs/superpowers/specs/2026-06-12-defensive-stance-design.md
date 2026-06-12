# Defensive Stance — Design Spec
**Date:** 2026-06-12

## Summary

Replace Mercenary's separate Dodge (Evasion +5–15) and Block (PhysicalDefense +5–15) skills with a single "Defensive Stance" skill that buffs both stats simultaneously for 1 AP / 1 round.

This requires refactoring `AttackEffect` to support multiple stat changes per effect.

---

## Data Model Changes

### `AttackEffect.cs`

Introduce a `StatChange` record and change `AttackEffect` from single-stat to a list:

```csharp
public record StatChange(CombatStat Stat, int MinAmount, int MaxAmount);

public record AttackEffect(
    AttackEffectType Type,
    IReadOnlyList<StatChange> Stats,
    int MinDuration,
    int MaxDuration
);
```

`TargetStat`, `MinAmount`, `MaxAmount` are removed. Duration applies to the whole effect.

---

## Behavior Changes

### `Creature.ApplyStatusEffect`

Loop `effect.Stats`, roll each independently, create one `StatusEffect` per stat:

```csharp
public void ApplyStatusEffect(AttackEffect effect)
{
    int duration = Random.Shared.Next(effect.MinDuration, effect.MaxDuration + 1);
    foreach (var statChange in effect.Stats)
    {
        var statusEffect = new StatusEffect(
            (StatusEffectType)effect.Type,
            statChange.Stat,
            Random.Shared.Next(statChange.MinAmount, statChange.MaxAmount + 1),
            duration
        );
        StatusEffects.Add(statusEffect);
    }
}
```

All other `StatusEffect` / `TickStatusEffects` / `CombatResolver` code is unchanged.

---

## Mercenary Skill Changes

- Remove: `Dodge`, `Block`
- Add: `Defensive Stance`
  - AP cost: 1
  - Accuracy: 100
  - Shape: self (0,0)
  - Range: 0
  - OnHit: `AttackEffect(StatIncrease, [Evasion(5,15), PhysicalDefense(5,15)], 1, 1)`

---

## Migration — Existing Skills

All existing single-stat `AttackEffect` usages (Rush, etc.) wrap one `StatChange` in the list. No behavior change.

---

## Files Affected

| File | Change |
|------|--------|
| `Scripts/Combat/AttackEffect.cs` | Add `StatChange`, refactor `AttackEffect` |
| `Scripts/Creatures/Creature.cs` | Update `ApplyStatusEffect` to loop stats |
| `Scripts/Creatures/Humanoids/Mercenaries/Mercenary.cs` | Replace Dodge+Block with Defensive Stance |
