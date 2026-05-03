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

        Assert.Equal(MathF.Round(a * 2.50f), MathF.Round(c.CombatStats.CritChance));
        Assert.Equal(MathF.Round(a * 0.50f), MathF.Round(c.CombatStats.Evasion));
        Assert.Equal(MathF.Round(a * 0.25f), MathF.Round(c.CombatStats.ActionPoints));

        Assert.Equal(MathF.Round(f * 1.00f), MathF.Round(c.CombatStats.Accuracy));
        Assert.Equal(MathF.Round(f * 2.50f), MathF.Round(c.CombatStats.AbilityCooldown));
        Assert.Equal(MathF.Round(f * 5.00f), MathF.Round(c.CombatStats.StaminaPool));

        Assert.Equal(MathF.Round(m * 5.00f), MathF.Round(c.CombatStats.ManaPool));
        Assert.Equal(MathF.Round(m * 2.50f), MathF.Round(c.CombatStats.MagicDamage));
        Assert.Equal(MathF.Round(m * 1.00f), MathF.Round(c.CombatStats.MagicDefense));

        Assert.Equal(MathF.Round(s * 2.50f), MathF.Round(c.CombatStats.AttackPower));
        Assert.Equal(MathF.Round(s * 1.00f), MathF.Round(c.CombatStats.PhysicalDefense));
        Assert.Equal(MathF.Round(s * 5.00f), MathF.Round(c.CombatStats.CritDamageBonus));

        Assert.Equal(MathF.Round(st * 2.00f), MathF.Round(c.CombatStats.HitPoints));
        Assert.Equal(MathF.Round(st * 0.50f + 3.00f), MathF.Round(c.CombatStats.MovementPoints));
    }

    [Fact]
    public void CurrentResources_InitializedFromCombatStats()
    {
        int f = RandStat(), m = RandStat(), st = RandStat();
        var c = Make(focus: f, mind: m, stamina: st);

        Assert.Equal(MathF.Round(st * 2.00f), MathF.Round(c.CurrentHp));
        Assert.Equal(MathF.Round(f * 5.00f), MathF.Round(c.CurrentStamina));
        Assert.Equal(MathF.Round(m * 5.00f), MathF.Round(c.CurrentMana));
    }

}
