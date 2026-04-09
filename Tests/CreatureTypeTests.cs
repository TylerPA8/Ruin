using RuinGamePDT.Creatures;
using static RuinGamePDT.Creatures.BaseStat;

namespace RuinGamePDT.Tests;

public class CreatureTypeTests
{
    private static readonly Random Rng = new();

    private static int AboveCap(int cap) => Rng.Next(cap + 1, cap + 10);
    private static int WithinCap(int cap) => Rng.Next(1, cap + 1);

    [Theory]
    [InlineData(typeof(Mercenary))]
    [InlineData(typeof(Beast))]
    [InlineData(typeof(Monstrosity))]
    public void Type_IsACreature(Type type)
    {
        var instance = (Creature)Activator.CreateInstance(type, 1, 1, 1, 1, 1)!;
        Assert.IsAssignableFrom<Creature>(instance);
    }

    [Theory]
    [InlineData(typeof(Mercenary))]
    [InlineData(typeof(Beast))]
    [InlineData(typeof(Monstrosity))]
    public void Type_StatsWithinCap_AreAccepted(Type type)
    {
        int a = WithinCap(10), f = WithinCap(10), m = WithinCap(10),
            s = WithinCap(10), t = WithinCap(10);
        var instance = (Creature)Activator.CreateInstance(type, a, f, m, s, t)!;

        Assert.Equal(a, instance.BaseStats.Agility);
        Assert.Equal(f, instance.BaseStats.Focus);
        Assert.Equal(m, instance.BaseStats.Mind);
        Assert.Equal(s, instance.BaseStats.Strength);
        Assert.Equal(t, instance.BaseStats.Toughness);
    }

    [Theory]
    [InlineData(typeof(Mercenary))]
    [InlineData(typeof(Beast))]
    [InlineData(typeof(Monstrosity))]
    public void Type_StatsAboveCap_AreClampedToTen(Type type)
    {
        int a = AboveCap(10), f = AboveCap(10), m = AboveCap(10),
            s = AboveCap(10), t = AboveCap(10);
        var instance = (Creature)Activator.CreateInstance(type, a, f, m, s, t)!;

        Assert.Equal(10, instance.BaseStats.Agility);
        Assert.Equal(10, instance.BaseStats.Focus);
        Assert.Equal(10, instance.BaseStats.Mind);
        Assert.Equal(10, instance.BaseStats.Strength);
        Assert.Equal(10, instance.BaseStats.Toughness);
    }

    [Theory]
    [InlineData(typeof(Mercenary))]
    [InlineData(typeof(Beast))]
    [InlineData(typeof(Monstrosity))]
    public void Type_RaiseStat_CannotExceedCap(Type type)
    {
        var instance = (Creature)Activator.CreateInstance(type, 9, 9, 9, 9, 9)!;
        instance.RaiseStat(Agility, Rng.Next(1, 10));

        Assert.Equal(10, instance.BaseStats.Agility);
    }
}
