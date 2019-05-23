using UnityEngine;
using System.Collections;

public class Skill
{
    public int Id;
    public string Name;
    public string Description;
    public SkillRangeType RangeType;
    public int Range;
    public SkillEffectType EffectType;
    public int Power;

    public Skill(){
        Id = 1;
        Name = "Test Skill";
        Description = "This is a Linear Skill";
        RangeType = SkillRangeType.Line;
        Range = 2;
        EffectType = SkillEffectType.PhysicalDamage;
        Power = 10000;
    }
}


public enum SkillRangeType{
    Line,
    Fan,
    Circle,
    TargetCircle
}

public enum SkillEffectType{
    Heal,
    PhysicalDamage,
    MagicalDamage,
    DirectDamage,
    AddMp,
    Summon,
}
