using RuinGamePDT.Combat;
using RuinGamePDT.Creatures;
using RuinGamePDT.Resources;
using RuinGamePDT.Weapons;

namespace RuinGamePDT.Tests;

public class BowTests
{
    private readonly Bow _b = new();

    [Fact] public void Bow_HasCorrectType()   => Assert.Equal(WeaponType.Bow, _b.Type);
    [Fact] public void Bow_HasFourAttacks()   => Assert.Equal(4, _b.Attacks.Count);

    [Fact]
    public void Loose_HasCorrectProperties()
    {
        var a = _b.Attacks.First(a => a.Name == "Loose");
        Assert.Equal(2, a.MinDamage);
        Assert.Equal(7, a.MaxDamage);
        Assert.Equal(1, a.ActionPointCost);
        Assert.Equal(95, a.Accuracy);
        Assert.Equal(15, a.Range);
        Assert.Equal(3, a.MinRange);
        Assert.False(a.AutoCrit);
        Assert.Equal(1, a.MinHits);
        Assert.Equal(1, a.MaxHits);
        Assert.Single(a.AttackShape.Offsets);
        Assert.Null(a.OnHit);
        Assert.NotNull(a.OnCrit);
        Assert.Equal(AttackEffectType.Bleed, a.OnCrit!.Type);
        Assert.Equal(CombatStat.HitPoints, a.OnCrit.TargetStat);
        Assert.Equal(3, a.OnCrit.MinAmount);
        Assert.Equal(3, a.OnCrit.MaxAmount);
        Assert.Equal(2, a.OnCrit.MinDuration);
        Assert.Equal(2, a.OnCrit.MaxDuration);
    }

    [Fact]
    public void Deadeye_HasCorrectProperties()
    {
        var a = _b.Attacks.First(a => a.Name == "Deadeye");
        Assert.Equal(5, a.MinDamage);
        Assert.Equal(10, a.MaxDamage);
        Assert.Equal(2, a.ActionPointCost);
        Assert.Equal(60, a.Accuracy);
        Assert.Equal(20, a.Range);
        Assert.Equal(3, a.MinRange);
        Assert.True(a.AutoCrit);
        Assert.Equal(1, a.MinHits);
        Assert.Equal(1, a.MaxHits);
        Assert.Single(a.AttackShape.Offsets);
        Assert.Null(a.OnHit);
        Assert.Null(a.OnCrit);
    }

    [Fact]
    public void Suppression_HasCorrectProperties()
    {
        var a = _b.Attacks.First(a => a.Name == "Suppression");
        Assert.Equal(1, a.MinDamage);
        Assert.Equal(2, a.MaxDamage);
        Assert.Equal(2, a.ActionPointCost);
        Assert.Equal(200, a.Accuracy);
        Assert.Equal(20, a.Range);
        Assert.Equal(3, a.MinRange);
        Assert.False(a.AutoCrit);
        Assert.Equal(1, a.MinHits);
        Assert.Equal(1, a.MaxHits);
        Assert.Equal(9, a.AttackShape.Offsets.Count());
        Assert.Null(a.OnHit);
        Assert.NotNull(a.OnCrit);
        Assert.Equal(AttackEffectType.StatReduction, a.OnCrit!.Type);
        Assert.Equal(CombatStat.MovementPoints, a.OnCrit.TargetStat);
        Assert.Equal(2, a.OnCrit.MinAmount);
        Assert.Equal(2, a.OnCrit.MaxAmount);
        Assert.Equal(1, a.OnCrit.MinDuration);
        Assert.Equal(1, a.OnCrit.MaxDuration);
    }

    [Fact]
    public void Suppression_ShapeIs3x3AroundTarget()
    {
        var a = _b.Attacks.First(a => a.Name == "Suppression");
        var offsets = a.AttackShape.Offsets.ToHashSet();

        Assert.Equal(9, offsets.Count);
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
                Assert.Contains((dx, dy), offsets);
    }

    [Fact]
    public void Pincushion_HasCorrectProperties()
    {
        var a = _b.Attacks.First(a => a.Name == "Pincushion");
        Assert.Equal(1, a.MinDamage);
        Assert.Equal(2, a.MaxDamage);
        Assert.Equal(3, a.ActionPointCost);
        Assert.Equal(70, a.Accuracy);
        Assert.Equal(15, a.Range);
        Assert.Equal(3, a.MinRange);
        Assert.False(a.AutoCrit);
        Assert.Equal(3, a.MinHits);
        Assert.Equal(5, a.MaxHits);
        Assert.Single(a.AttackShape.Offsets);
        Assert.NotNull(a.OnHit);
        Assert.Equal(AttackEffectType.Bleed, a.OnHit!.Type);
        Assert.Equal(CombatStat.HitPoints, a.OnHit.TargetStat);
        Assert.Equal(2, a.OnHit.MinAmount);
        Assert.Equal(2, a.OnHit.MaxAmount);
        Assert.Equal(2, a.OnHit.MinDuration);
        Assert.Equal(2, a.OnHit.MaxDuration);
        Assert.Null(a.OnCrit);
    }
}
