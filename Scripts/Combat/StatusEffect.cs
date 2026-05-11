using RuinGamePDT.Creatures;

namespace RuinGamePDT.Combat;

public enum StatusEffectType
{
    Bleed,
    Burn,
    Chill,
    Poison,
    Static,
    StatReduction,
    StatIncrease,
    Stun
}

public class StatusEffect
{
    public StatusEffectType Type { get; }
    public CombatStat TargetStat { get; }
    public int Amount { get; set; }
    public int Duration { get; set; }
    public int MaxDuration { get; }

    public StatusEffect(StatusEffectType type, CombatStat targetStat, int amount, int duration)
    {
        Type = type;
        TargetStat = targetStat;
        Amount = amount;
        Duration = duration;
        MaxDuration = duration;
    }

    public void Tick()
    {
        if (Duration > 0)
        {
            Duration--;
            // Apply effect logic here, e.g., damage for Bleed/Burn/Poison
        }
    }

    public bool IsExpired => Duration <= 0;
}