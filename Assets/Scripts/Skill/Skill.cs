using UnityEngine;
using System.Collections;

public class Skill
{
    public int Id;
    public int Level;
    public int LevelMax;
    public int NameLang;
    public int DescriptionLang;
    public Sprite Image;
    public SkillRangeType RangeType;
    public int Range;
    public SkillEffectType EffectType;
    public int Attribute;
    public int Power;
    public float CD;
    public float Sing;
    public float Priority;

    public int HotKeyIndex;
    public float Counting;

    public Skill(){
        Id = 1;
        NameLang = 10000;
        DescriptionLang = 10001;
        Image = Resources.Load("Skill/LinerSword",typeof(Sprite)) as Sprite;
        HotKeyIndex = 0;
        RangeType = SkillRangeType.Line;
        Range = 3;
        EffectType = SkillEffectType.PhysicalDamage;
        CD = 2;
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

/// <summary>
/// 玩家角色存储的技能信息
/// </summary>
public class SkillInfo{
    public int Id;
    public int Type;//0BasicSkill,BattleSkill
    public int Level;
    public int Experience;
}

/// <summary>
/// 心法类
/// </summary>
public class MentalMethod{
    public int Id;
    public int NameLang;
    public int DescriptionLang;
    public string Image;
    public int MpIncrease;

    public MentalMethod(){

    }
}
