//using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    private static BattleManager _instance;
    public static BattleManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("BattlePanel").GetComponent<BattleManager>();
            return _instance;
        }
    }

    private List<BattleUnit> AllyList;
    private List<BattleUnit> EnemyList;
    private BattleView battleView;
    private BattleState battleState;
    private TargetArrow arrow;

    private BattleUnit actingUnit;
    private Skill actingSkill;

    private bool aiPerforming = false;

    public GameObject TestStartBattleBtn;
    private GameObject unitPrefab;

    private void Start()
    {
        battleView = this.GetComponent<BattleView>();
        arrow = GetComponent<TargetArrow>();
        unitPrefab = Resources.Load("Prefabs/BattleUnit", typeof(GameObject)) as GameObject;
        //StartBattle();
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
        actingUnit = unit;

        //Todo下面的代码需要区分自动战斗还是手动战斗
        if(aiPerforming){
            Debug.Log("Start Ai Running...");
        }else{
            battleState = BattleState.Walking;
            BattleMapManager.Instance.StartRound(unit);
        }
    }

    //**************
    //Unit移动(手动移动)
    //**************
    void UnitMove(Vector2Int targetPos){
        Vector2 newPos = BattleMapManager.Instance.UnitMoveTo(actingUnit.Position,targetPos);
        actingUnit.View.MoveTo(newPos);
        actingUnit.Position = targetPos;

        if (!aiPerforming)
            BattleMapManager.Instance.MapView.ResetState();
        SelectingTarget();
    }



    //**************
    //选择目标
    //**************
    void SelectingTarget(int skillIndex=0){
        battleState = BattleState.SelectingTarget;
        actingSkill = actingUnit.Skills[skillIndex];
        Vector3 pos = actingUnit.View.gameObject.transform.position;
        arrow.On(pos, actingSkill);
    }

    void ConfirmingTarget(){
        Debug.Log("Releasing Skill " + actingUnit.Name + "," + actingSkill.Description);
        arrow.Off();
        BattleMapManager.Instance.MapView.ResetState();
        //Todo 释放技能特效
        List<BattleUnit> targets = GetTargets(arrow.targetGrids);
        Debug.Log(targets.Count + " targets found!");
        for (int i = 0; i < targets.Count;i++){
            CastSkill(actingUnit, targets[i], actingSkill);
        }
    }

    List<BattleUnit> GetTargets(List<BattleGrid> targetGrids){
        //TestCode
        string str = "";
        for (int i = 0; i < targetGrids.Count; i++)
            str += targetGrids[i].Position;
        Debug.Log("targetGrids = " + str);

        List<BattleUnit> targets = new List<BattleUnit>();
        BattleGrid battleGrid;
        for (int i = 0; i < AllyList.Count;i++){
            battleGrid = BattleMapManager.Instance.GetBattleGridByPos(AllyList[i].Position);
            Debug.Log(AllyList[i].Name + " position = " + AllyList[i].Position);
            for (int j = 0; j < targetGrids.Count;j++){
                if(targetGrids[j].Position==AllyList[i].Position)
                    targets.Add(AllyList[i]);
            }
        }
        for (int i = 0; i < EnemyList.Count;i++){
            battleGrid = BattleMapManager.Instance.GetBattleGridByPos(EnemyList[i].Position);
            Debug.Log(EnemyList[i].Name + " position = " + EnemyList[i].Position);
            for (int j = 0; j < targetGrids.Count; j++)
            {
                if (targetGrids[j].Position == EnemyList[i].Position)
                    targets.Add(EnemyList[i]);
            }
        }
        return targets;
    }

    //**************
    //释放技能
    //**************
    //Todo 多个目标
    public void CastSkill(BattleUnit attacker,BattleUnit target,Skill skill){
        Debug.Log("Casting Skill to " + target.Name);
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
        object[] parameters = new object[] { attacker, skill.Power.ToString(), target };
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


    public void ClickCellRespond(Vector2Int targetPos){
        if(battleState==BattleState.Walking){
            Debug.Log("Moveing to ");
            UnitMove(targetPos);
        }else if(battleState==BattleState.SelectingTarget){
            Debug.Log("Confirming Target");
            ConfirmingTarget();
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
