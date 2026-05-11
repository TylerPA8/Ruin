namespace RuinGamePDT.Combat;

public record AttackEffect(
    AttackEffectType Type,
    int MinAmount,
    int MaxAmount,
    int MinDuration,
    int MaxDuration,
    StatusEffect? Buff,
    StatusEffect? Debuff
);
