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
}


public enum SkillRangeType{
    Line,
    Fan,
    Circle,
    TargetCircle
}

public enum SkillEffectType{
    Heal,
    Damage,
    Break,
}
