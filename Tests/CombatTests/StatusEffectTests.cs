using RuinGamePDT.Combat;
using RuinGamePDT.Creatures;

namespace RuinGamePDT.Tests;

public class StatusEffectTests
{
    [Fact]
    public void Constructor_SetsMaxDuration_ToInitialDuration()
    {
        var e = new StatusEffect(StatusEffectType.Bleed, CombatStat.HitPoints, amount: 5, duration: 4);
        Assert.Equal(4, e.MaxDuration);
    }

    [Fact]
    public void Tick_DecrementsDuration_ButNotMaxDuration()
    {
        var e = new StatusEffect(StatusEffectType.Bleed, CombatStat.HitPoints, amount: 5, duration: 3);
        e.Tick();
        Assert.Equal(2, e.Duration);
        Assert.Equal(3, e.MaxDuration);
    }

    [Fact]
    public void IsExpired_TrueWhenDurationHitsZero()
    {
        var e = new StatusEffect(StatusEffectType.Bleed, CombatStat.HitPoints, amount: 1, duration: 1);
        Assert.False(e.IsExpired);
        e.Tick();
        Assert.True(e.IsExpired);
    }
}
