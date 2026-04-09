using RuinGamePDT.Creatures;
using static RuinGamePDT.Creatures.BaseStat;

namespace RuinGamePDT.Tests;

internal class TestCreature(string name, int agility, int focus, int mind, int strength, int toughness)
    : Creature(name, agility, focus, mind, strength, toughness);

public class CreatureTests
{
    private static readonly Random Rng = new();

    private static int RandStat() => Rng.Next(1, 20);

    private static TestCreature Make(int agility = 5, int focus = 5, int mind = 5, int strength = 5, int toughness = 5)
        => new("Test", agility, focus, mind, strength, toughness);

    [Fact]
    public void BaseStats_InitializedFromConstructor()
    {
        int a = RandStat(), f = RandStat(), m = RandStat(), s = RandStat(), t = RandStat();
        var c = Make(a, f, m, s, t);

        Assert.Equal(a, c.BaseStats.Agility);
        Assert.Equal(f, c.BaseStats.Focus);
        Assert.Equal(m, c.BaseStats.Mind);
        Assert.Equal(s, c.BaseStats.Strength);
        Assert.Equal(t, c.BaseStats.Toughness);
    }

    [Fact]
    public void CombatStats_ScaledFromBaseStats()
    {
        int a = RandStat(), f = RandStat(), m = RandStat(), s = RandStat(), t = RandStat();
        var c = Make(a, f, m, s, t);

        Assert.Equal(a * 2.5f, c.CombatStats.CritChance);
        Assert.Equal(a * 0.5f, c.CombatStats.Evasion);
        Assert.Equal(a * 0.25f, c.CombatStats.Speed);

        Assert.Equal(f * 1f,   c.CombatStats.Accuracy);
        Assert.Equal(f * 2.5f, c.CombatStats.AbilityCooldown);
        Assert.Equal(f * 5f,   c.CombatStats.StaminaPool);

        Assert.Equal(m * 5f,   c.CombatStats.ManaPool);
        Assert.Equal(m * 2.5f, c.CombatStats.MagicDamage);
        Assert.Equal(m * 1f,   c.CombatStats.MagicDefense);

        Assert.Equal(s * 2.5f, c.CombatStats.AttackPower);
        Assert.Equal(s * 5f,   c.CombatStats.CritDamageBonus);

        Assert.Equal(t * 2f,   c.CombatStats.HitPoints);
        Assert.Equal(t * 1f,   c.CombatStats.PhysicalDefense);
    }

    [Fact]
    public void CurrentResources_InitializedFromCombatStats()
    {
        int f = RandStat(), m = RandStat(), t = RandStat();
        var c = Make(focus: f, mind: m, toughness: t);

        Assert.Equal(t * 2f, c.CurrentHp);
        Assert.Equal(f * 5f, c.CurrentStamina);
        Assert.Equal(m * 5f, c.CurrentMana);
    }

    [Fact]
    public void RaiseStat_UpdatesBaseAndCombatStats()
    {
        int a = RandStat(), f = RandStat(), m = RandStat(), s = RandStat(), t = RandStat();
        int amount = Rng.Next(1, 10);

        var c = Make(a, f, m, s, t);
        int a2 = a + amount, f2 = f + amount, m2 = m + amount, s2 = s + amount, t2 = t + amount;

        c.RaiseStat(Agility, amount);
        Assert.Equal(a2,        c.BaseStats.Agility);
        Assert.Equal(a2 * 2.5f, c.CombatStats.CritChance);
        Assert.Equal(a2 * 0.5f, c.CombatStats.Evasion);
        Assert.Equal(a2 * 0.25f,c.CombatStats.Speed);

        c.RaiseStat(Focus, amount);
        Assert.Equal(f2,        c.BaseStats.Focus);
        Assert.Equal(f2 * 1f,   c.CombatStats.Accuracy);
        Assert.Equal(f2 * 2.5f, c.CombatStats.AbilityCooldown);
        Assert.Equal(f2 * 5f,   c.CombatStats.StaminaPool);

        c.RaiseStat(Mind, amount);
        Assert.Equal(m2,        c.BaseStats.Mind);
        Assert.Equal(m2 * 5f,   c.CombatStats.ManaPool);
        Assert.Equal(m2 * 2.5f, c.CombatStats.MagicDamage);
        Assert.Equal(m2 * 1f,   c.CombatStats.MagicDefense);

        c.RaiseStat(Strength, amount);
        Assert.Equal(s2,        c.BaseStats.Strength);
        Assert.Equal(s2 * 2.5f, c.CombatStats.AttackPower);
        Assert.Equal(s2 * 5f,   c.CombatStats.CritDamageBonus);

        c.RaiseStat(Toughness, amount);
        Assert.Equal(t2,       c.BaseStats.Toughness);
        Assert.Equal(t2 * 2f,  c.CombatStats.HitPoints);
        Assert.Equal(t2 * 1f,  c.CombatStats.PhysicalDefense);
    }

