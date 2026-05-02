using RuinGamePDT.Combat;

namespace RuinGamePDT.Creatures;

public enum BaseStat { Agility, Focus, Mind, Strength, Stamina }

public class BaseStats(int agility, int focus, int mind, int strength, int stamina)
{
    public int Agility { get; set; } = agility;
    public int Focus { get; set; } = focus;
    public int Mind { get; set; } = mind;
    public int Strength { get; set; } = strength;
    public int Stamina { get; set; } = stamina;
}

public class CombatStats(BaseStats b)
{
    // 1 Agility = 2.5 CritChance, 0.5 Evasion, .5 ActionPoints
    public float CritChance { get; set; }     = b.Agility * 2.5f;
    public float Evasion { get; set; }        = b.Agility * 0.5f;
    public float ActionPoints { get; set; }   = MathF.Round(b.Agility * 0.1f);

    // 1 Focus = 1 Accuracy, 2.5 AbilityCooldown, 5 StaminaPool
    public float Accuracy { get; set; }       = b.Focus * 1f;
    public float AbilityCooldown { get; set; }= b.Focus * 2.5f;
    public float StaminaPool { get; set; }    = b.Focus * 5f;

    // 1 Mind = 5 ManaPool, 2.5 MagicDamage, 1 MagicDefense
    public float ManaPool { get; set; }       = MathF.Round(b.Mind * 5f);
    public float MagicDamage { get; set; }    = b.Mind * 2.5f;
    public float MagicDefense { get; set; }   = b.Mind * 1f;

    // 1 Strength = 2.5 AttackPower, 1 PhysicalDefense, 5 CritDamageBonus
    public float AttackPower { get; set; }    = b.Strength * 2.5f;
    public float PhysicalDefense { get; set; }= b.Strength * 1f;
    public float CritDamageBonus { get; set; }= b.Strength * 5f;

    // 1 Stamina = 2 HitPoints, .5 MovementPoints
    public float HitPoints { get; set; }      = MathF.Round(b.Stamina * 2f);
    public float MovementPoints { get; set; } = MathF.Round(b.Stamina * 0.5f + 3f);
}

public abstract class Creature
{
    public string Name { get; set; } = string.Empty;

    public BaseStats BaseStats { get; set; }
    public CombatStats CombatStats { get; set; }

    public float CurrentHp { get; set; }
    public float CurrentStamina { get; set; }
    public float CurrentMana { get; set; }

    public List<Attack> Attacks { get; } = [];
    public List<StatusEffect> StatusEffects { get; } = [];

    protected virtual int StatCap => int.MaxValue;

    protected Creature(string name, int agility, int focus, int mind, int strength, int stamina)
    {
        Name = name;
        BaseStats = new BaseStats(
            Math.Clamp(agility,   0, StatCap),
            Math.Clamp(focus,     0, StatCap),
            Math.Clamp(mind,      0, StatCap),
            Math.Clamp(strength,  0, StatCap),
            Math.Clamp(stamina,   0, StatCap)
        );
        CombatStats = new CombatStats(BaseStats);

        CurrentHp = CombatStats.HitPoints;
        CurrentStamina = CombatStats.StaminaPool;
        CurrentMana = CombatStats.ManaPool;
    }

    public void ApplyStatusEffect(AttackEffect effect)
    {
        var statusEffect = new StatusEffect(
            (StatusEffectType)effect.Type,
            Random.Shared.NextSingle() * (effect.MaxAmount - effect.MinAmount) + effect.MinAmount,
            Random.Shared.Next(effect.MinDuration, effect.MaxDuration + 1)
        );
        StatusEffects.Add(statusEffect);
    }

    public void TickStatusEffects()
    {
        foreach (var effect in StatusEffects.ToList())
        {
            effect.Tick();
            switch (effect.Type)
            {
                case StatusEffectType.Bleed:
                case StatusEffectType.Burn:
                case StatusEffectType.Poison:
                    CurrentHp -= effect.Amount;
                    break;
                case StatusEffectType.Chill:
                    CombatStats.MovementPoints -= effect.Amount;
                    break;
                case StatusEffectType.Static:
                    break;
                case StatusEffectType.StatReduction:
                    // Generic stat reduction, e.g., reduce Strength
                    break;
                case StatusEffectType.Stun:
                    break;
            }
            if (effect.IsExpired)
            {
                StatusEffects.Remove(effect);
            }
        }
    }

