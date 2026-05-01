using RuinGamePDT.Creatures;
using static RuinGamePDT.Creatures.BaseStat;

namespace RuinGamePDT.Tests;

public class CreatureTypeTests
{
    private static readonly Random Rng = new();

    private static int AboveCap(int cap) => Rng.Next(cap + 1, cap + 10);
    private static int WithinCap(int cap) => Rng.Next(1, cap + 1);

    [Theory]
    [InlineData(typeof(Mercenary), 10)]
    [InlineData(typeof(Beast),     12)]
    [InlineData(typeof(Monstrosity), 14)]
    public void Type_IsACreature(Type type, int _)
    {
        var instance = (Creature)Activator.CreateInstance(type, 1, 1, 1, 1, 1)!;
        Assert.IsAssignableFrom<Creature>(instance);
    }

    [Theory]
    [InlineData(typeof(Mercenary), 10)]
    [InlineData(typeof(Beast),     12)]
    [InlineData(typeof(Monstrosity), 14)]
    public void Type_StatsWithinCap_AreAccepted(Type type, int cap)
    {
        int a = WithinCap(cap), f = WithinCap(cap), m = WithinCap(cap),
            s = WithinCap(cap), t = WithinCap(cap);
        var instance = (Creature)Activator.CreateInstance(type, a, f, m, s, t)!;

        Assert.Equal(a, instance.BaseStats.Agility);
        Assert.Equal(f, instance.BaseStats.Focus);
        Assert.Equal(m, instance.BaseStats.Mind);
        Assert.Equal(s, instance.BaseStats.Strength);
        Assert.Equal(t, instance.BaseStats.Stamina);
    }

    [Theory]
    [InlineData(typeof(Mercenary), 10)]
    [InlineData(typeof(Beast),     12)]
    [InlineData(typeof(Monstrosity), 14)]
    public void Type_StatsAboveCap_AreClampedToCap(Type type, int cap)
    {
        int a = AboveCap(cap), f = AboveCap(cap), m = AboveCap(cap),
            s = AboveCap(cap), t = AboveCap(cap);
        var instance = (Creature)Activator.CreateInstance(type, a, f, m, s, t)!;

        Assert.Equal(cap, instance.BaseStats.Agility);
        Assert.Equal(cap, instance.BaseStats.Focus);
        Assert.Equal(cap, instance.BaseStats.Mind);
        Assert.Equal(cap, instance.BaseStats.Strength);
        Assert.Equal(cap, instance.BaseStats.Stamina);
    }

    [Theory]
    [InlineData(typeof(Mercenary), 10)]
    [InlineData(typeof(Beast),     12)]
    [InlineData(typeof(Monstrosity), 14)]
    public void Type_RaiseStat_CannotExceedCap(Type type, int cap)
    {
        var instance = (Creature)Activator.CreateInstance(type, cap - 1, cap - 1, cap - 1, cap - 1, cap - 1)!;
        instance.RaiseStat(Agility, Rng.Next(1, 10));

        Assert.Equal(cap, instance.BaseStats.Agility);
    }
}
