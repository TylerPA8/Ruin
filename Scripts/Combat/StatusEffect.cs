namespace RuinGamePDT.Combat;

public enum StatusEffectType
{
    Bleed,
    Burn,
    Chill,
    Poison,
    Static,
    StatReduction,
    Stun
}

public class StatusEffect
{
    public StatusEffectType Type { get; }
    public float Amount { get; set; }
    public int Duration { get; set; }
    public int MaxDuration { get; }

    public StatusEffect(StatusEffectType type, float amount, int duration)
    {
        Type = type;
        Amount = amount;
        Duration = duration;
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