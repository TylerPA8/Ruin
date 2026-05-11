using RuinGamePDT.Creatures;

namespace RuinGamePDT.Combat;

public record AttackEffect(
    AttackEffectType Type,
    CombatStat TargetStat,
    int MinAmount,
    int MaxAmount,
    int MinDuration,
    int MaxDuration
);
