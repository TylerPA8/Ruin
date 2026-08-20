using RuinGamePDT.Combat;
using RuinGamePDT.Creatures;

namespace RuinGamePDT.Tests;

public class CreatureStatusEffectTests
{
    [Fact]
    public void ApplyStatusEffect_WithEqualMinMax_ProducesExactValue()
    {
        var merc = new Mercenary();
        var effect = new AttackEffect(AttackEffectType.Bleed,
            new[] { new StatChange(CombatStat.HitPoints, 5, 5) },
            MinDuration: 2, MaxDuration: 2);

        merc.ApplyStatusEffect(effect);

        Assert.Single(merc.StatusEffects);
        Assert.Equal(5, merc.StatusEffects[0].Amount);
        Assert.Equal(2, merc.StatusEffects[0].Duration);
    }

    [Fact]
    public void ApplyStatusEffect_InclusiveUpperBound_CanProduceMaxValue()
    {
        var merc = new Mercenary();
        int observedMaxAmount = int.MinValue;
        int observedMaxDuration = int.MinValue;

        for (int i = 0; i < 200; i++)
        {
            merc.StatusEffects.Clear();
            merc.ApplyStatusEffect(new AttackEffect(AttackEffectType.Bleed,
                new[] { new StatChange(CombatStat.HitPoints, 1, 3) },
                MinDuration: 1, MaxDuration: 3));
            observedMaxAmount   = Math.Max(observedMaxAmount,   merc.StatusEffects[0].Amount);
            observedMaxDuration = Math.Max(observedMaxDuration, merc.StatusEffects[0].Duration);
        }

        Assert.Equal(3, observedMaxAmount);
        Assert.Equal(3, observedMaxDuration);
    }

    [Fact]
    public void ApplyStatusEffect_PropagatesTargetStat_FromStatChange()
    {
        var merc = new Mercenary();
        var effect = new AttackEffect(AttackEffectType.StatReduction,
            new[] { new StatChange(CombatStat.Accuracy, 3, 3) },
            MinDuration: 1, MaxDuration: 1);

        merc.ApplyStatusEffect(effect);

        Assert.Single(merc.StatusEffects);
        Assert.Equal(CombatStat.Accuracy, merc.StatusEffects[0].TargetStat);
    }

    [Fact]
    public void ApplyStatusEffect_MultiStat_CreatesOneStatusEffectPerStat()
    {
        var merc = new Mercenary();
        var effect = new AttackEffect(AttackEffectType.StatIncrease,
            new[]
            {
                new StatChange(CombatStat.Evasion, 10, 10),
                new StatChange(CombatStat.PhysicalDefense, 5, 5)
            },
            MinDuration: 1, MaxDuration: 1);

        merc.ApplyStatusEffect(effect);

        Assert.Equal(2, merc.StatusEffects.Count);
        Assert.Contains(merc.StatusEffects, s => s.TargetStat == CombatStat.Evasion && s.Amount == 10);
        Assert.Contains(merc.StatusEffects, s => s.TargetStat == CombatStat.PhysicalDefense && s.Amount == 5);
    }

    [Fact]
    public void ApplyStatusEffect_MultiStat_AllShareSameDuration()
    {
        var merc = new Mercenary();
        var effect = new AttackEffect(AttackEffectType.StatIncrease,
            new[]
            {
                new StatChange(CombatStat.Evasion, 5, 5),
                new StatChange(CombatStat.PhysicalDefense, 5, 5)
            },
            MinDuration: 2, MaxDuration: 2);

        merc.ApplyStatusEffect(effect);

        Assert.Equal(2, merc.StatusEffects.Count);
        Assert.All(merc.StatusEffects, s => Assert.Equal(2, s.Duration));
    }
}
