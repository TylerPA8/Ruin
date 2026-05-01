namespace RuinGamePDT.Combat;

public class Attack
{
    public string Name        { get; }
    public int MinDamage      { get; }
    public int MaxDamage      { get; }
    public int MinRange       { get; }
    public int MaxRange       { get; }
    public AttackEffect? OnCrit { get; }
    public AttackEffect? OnHit  { get; }

    public Attack(string name, int minDamage, int maxDamage, int minRange, int maxRange,
        AttackEffect? onCrit = null, AttackEffect? onHit = null)
    {
        Name = name;
        MinDamage = minDamage;
        MaxDamage = maxDamage;
        MinRange  = minRange;
        MaxRange  = maxRange;
        OnCrit    = onCrit;
        OnHit     = onHit;
    }
}