    public void LowerStat(BaseStat stat, int amount)
    {
        switch (stat)
        {
            case BaseStat.Agility:
                BaseStats.Agility         = Math.Max(0, BaseStats.Agility - amount);
                CombatStats.CritChance    = MathF.Max(0, CombatStats.CritChance   - amount * 2.5f);
                CombatStats.Evasion       = MathF.Max(0, CombatStats.Evasion      - amount * 0.5f);
                CombatStats.ActionPoints  = MathF.Max(0, MathF.Round(CombatStats.ActionPoints - amount * 0f));
                break;
            case BaseStat.Focus:
                BaseStats.Focus               = Math.Max(0, BaseStats.Focus - amount);
                CombatStats.Accuracy          = MathF.Max(0, CombatStats.Accuracy        - amount * 1f);
                CombatStats.AbilityCooldown   = MathF.Max(0, CombatStats.AbilityCooldown - amount * 2.5f);
                CombatStats.StaminaPool       = MathF.Max(0, CombatStats.StaminaPool     - amount * 5f);
                break;
            case BaseStat.Mind:
                BaseStats.Mind            = Math.Max(0, BaseStats.Mind - amount);
                CombatStats.ManaPool      = MathF.Max(0, MathF.Round(CombatStats.ManaPool     - amount * 5f));
                CombatStats.MagicDamage   = MathF.Max(0, CombatStats.MagicDamage  - amount * 2.5f);
                CombatStats.MagicDefense  = MathF.Max(0, CombatStats.MagicDefense - amount * 1f);
                break;
            case BaseStat.Strength:
                BaseStats.Strength            = Math.Max(0, BaseStats.Strength - amount);
                CombatStats.AttackPower       = MathF.Max(0, CombatStats.AttackPower     - amount * 2.5f);
                CombatStats.PhysicalDefense   = MathF.Max(0, CombatStats.PhysicalDefense - amount * 0f);
                CombatStats.CritDamageBonus   = MathF.Max(0, CombatStats.CritDamageBonus - amount * 5f);
                break;
            case BaseStat.Stamina:
                BaseStats.Stamina             = Math.Max(0, BaseStats.Stamina - amount);
                CombatStats.HitPoints         = MathF.Max(0, MathF.Round(CombatStats.HitPoints      - amount * 2f));
                CombatStats.MovementPoints    = MathF.Max(0, MathF.Round(CombatStats.MovementPoints - amount * 0f));
                break;
        }
    }

    public void RaiseStat(BaseStat stat, int amount)
    {
        switch (stat)
        {
            case BaseStat.Agility:
                int newAgility = Math.Clamp(BaseStats.Agility + amount, 0, StatCap);
                int da = newAgility - BaseStats.Agility;
                BaseStats.Agility         = newAgility;
                CombatStats.CritChance   += da * 2.5f;
                CombatStats.Evasion      += da * .5f;
                CombatStats.ActionPoints += MathF.Round((da * .1f) + 4);
                break;
            case BaseStat.Focus:
                int newFocus = Math.Clamp(BaseStats.Focus + amount, 0, StatCap);
                int df = newFocus - BaseStats.Focus;
                BaseStats.Focus              = newFocus;
                CombatStats.Accuracy        += df * 1f;
                CombatStats.AbilityCooldown += df * 2.5f;
                CombatStats.StaminaPool     += df * 5f;
                break;
            case BaseStat.Mind:
                int newMind = Math.Clamp(BaseStats.Mind + amount, 0, StatCap);
                int dm = newMind - BaseStats.Mind;
                BaseStats.Mind            = newMind;
                CombatStats.ManaPool     += MathF.Round(dm * 5f);
                CombatStats.MagicDamage  += dm * 2.5f;
                CombatStats.MagicDefense += dm * 1f;
                break;
            case BaseStat.Strength:
                int newStrength = Math.Clamp(BaseStats.Strength + amount, 0, StatCap);
                int ds = newStrength - BaseStats.Strength;
                BaseStats.Strength           = newStrength;
                CombatStats.AttackPower     += ds * 2.5f;
                CombatStats.PhysicalDefense += ds * 0f;
                CombatStats.CritDamageBonus += ds * 5f;
                break;
            case BaseStat.Stamina:
                int newStamina = Math.Clamp(BaseStats.Stamina + amount, 0, StatCap);
                int dst = newStamina - BaseStats.Stamina;
                BaseStats.Stamina            = newStamina;
                CombatStats.HitPoints       += MathF.Round(dst * 2f);
                CombatStats.MovementPoints  += MathF.Round((dst * .5f) + 6);
                break;
        }
    }
}
