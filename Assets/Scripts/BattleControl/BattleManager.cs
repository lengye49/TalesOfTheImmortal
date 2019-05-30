//using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

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

    private List<BattleUnit> AllyList;
    private List<BattleUnit> EnemyList;
    private BattleView battleView;
    private BattleState battleState;

    public GameObject TestStartBattleBtn;
    private GameObject unitPrefab;

    private void Start()
    {
        battleView = this.GetComponent<BattleView>();
        unitPrefab = Resources.Load("Prefabs/BattleUnit", typeof(GameObject)) as GameObject;
    }

    //**************
    /*
     * 步骤：
     * 0. 屏幕放一个按钮，用于开始战斗。 Done
     * 1. 初始化地图格子，不存在则生成，存在则使用之前的。如果卡顿，在考虑优化换成切场景。 Done
     * 2. 生成BattleUnit，并初始化数据和位置。 Done
     * 3. 回合判定，玩家先走。
     * 4. 角色移动。
     * 5. 角色选择攻击目标。
     * 6. 角色释放技能、播放序列帧动画、造成技能效果。
     * 7. 战斗结果判定
     * 8. 回合判定，轮到敌人。
     * 9. 敌人AI-->移动-->选择目标-->释放技能。
     * 10.循环3步直到出现战斗结果
     * 11.停止，并可重新战斗 Done
    */
    //**************


    //**************
    //初始化
    //**************
    public void StartBattle()
    {
        TestStartBattleBtn.SetActive(false);

        BattleMapManager.Instance.InitBattleMap();

        AllyList = new List<BattleUnit>();
        EnemyList = new List<BattleUnit>();
        BattleUnit player = SetBattleUnit("player",UnitSide.Ally, new Vector2Int(3,3));
        AllyList.Add(player);

        BattleUnit enemy = SetBattleUnit("enemy",UnitSide.Enemy, new Vector2Int(3,8));
        EnemyList.Add(enemy);

        battleState = BattleState.Waiting;

        CheckRound();
    }

    BattleUnit SetBattleUnit(string unitName,UnitSide side, Vector2Int pos)
    {
        GameObject unitView = Instantiate(unitPrefab) as GameObject;
        unitView.transform.SetParent(transform);
        BattleUnit unit = new BattleUnit(unitName,side, unitView.GetComponent<BattleUnitView>(),pos);
        Vector2 realPos = BattleMapManager.Instance.UnitsIntoBattle(pos);
        unit.View.SetPos(realPos);
        return unit;
    }


    //**************
    //回合判定
    //**************
    void CheckRound(){
        Debug.Log("Checking Round");
        BattleUnit _unit = AllyList[0];
        for (int i = 0; i < AllyList.Count; i++)
        {
            if (AllyList[i].WaitingTime < _unit.WaitingTime)
                _unit = AllyList[i];
        }
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i].WaitingTime < _unit.WaitingTime)
                _unit = EnemyList[i];
        }
        StartRound(_unit);
    }

    void StartRound(BattleUnit unit)
    {
        Debug.Log("Starting Round "+unit.Side.ToString());
        //切换UI状态
        battleView.StartTurn(unit);

        //Todo下面的代码需要区分自动战斗还是手动战斗
        if (unit.Side == UnitSide.Ally)
        {
            /*
             *  (1)自动战斗、进入AI逻辑，（2）手动战斗，通知BattleMapView，切换该角色状态
            */
            battleState = BattleState.Walking;
            BattleMapManager.Instance.StartRound(unit);
        }
        else
        {
            /*进入AI逻辑*/
        }
    }

    //**************
    //Unit移动
    //**************
    void UnitMove(){
        //Vector2 newPos = BattleMapManager.Instance.UnitMoveTo(start,end);
        //unit.View.MoveTo(newPos);
    }



    //**************
    //选择目标
    //**************



    //**************
    //释放技能
    //**************
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


    //**************
    //结果判定，如果战斗没结束，则进入下一回合
    //**************
    void CheckIsBattleEnd(){
        int unitNum = 0;
        for (int i = 0; i < AllyList.Count;i++){
            if (AllyList[i].HP > 0)
                unitNum++;
        }
        if (unitNum <= 0)
        {
            FinishBattle(true);
            return;
        }

        unitNum = 0;
        for (int i = 0; i < EnemyList.Count;i++){
            if (EnemyList[i].HP > 0)
                unitNum++;
        }
        if(unitNum<=0){
            FinishBattle(false);
            return;
        }
        CheckRound();
    }

    void FinishBattle(bool playerWin){
        Debug.Log("BattleResult : " + (playerWin ? "Win" : "Lose"));
        TestStartBattleBtn.SetActive(true);
    }

    //**************
    //自动行动
    //**************
    void AIRound(BattleUnit unit){
        //1. Move

        //2. Attack

        //3.NextRound
        CheckIsBattleEnd();
    }


    public void ClickCellRespond(){
        if(battleState==BattleState.Walking){
            Debug.Log("Moveing to ");
        }else if(battleState==BattleState.SelectingTarget){
            Debug.Log("Targeting to ");
        }else{
            Debug.Log("State Error");
        }
    }

}

public enum BattleState{
    Waiting,
    Walking,
    SelectingTarget
}
