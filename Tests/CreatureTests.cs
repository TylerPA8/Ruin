using RuinGamePDT.Creatures;
using static RuinGamePDT.Creatures.BaseStat;

namespace RuinGamePDT.Tests;

internal class TestCreature(string name, int agility, int focus, int mind, int strength, int stamina)
    : Creature(name, agility, focus, mind, strength, stamina);

public class CreatureTests
{
    private static readonly Random Rng = new();

    private static int RandStat() => Rng.Next(1, 20);

    private static TestCreature Make(int agility = 5, int focus = 5, int mind = 5, int strength = 5, int stamina = 5)
        => new("Test", agility, focus, mind, strength, stamina);

    [Fact]
    public void BaseStats_InitializedFromConstructor()
    {
        int a = RandStat(), f = RandStat(), m = RandStat(), s = RandStat(), st = RandStat();
        var c = Make(a, f, m, s, st);

        Assert.Equal(a,  c.BaseStats.Agility);
        Assert.Equal(f,  c.BaseStats.Focus);
        Assert.Equal(m,  c.BaseStats.Mind);
        Assert.Equal(s,  c.BaseStats.Strength);
        Assert.Equal(st, c.BaseStats.Stamina);
    }

    [Fact]
    public void CombatStats_ScaledFromBaseStats()
    {
        int a = RandStat(), f = RandStat(), m = RandStat(), s = RandStat(), st = RandStat();
        var c = Make(a, f, m, s, st);

        Assert.Equal(a * 2.5f, c.CombatStats.CritChance);
        Assert.Equal(a * 0.5f, c.CombatStats.Evasion);
        Assert.Equal(a * 0f,   c.CombatStats.ActionPoints);   // ratio TBD

        Assert.Equal(f * 1f,   c.CombatStats.Accuracy);
        Assert.Equal(f * 2.5f, c.CombatStats.AbilityCooldown);
        Assert.Equal(f * 5f,   c.CombatStats.StaminaPool);

        Assert.Equal(m * 5f,   c.CombatStats.ManaPool);
        Assert.Equal(m * 2.5f, c.CombatStats.MagicDamage);
        Assert.Equal(m * 1f,   c.CombatStats.MagicDefense);

        Assert.Equal(s * 2.5f, c.CombatStats.AttackPower);
        Assert.Equal(s * 0f,   c.CombatStats.PhysicalDefense); // ratio TBD
        Assert.Equal(s * 5f,   c.CombatStats.CritDamageBonus);

        Assert.Equal(st * 2f,  c.CombatStats.HitPoints);
        Assert.Equal(MathF.Round(st * 0.5f + 3f), c.CombatStats.MovementPoints);
    }

    [Fact]
    public void CurrentResources_InitializedFromCombatStats()
    {
        int f = RandStat(), m = RandStat(), st = RandStat();
        var c = Make(focus: f, mind: m, stamina: st);

        Assert.Equal(st * 2f, c.CurrentHp);
        Assert.Equal(f * 5f,  c.CurrentStamina);
        Assert.Equal(m * 5f,  c.CurrentMana);
    }

    [Fact]
    public void RaiseStat_UpdatesBaseAndCombatStats()
    {
        int a = RandStat(), f = RandStat(), m = RandStat(), s = RandStat(), st = RandStat();
        int amount = Rng.Next(1, 10);
        var c = Make(a, f, m, s, st);

        int a2 = a + amount, f2 = f + amount, m2 = m + amount, s2 = s + amount, st2 = st + amount;

        c.RaiseStat(Agility, amount);
        Assert.Equal(a2,        c.BaseStats.Agility);
        Assert.Equal(a2 * 2.5f, c.CombatStats.CritChance);
        Assert.Equal(a2 * 0.5f, c.CombatStats.Evasion);
        Assert.Equal(MathF.Round((amount * .1f) + 4), c.CombatStats.ActionPoints);

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
        Assert.Equal(s2 * 0f,   c.CombatStats.PhysicalDefense);
        Assert.Equal(s2 * 5f,   c.CombatStats.CritDamageBonus);

        c.RaiseStat(Stamina, amount);
        Assert.Equal(st2,       c.BaseStats.Stamina);
        Assert.Equal(st2 * 2f,  c.CombatStats.HitPoints);
        Assert.Equal(MathF.Round(st * 0.5f + 3f) + MathF.Round((amount * .5f) + 6), c.CombatStats.MovementPoints);
    }

    [Fact]
    public void LowerStat_UpdatesBaseAndCombatStats()
    {
        int a = Rng.Next(10, 20), f = Rng.Next(10, 20), m = Rng.Next(10, 20),
            s = Rng.Next(10, 20), st = Rng.Next(10, 20);
        int amount = Rng.Next(1, 5);
        var c = Make(a, f, m, s, st);

        int a2 = a - amount, f2 = f - amount, m2 = m - amount, s2 = s - amount, st2 = st - amount;

        c.LowerStat(Agility, amount);
        Assert.Equal(a2,         c.BaseStats.Agility);
        Assert.Equal(a2 * 2.5f,  c.CombatStats.CritChance);
        Assert.Equal(a2 * 0.5f,  c.CombatStats.Evasion);
        Assert.Equal(a2 * 0f,    c.CombatStats.ActionPoints);

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
        Assert.Equal(s2 * 0f,    c.CombatStats.PhysicalDefense);
        Assert.Equal(s2 * 5f,    c.CombatStats.CritDamageBonus);

        c.LowerStat(Stamina, amount);
        Assert.Equal(st2,        c.BaseStats.Stamina);
        Assert.Equal(st2 * 2f,   c.CombatStats.HitPoints);
        Assert.Equal(MathF.Round(st * 0.5f + 3f), c.CombatStats.MovementPoints);
    }

    [Fact]
    public void LowerStat_FloorsAtZero()
    {
        int stat = Rng.Next(1, 10);
        int amount = stat + Rng.Next(1, 10);
        var c = Make(agility: stat);
        c.LowerStat(Agility, amount);

        Assert.Equal(0,  c.BaseStats.Agility);
        Assert.Equal(0f, c.CombatStats.CritChance);
        Assert.Equal(0f, c.CombatStats.Evasion);
        Assert.Equal(0f, c.CombatStats.ActionPoints);
    }

    [Fact]
    public void ChangingStat_DoesNotAffectOthers()
    {
        int a = RandStat(), st = RandStat();
        int amount = Rng.Next(1, 10);
        var c = Make(agility: a, stamina: st);

        c.RaiseStat(Agility, amount);

        Assert.Equal(st,      c.BaseStats.Stamina);
        Assert.Equal(st * 2f, c.CombatStats.HitPoints);
        Assert.Equal(MathF.Round(st * 0.5f + 3f), c.CombatStats.MovementPoints);
    }
}
