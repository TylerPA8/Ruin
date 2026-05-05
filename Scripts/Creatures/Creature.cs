using RuinGamePDT.Combat;

namespace RuinGamePDT.Creatures;

public enum BaseStat { Agility, Focus, Mind, Strength, Stamina }

public enum CombatStat
{
    CritChance, Evasion, ActionPoints,
    Accuracy, AbilityCooldown, StaminaPool,
    ManaPool, MagicDamage, MagicDefense,
    AttackPower, PhysicalDefense, CritDamageBonus,
    HitPoints, MovementPoints
}

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
    // 1 Agility = 2.5 CritChance, 0.5 Evasion, .25 ActionPoints
    public float CritChance { get; set; }     = b.Agility * 2.5f;
    public float Evasion { get; set; }        = b.Agility * 0.5f;
    public float ActionPoints { get; set; }   = b.Agility * .25f;

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

    public void RaiseStat(BaseStat stat, int amount)
    {
        switch (stat)
        {
            case BaseStat.Agility:
                BaseStats.Agility = Math.Clamp(BaseStats.Agility + amount, 0, StatCap);
                break;
            case BaseStat.Focus:
                BaseStats.Focus = Math.Clamp(BaseStats.Focus + amount, 0, StatCap);
                break;
            case BaseStat.Mind:
                BaseStats.Mind = Math.Clamp(BaseStats.Mind + amount, 0, StatCap);
                break;
            case BaseStat.Strength:
                BaseStats.Strength = Math.Clamp(BaseStats.Strength + amount, 0, StatCap);
                break;
            case BaseStat.Stamina:
                BaseStats.Stamina = Math.Clamp(BaseStats.Stamina + amount, 0, StatCap);
                break;
        }
        CombatStats = new CombatStats(BaseStats);
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

public void RaiseCombatStat(CombatStat stat, float amount)
    {
        switch (stat)
        {
            case CombatStat.CritChance:
                CombatStats.CritChance = MathF.Max(0, CombatStats.CritChance + amount);
                break;
            case CombatStat.Evasion:
                CombatStats.Evasion = MathF.Max(0, CombatStats.Evasion + amount);
                break;
            case CombatStat.ActionPoints:
                CombatStats.ActionPoints = MathF.Max(0, CombatStats.ActionPoints + amount);
                break;
            case CombatStat.Accuracy:
                CombatStats.Accuracy = MathF.Max(0, CombatStats.Accuracy + amount);
                break;
            case CombatStat.AbilityCooldown:
                CombatStats.AbilityCooldown = MathF.Max(0, CombatStats.AbilityCooldown + amount);
                break;
            case CombatStat.StaminaPool:
                CombatStats.StaminaPool = MathF.Max(0, CombatStats.StaminaPool + amount);
                break;
            case CombatStat.ManaPool:
                CombatStats.ManaPool = MathF.Max(0, CombatStats.ManaPool + amount);
                break;
            case CombatStat.MagicDamage:
                CombatStats.MagicDamage = MathF.Max(0, CombatStats.MagicDamage + amount);
                break;
            case CombatStat.MagicDefense:
                CombatStats.MagicDefense = MathF.Max(0, CombatStats.MagicDefense + amount);
                break;
            case CombatStat.AttackPower:
                CombatStats.AttackPower = MathF.Max(0, CombatStats.AttackPower + amount);
                break;
            case CombatStat.PhysicalDefense:
                CombatStats.PhysicalDefense = MathF.Max(0, CombatStats.PhysicalDefense + amount);
                break;
            case CombatStat.CritDamageBonus:
                CombatStats.CritDamageBonus = MathF.Max(0, CombatStats.CritDamageBonus + amount);
                break;
            case CombatStat.HitPoints:
                CombatStats.HitPoints = MathF.Max(0, CombatStats.HitPoints + amount);
                break;
            case CombatStat.MovementPoints:
                CombatStats.MovementPoints = MathF.Max(0, CombatStats.MovementPoints + amount);
                break;
        }
    }

    public void LowerCombatStat(CombatStat stat, float amount)
    {
        switch (stat)
        {
            case CombatStat.CritChance:
                CombatStats.CritChance = MathF.Max(0, CombatStats.CritChance - amount);
                break;
            case CombatStat.Evasion:
                CombatStats.Evasion = MathF.Max(0, CombatStats.Evasion - amount);
                break;
            case CombatStat.ActionPoints:
                CombatStats.ActionPoints = MathF.Max(0, CombatStats.ActionPoints - amount);
                break;
            case CombatStat.Accuracy:
                CombatStats.Accuracy = MathF.Max(0, CombatStats.Accuracy - amount);
                break;
            case CombatStat.AbilityCooldown:
                CombatStats.AbilityCooldown = MathF.Max(0, CombatStats.AbilityCooldown - amount);
                break;
            case CombatStat.StaminaPool:
                CombatStats.StaminaPool = MathF.Max(0, CombatStats.StaminaPool - amount);
                break;
            case CombatStat.ManaPool:
                CombatStats.ManaPool = MathF.Max(0, CombatStats.ManaPool - amount);
                break;
            case CombatStat.MagicDamage:
                CombatStats.MagicDamage = MathF.Max(0, CombatStats.MagicDamage - amount);
                break;
            case CombatStat.MagicDefense:
                CombatStats.MagicDefense = MathF.Max(0, CombatStats.MagicDefense - amount);
                break;
            case CombatStat.AttackPower:
                CombatStats.AttackPower = MathF.Max(0, CombatStats.AttackPower - amount);
                break;
            case CombatStat.PhysicalDefense:
                CombatStats.PhysicalDefense = MathF.Max(0, CombatStats.PhysicalDefense - amount);
                break;
            case CombatStat.CritDamageBonus:
                CombatStats.CritDamageBonus = MathF.Max(0, CombatStats.CritDamageBonus - amount);
                break;
            case CombatStat.HitPoints:
                CombatStats.HitPoints = MathF.Max(0, CombatStats.HitPoints - amount);
                break;
            case CombatStat.MovementPoints:
                CombatStats.MovementPoints = MathF.Max(0, CombatStats.MovementPoints - amount);
                break;
        }
    }
}