    [Fact]
    public void LowerStat_UpdatesBaseAndCombatStats()
    {
        // Start high enough that we won't floor at zero
        int a = Rng.Next(10, 20), f = Rng.Next(10, 20), m = Rng.Next(10, 20),
            s = Rng.Next(10, 20), t = Rng.Next(10, 20);
        int amount = Rng.Next(1, 5);

        var c = Make(a, f, m, s, t);
        int a2 = a - amount, f2 = f - amount, m2 = m - amount, s2 = s - amount, t2 = t - amount;

        c.LowerStat(Agility, amount);
        Assert.Equal(a2,         c.BaseStats.Agility);
        Assert.Equal(a2 * 2.5f,  c.CombatStats.CritChance);
        Assert.Equal(a2 * 0.5f,  c.CombatStats.Evasion);
        Assert.Equal(a2 * 0.25f, c.CombatStats.Speed);

        c.LowerStat(Focus, amount);
        Assert.Equal(f2,         c.BaseStats.Focus);
        Assert.Equal(f2 * 1f,    c.CombatStats.Accuracy);
        Assert.Equal(f2 * 2.5f,  c.CombatStats.AbilityCooldown);
        Assert.Equal(f2 * 5f,    c.CombatStats.StaminaPool);

        c.LowerStat(Mind, amount);
        Assert.Equal(m2,         c.BaseStats.Mind);
        Assert.Equal(m2 * 5f,    c.CombatStats.ManaPool);
        Assert.Equal(m2 * 2.5f,  c.CombatStats.MagicDamage);
        Assert.Equal(m2 * 1f,    c.CombatStats.MagicDefense);

        c.LowerStat(Strength, amount);
        Assert.Equal(s2,         c.BaseStats.Strength);
        Assert.Equal(s2 * 2.5f,  c.CombatStats.AttackPower);
        Assert.Equal(s2 * 5f,    c.CombatStats.CritDamageBonus);

        c.LowerStat(Toughness, amount);
        Assert.Equal(t2,         c.BaseStats.Toughness);
        Assert.Equal(t2 * 2f,    c.CombatStats.HitPoints);
        Assert.Equal(t2 * 1f,    c.CombatStats.PhysicalDefense);
    }

    [Fact]
    public void LowerStat_FloorsAtZero()
    {
        int stat = Rng.Next(1, 10);
        int amount = stat + Rng.Next(1, 10); // always exceeds the stat value
        var c = Make(agility: stat);
        c.LowerStat(Agility, amount);

        Assert.Equal(0,   c.BaseStats.Agility);
        Assert.Equal(0f,  c.CombatStats.CritChance);
        Assert.Equal(0f,  c.CombatStats.Evasion);
        Assert.Equal(0f,  c.CombatStats.Speed);
    }

    [Fact]
    public void ChangingStat_DoesNotAffectOthers()
    {
        int a = RandStat(), t = RandStat();
        int amount = Rng.Next(1, 10);
        var c = Make(agility: a, toughness: t);

        c.RaiseStat(Agility, amount);

        Assert.Equal(t,       c.BaseStats.Toughness);
        Assert.Equal(t * 2f,  c.CombatStats.HitPoints);
        Assert.Equal(t * 1f,  c.CombatStats.PhysicalDefense);
    }
}