using RuinGamePDT.Creatures;
using RuinGamePDT.Weapons;
using static RuinGamePDT.Creatures.BaseStat;

namespace RuinGamePDT.Tests;

public class MercenaryTests
{
    [Fact]
    public void Mercenary_IsACreature()
    {
        Assert.IsAssignableFrom<Creature>(new Mercenary());
    }

    [Fact]
    public void Mercenary_HasCorrectName()
    {
        Assert.Equal("Mercenary", new Mercenary().Name);
    }

    [Fact]
    public void Mercenary_StatsWithinRange_AreAccepted()
    {
        var m = new Mercenary(agility: 3, focus: 4, mind: 5, strength: 6, stamina: 7);
        Assert.Equal(3, m.BaseStats.Agility);
        Assert.Equal(4, m.BaseStats.Focus);
        Assert.Equal(5, m.BaseStats.Mind);
        Assert.Equal(6, m.BaseStats.Strength);
        Assert.Equal(7, m.BaseStats.Stamina);
    }

    [Fact]
    public void Mercenary_StatsAboveCap_AreClampedToTen()
    {
        var m = new Mercenary(agility: 15, focus: 15, mind: 15, strength: 15, stamina: 15);
        Assert.Equal(10, m.BaseStats.Agility);
        Assert.Equal(10, m.BaseStats.Focus);
        Assert.Equal(10, m.BaseStats.Mind);
        Assert.Equal(10, m.BaseStats.Strength);
        Assert.Equal(10, m.BaseStats.Stamina);
    }

    [Fact]
    public void Mercenary_RaiseStat_CannotExceedCap()
    {
        var m = new Mercenary(agility: 9);
        m.RaiseStat(Agility, 5);
        Assert.Equal(10, m.BaseStats.Agility);
    }

    [Fact]
    public void Mercenary_DefaultEquippedWeapon_IsSword()
    {
        Assert.IsType<Sword>(new Mercenary().EquippedWeapon);
    }

    [Fact]
    public void Mercenary_Attacks_PopulatedFromEquippedWeapon()
    {
        var m = new Mercenary();
        Assert.Equal(m.EquippedWeapon!.Attacks.Count, m.Attacks.Count);
        foreach (var weaponAttack in m.EquippedWeapon.Attacks)
            Assert.Contains(weaponAttack, m.Attacks);
    }

    [Fact]
    public void Mercenary_RaiseStat_PreservesEquippedWeaponAndAttacks()
    {
        var m = new Mercenary();
        var weapon = m.EquippedWeapon;
        int attackCount = m.Attacks.Count;

        m.RaiseStat(Strength, 1);

        Assert.Same(weapon, m.EquippedWeapon);
        Assert.Equal(attackCount, m.Attacks.Count);
    }
}
