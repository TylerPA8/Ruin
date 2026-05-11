using RuinGamePDT.Combat;
using RuinGamePDT.Resources;

namespace RuinGamePDT.Weapons;

public abstract class Weapon
{
    public WeaponType Type { get; }
    public List<Attack> Attacks { get; } = [];

    protected Weapon(WeaponType type)
    {
        Type = type;
    }
}