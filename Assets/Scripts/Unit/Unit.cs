using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit
{
    public string Name;
    public string Description;
    public Dictionary<int, SkillInfo> Skills;
    public Dictionary<int, MentalMethod> MentalMethods;
    public List<Item> Equips;
    public List<HotKeyInfo> HotKeys;

    public int Body;//体质-->Hp,DamageReduction
    public int Strength;//力量-->PhysicalAttack,WeaponWeight
    public int Knowlodge;//悟性-->LearningSpeed
    public int Speed;//速度-->BasicSpeed,Dodge
    public int Spirit;//精神-->Crit|Hit


    public BattleUnit GetBattleUnit(BattleUnitView view,Vector2Int pos){
        BattleUnit unit = new BattleUnit(Name,UnitSide.Ally,view,pos);
        unit.HotKeys = HotKeys;
        unit.Skills = new List<Skill>();
        for (int i = 0; i < HotKeys.Count;i++){
            if(HotKeys[i].type==0){
                Skill skill = LoadFiles.GetSkill(Skills[HotKeys[i].param]);
                unit.Skills.Add(skill);
            }else{
                //Todo Item
            }
        }
        return unit;
    }
}
