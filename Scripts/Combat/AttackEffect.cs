namespace RuinGamePDT.Combat;

public record AttackEffect(
    AttackEffectType Type,
    float MinAmount,
    float MaxAmount,
    int MinDuration,
    int MaxDuration
);
