using RuinGamePDT.Resources;
using RuinGamePDT.Weapons;

namespace RuinGamePDT.Tests;

public class UnarmedTests
{
    private readonly Unarmed _u = new();

    [Fact] public void Unarmed_HasCorrectType()    => Assert.Equal(WeaponType.Unarmed, _u.Type);
    [Fact] public void Unarmed_HasThreeAttacks()   => Assert.Equal(3, _u.Attacks.Count);

    [Fact]
    public void Unarmed_ContainsPunch()
    {
        var a = _u.Attacks.First(a => a.Name == "Punch");
        Assert.Equal(0, a.MinDamage);
        Assert.Equal(0, a.MaxDamage);
        Assert.Equal(1, a.ActionPointCost);
        Assert.Equal(1, a.Range);
        Assert.Null(a.OnHit);
        Assert.Null(a.OnCrit);
        Assert.Null(a.Reaction);
    }

    [Fact]
    public void Unarmed_ContainsThrowStone()
    {
        var a = _u.Attacks.First(a => a.Name == "Throw Stone");
        Assert.Equal(0, a.MinDamage);
        Assert.Equal(0, a.MaxDamage);
        Assert.Equal(1, a.ActionPointCost);
        Assert.Equal(1, a.Range);
        Assert.Null(a.OnHit);
        Assert.Null(a.OnCrit);
        Assert.Null(a.Reaction);
    }

    [Fact]
    public void Unarmed_ContainsShout()
    {
        var a = _u.Attacks.First(a => a.Name == "Shout");
        Assert.Equal(0, a.MinDamage);
        Assert.Equal(0, a.MaxDamage);
        Assert.Equal(1, a.ActionPointCost);
        Assert.Equal(1, a.Range);
        Assert.Null(a.OnHit);
        Assert.Null(a.OnCrit);
        Assert.Null(a.Reaction);
    }
}
