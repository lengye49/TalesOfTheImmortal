//using System;
using UnityEngine;
using System.Reflection;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    private static BattleMapManager _instance;
    public static BattleMapManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("BattleMapContainer").GetComponent<BattleMapManager>();
            return _instance;
        }
    }

    //**************
    //初始化
    //**************

    //**************
    /*
     * 步骤：
     * 0. 屏幕放一个按钮，用于开始战斗
     * 1. 初始化地图格子，不存在则生成，存在则使用之前的。如果卡顿，在考虑优化换成切场景。
     * 2. 生成BattleUnit，并初始化数据和位置。
     * 3. 回合判定，玩家先走。
     * 4. 角色移动。
     * 5. 角色选择攻击目标。
     * 6. 角色释放技能、播放序列帧动画、造成技能效果。
     * 7. 战斗结果判定
     * 8. 回合判定，轮到敌人。
     * 9. 敌人AI-->移动-->选择目标-->释放技能。
     * 10.循环3步直到出现战斗结果
     * 11.停止，并可重新战斗
    */

    public void StartBattle()
    {

        BattleMapManager.Instance.InitBattleMap();


        BattleUnit player = SetBattleUnit("player",new Vector2Int(3,3));
        BattleUnit enemy = SetBattleUnit("enemy",new Vector2Int(3,8));
    }

    BattleUnit SetBattleUnit(string unitName, Vector2Int pos)
    {
        GameObject unitView = Resources.Load("Prefab/BattleUnitView", typeof(GameObject)) as GameObject;
        unitView.transform.SetParent(transform);
        BattleUnit unit = new BattleUnit(unitName, unitView.GetComponent<BattleUnitView>());
        Vector2 realPos = BattleMapManager.Instance.UnitsIntoBattle(pos);
        unit.View.SetPos(realPos);
        return unit;
    }

    void UnitMove(){
        //Vector2 newPos = BattleMapManager.Instance.UnitMoveTo(start,end);
        //unit.View.MoveTo(newPos);
    }

    //Todo 多个目标
    public void CastSkill(BattleUnit attacker,BattleUnit target,Skill skill){
        if (skill == null)
            return;
        SkillEffectHandler handler = new SkillEffectHandler();

        string methodName = skill.EffectType.ToString();
        //string methodName = Enum.GetName(typeof(SkillEffectType), skill.EffectType); //需要using System;
        MethodInfo method = handler.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        if(method==null){
            Debug.Log("Can not find method: " + methodName);
            return;
        }
        object[] parameters = new object[] { attacker, skill.Power, target };
        int result = (int)method.Invoke(handler, parameters);
        Debug.Log(skill.Name + " released, effect--> " + methodName + ", result--> " + result);
    }
}
