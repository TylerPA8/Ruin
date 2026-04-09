using RuinGamePDT.Creatures;
using static RuinGamePDT.Creatures.BaseStat;

namespace RuinGamePDT.Tests;

public class BanditTests
{
    private static readonly Random Rng = new();

    [Fact]
    public void Bandit_IsACreature()
    {
        var bandit = new Bandit();
        Assert.IsAssignableFrom<Creature>(bandit);
    }

    [Fact]
    public void Bandit_StatsWithinRange_AreAccepted()
    {
        int a = Rng.Next(1, 7), f = Rng.Next(1, 7), m = Rng.Next(1, 7),
            s = Rng.Next(1, 7), t = Rng.Next(1, 7);
        var bandit = new Bandit(a, f, m, s, t);

        Assert.Equal(a, bandit.BaseStats.Agility);
        Assert.Equal(f, bandit.BaseStats.Focus);
        Assert.Equal(m, bandit.BaseStats.Mind);
        Assert.Equal(s, bandit.BaseStats.Strength);
        Assert.Equal(t, bandit.BaseStats.Toughness);
    }

    [Fact]
    public void Bandit_StatsAboveCap_AreClampedToSix()
    {
        int a = Rng.Next(7, 20), f = Rng.Next(7, 20), m = Rng.Next(7, 20),
            s = Rng.Next(7, 20), t = Rng.Next(7, 20);
        var bandit = new Bandit(a, f, m, s, t);

        Assert.Equal(6, bandit.BaseStats.Agility);
        Assert.Equal(6, bandit.BaseStats.Focus);
        Assert.Equal(6, bandit.BaseStats.Mind);
        Assert.Equal(6, bandit.BaseStats.Strength);
        Assert.Equal(6, bandit.BaseStats.Toughness);
    }

    [Fact]
    public void Bandit_RaiseStat_CannotExceedCap()
    {
        var bandit = new Bandit(5, 5, 5, 5, 5);
        int amount = Rng.Next(1, 10);
        bandit.RaiseStat(Agility, amount);

        Assert.Equal(6, bandit.BaseStats.Agility);
    }
}