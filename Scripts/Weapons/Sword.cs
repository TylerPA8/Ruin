using RuinGamePDT.Combat;
using RuinGamePDT.Creatures;
using RuinGamePDT.Resources;

namespace RuinGamePDT.Weapons;

public class Sword : Weapon
{
    public Sword() : base(WeaponType.Sword)
    {
        // Slash
        Attacks.Add(new Attack(
            name: "Slash",
            minDamage: 3,
            maxDamage: 5,
            actionPointCost: 1,
            accuracy: 110,
            attackShape: new AttackShape(new[] { (0, 0) }),
            range: 1,
            reaction: null,
            onHit: null,
            onCrit: null
        ));

        // Thrust - 2 square line
        Attacks.Add(new Attack(
            name: "Thrust",
            minDamage: 2,
            maxDamage: 4,
            actionPointCost: 1,
            accuracy: 100,
            attackShape: new AttackShape(new[] { (0, 0), (0, 1) }),
            range: 2,
            reaction: null,
            onHit: new AttackEffect(AttackEffectType.StatReduction, CombatStat.PhysicalDefense, 5, 5, 2, 2),
            onCrit: null
        ));

        // Parry
        Attacks.Add(new Attack(
            name: "Parry",
            minDamage: 0,
            maxDamage: 0,
            actionPointCost: 1,
            accuracy: 100,
            attackShape: new AttackShape(new[] { (0, 0) }),
            range: 0,
            reaction: new Reaction(ReactionType.OnMissCounter, "Slash"),
            onHit: new AttackEffect(AttackEffectType.StatIncrease, CombatStat.PhysicalDefense, 10, 10, 1, 1),
            onCrit: null
        ));

        // Whirlwind
        Attacks.Add(new Attack(
            name: "Whirlwind",
            minDamage: 2,
            maxDamage: 3,
            actionPointCost: 3,
            accuracy: 75,
            attackShape: new AttackShape(new[] { (-1, 0), (1, 0), (0, -1), (0, 1) }),
            range: 1,
            reaction: null,
            onHit: null,
            onCrit: new AttackEffect(AttackEffectType.StatReduction, CombatStat.PhysicalDefense, 5, 5, 2, 2)
        ));
    }
}
