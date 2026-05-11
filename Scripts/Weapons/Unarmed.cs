using RuinGamePDT.Combat;
using RuinGamePDT.Resources;

namespace RuinGamePDT.Weapons;

public class Unarmed : Weapon
{
    public Unarmed() : base(WeaponType.Unarmed)
    {
        Attacks.Add(new Attack(
            name: "Punch",
            minDamage: 0,
            maxDamage: 0,
            actionPointCost: 1,
            accuracy: 100,
            attackShape: new AttackShape(new[] { (0, 0) }),
            range: 1
        ));

        Attacks.Add(new Attack(
            name: "Throw Stone",
            minDamage: 0,
            maxDamage: 0,
            actionPointCost: 1,
            accuracy: 100,
            attackShape: new AttackShape(new[] { (0, 0) }),
            range: 1
        ));

        Attacks.Add(new Attack(
            name: "Shout",
            minDamage: 0,
            maxDamage: 0,
            actionPointCost: 1,
            accuracy: 100,
            attackShape: new AttackShape(new[] { (0, 0) }),
            range: 1
        ));
    }
}
