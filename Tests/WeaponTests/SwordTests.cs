using RuinGamePDT.Combat;
using RuinGamePDT.Creatures;
using RuinGamePDT.Resources;
using RuinGamePDT.Weapons;

namespace RuinGamePDT.Tests;

public class SwordTests
{
    private readonly Sword _s = new();

    [Fact] public void Sword_HasCorrectType()  => Assert.Equal(WeaponType.Sword, _s.Type);
    [Fact] public void Sword_HasFourAttacks()  => Assert.Equal(4, _s.Attacks.Count);

    [Fact]
    public void Slash_HasCorrectProperties()
    {
        var a = _s.Attacks.First(a => a.Name == "Slash");
        Assert.Equal(3, a.MinDamage);
        Assert.Equal(5, a.MaxDamage);
        Assert.Equal(1, a.ActionPointCost);
        Assert.Equal(110, a.Accuracy);
        Assert.Equal(1, a.Range);
        Assert.Null(a.Reaction);
        Assert.Null(a.OnHit);
        Assert.Null(a.OnCrit);
    }

    [Fact]
    public void Thrust_HasCorrectProperties()
    {
        var a = _s.Attacks.First(a => a.Name == "Thrust");
        Assert.Equal(2, a.MinDamage);
        Assert.Equal(4, a.MaxDamage);
        Assert.Equal(1, a.ActionPointCost);
        Assert.Equal(100, a.Accuracy);
        Assert.Equal(2, a.Range);
        Assert.Equal(2, a.AttackShape.Offsets.Count());
        Assert.NotNull(a.OnHit);
        Assert.Equal(AttackEffectType.StatReduction, a.OnHit!.Type);
        Assert.Equal(CombatStat.PhysicalDefense, a.OnHit.TargetStat);
        Assert.Equal(5, a.OnHit.MinAmount);
        Assert.Equal(5, a.OnHit.MaxAmount);
        Assert.Equal(2, a.OnHit.MinDuration);
        Assert.Equal(2, a.OnHit.MaxDuration);
    }

    [Fact]
    public void Parry_HasCorrectProperties()
    {
        var a = _s.Attacks.First(a => a.Name == "Parry");
        Assert.Equal(0, a.MinDamage);
        Assert.Equal(0, a.MaxDamage);
        Assert.Equal(1, a.ActionPointCost);
        Assert.Equal(0, a.Range);
        Assert.NotNull(a.Reaction);
        Assert.Equal(ReactionType.OnMissCounter, a.Reaction!.Type);
        Assert.Equal("Slash", a.Reaction.LinkedAttackName);
        Assert.NotNull(a.OnHit);
        Assert.Equal(AttackEffectType.StatIncrease, a.OnHit!.Type);
        Assert.Equal(CombatStat.PhysicalDefense, a.OnHit.TargetStat);
        Assert.Equal(10, a.OnHit.MinAmount);
        Assert.Equal(10, a.OnHit.MaxAmount);
        Assert.Equal(1, a.OnHit.MinDuration);
        Assert.Equal(1, a.OnHit.MaxDuration);
    }

    [Fact]
    public void Whirlwind_HasCorrectProperties()
    {
        var a = _s.Attacks.First(a => a.Name == "Whirlwind");
        Assert.Equal(2, a.MinDamage);
        Assert.Equal(3, a.MaxDamage);
        Assert.Equal(3, a.ActionPointCost);
        Assert.Equal(75, a.Accuracy);
        Assert.Equal(1, a.Range);
        Assert.Equal(4, a.AttackShape.Offsets.Count());
        Assert.Null(a.OnHit);
        Assert.NotNull(a.OnCrit);
        Assert.Equal(AttackEffectType.StatReduction, a.OnCrit!.Type);
        Assert.Equal(CombatStat.PhysicalDefense, a.OnCrit.TargetStat);
        Assert.Equal(5, a.OnCrit.MinAmount);
        Assert.Equal(5, a.OnCrit.MaxAmount);
        Assert.Equal(2, a.OnCrit.MinDuration);
        Assert.Equal(2, a.OnCrit.MaxDuration);
    }
}
