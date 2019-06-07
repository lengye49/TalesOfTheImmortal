using UnityEngine;
using System.Collections;

public class Skill:ItemBase
{
    public SkillRangeType RangeType;
    public int Range;
    public SkillEffectType EffectType;
    public int Power;
    public float Sing;

    public Skill(){
        Id = 1;
        Name = "Test Skill";
        Image = "LinerSword";
        Description = "This is a Linear Skill";
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

public class PlayerSkill{
    public int Id;
    public int Level;
    public int Experience;
    public int HotKeyIndex;
}

public class BattleSkill{
    public Skill _skill;
    public Sprite _image;
    public float Counting;

    public BattleSkill(Skill skill){
        _skill = skill;
        _image = Resources.Load("Skill/" + skill.Image, typeof(Sprite)) as Sprite;

    }
}