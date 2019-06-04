using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleView : MonoBehaviour
{


    public Button[] SkillList;

    //**************
    //回合开始时设置角色技能状态
    //**************
    public void StartTurn(BattleUnit unit){
        if(unit.Side==UnitSide.Ally){
            //切换技能、宝物图标，并设置可交互
            SetSkillList(unit.Skills);
            SetSkillInteractive(true);
        }else{
            SetSkillInteractive(false);
        }
    }

    void SetSkillList(List<Skill> skills){
        for (int i = 0; i < SkillList.Length;i++){
            if(i>=skills.Count){
                SkillList[i].gameObject.SetActive(false);
            }else{
                SkillList[i].gameObject.SetActive(true);
                SkillList[i].GetComponent<Image>().sprite = Resources.Load("Skills/" + skills[i].Image, typeof(Sprite)) as Sprite;
                SkillList[i].gameObject.name = skills[i].Id.ToString();
            }
        }
    }

    void SetItemList(){}

    void SetSkillInteractive(bool isActive){
        for (int i = 0; i < SkillList.Length;i++){
            SkillList[i].interactable = isActive;
        }
    }

    void SetItemInteractive(bool isActive){}

    //**************
    //选择目标
    //**************
    public void UpdateBattleGridsShow(List<BattleGrid> grids)
    {
       
    }

}
