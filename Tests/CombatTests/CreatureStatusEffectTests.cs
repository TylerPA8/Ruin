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
            MinAmount: 5, MaxAmount: 5,
            MinDuration: 2, MaxDuration: 2,
            Buff: null, Debuff: null);

        merc.ApplyStatusEffect(effect);

        Assert.Single(merc.StatusEffects);
        Assert.Equal(5, merc.StatusEffects[0].Amount);
        Assert.Equal(2, merc.StatusEffects[0].Duration);
    }

    [Fact]
    public void ApplyStatusEffect_InclusiveUpperBound_CanProduceMaxValue()
    {
        // Regression: Random.Next(min, max) is exclusive on max. Without the
        // fix, MaxAmount/MaxDuration are unreachable. Run enough trials that
        // the max bucket should be hit at least once if the fix is in place.
        var merc = new Mercenary();
        int observedMaxAmount = int.MinValue;
        int observedMaxDuration = int.MinValue;

        for (int i = 0; i < 200; i++)
        {
            merc.StatusEffects.Clear();
            merc.ApplyStatusEffect(new AttackEffect(AttackEffectType.Bleed,
                MinAmount: 1, MaxAmount: 3,
                MinDuration: 1, MaxDuration: 3,
                Buff: null, Debuff: null));
            observedMaxAmount   = Math.Max(observedMaxAmount,   merc.StatusEffects[0].Amount);
            observedMaxDuration = Math.Max(observedMaxDuration, merc.StatusEffects[0].Duration);
        }

        Assert.Equal(3, observedMaxAmount);
        Assert.Equal(3, observedMaxDuration);
    }
}
