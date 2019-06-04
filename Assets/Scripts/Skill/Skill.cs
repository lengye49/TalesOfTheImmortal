using UnityEngine;
using System.Collections;

public class Skill
{
    public int Id;
    public string Name;
    public string Description;
    public string Image;
    public SkillRangeType RangeType;
    public int Range;
    public SkillEffectType EffectType;
    public int Power;
    public float CD;
    public float CountingDown;//用于计时的CD

    public Skill(){
        Id = 1;
        Name = "Test Skill";
        Image = "LinerSword";
        Description = "This is a Linear Skill";
        RangeType = SkillRangeType.Line;
        Range = 5;
        EffectType = SkillEffectType.PhysicalDamage;
        Power = 10000;
        CD = 5;
        CountingDown = 0;
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
