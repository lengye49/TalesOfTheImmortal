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

    private void Awake()
    {
        //Debug.Log(LoadFiles.GetLanguage(10000));
        //Debug.Log(LoadFiles.GetLanguage(20004));
        //Debug.Log(LoadFiles.GetLanguage(30000));
    }

    private List<BattleUnit> AllyList;
    private List<BattleUnit> EnemyList;
    private BattleView battleView;
    public BattleState State;
    private TargetArrow arrow;

    private BattleUnit actingUnit;
    private Skill actingSkill;

    private bool aiPerforming = false;
    private int enemyDisMin = 0;
    private BattleUnit nearestEnemy;

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

        BattleUnit enemy = SetBattleUnit("enemy",UnitSide.Enemy, new Vector2Int(5,3));
        EnemyList.Add(enemy);

        State = BattleState.Waiting;

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

        if (unit.Side == UnitSide.Ally)
            BattleUI.Instance.InitBattleUI(unit);

        //Todo下面的代码需要区分自动战斗还是手动战斗
        if(aiPerforming){
            Debug.Log("Start Ai Running...");
            AiMove();
        }else{
            State = BattleState.Walking;
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
        //Todo 等待动画完成
        FinishMoving();
    }

    void AiMove(){
        Vector2Int targetPos;
        if (actingUnit.Side == UnitSide.Ally)
            targetPos = GetNearestOpponent(EnemyList);
        else
            targetPos = GetNearestOpponent(AllyList);


        Vector2Int newPos = BattleMapManager.Instance.GetMovingTargetPos(actingUnit, targetPos);
        UnitMove(newPos);
    }

    //Todo 未考虑不能走的情况
    Vector2Int GetNearestOpponent(List<BattleUnit> opponents){
        int distance = 0;
        int index = 0;
        int temp;
        for (int i = 0; i < opponents.Count;i++){
            bool isStraight;
            temp = MathCalculator.GetBattleGridDistance(actingUnit.Position, opponents[i].Position,out isStraight);
            if (i == 0 || temp < distance)
            {
                distance = temp;
                index = i;
            }
        }
        enemyDisMin = distance;
        nearestEnemy = opponents[index];
        return opponents[index].Position;
    }

    public void FinishMoving()
    {
        if (!aiPerforming)
            BattleMapManager.Instance.MapView.ResetState();

        //Todo 增加时间
        SelectingTarget();
    }
    //**************
    //选择目标
    //**************
    void SelectingTarget(){
        if (aiPerforming) {
            int skillIndex = AiSelectSkill();
            if (skillIndex >= 0)
            {
                Vector3 pos1 = actingUnit.View.gameObject.transform.position;
                Vector3 pos2 = nearestEnemy.View.gameObject.transform.position;
                actingSkill = actingUnit.Skills[skillIndex];
                arrow.AiSelectingTarget(pos1, pos2, actingSkill);
                ReleasingSkill();
            }else{
                //没有可用技能的处理情况
            }
        }
        else
        {
            State = BattleState.SelectingTarget;

            //Todo 走完不直接选技能,等待玩家手选
            actingSkill = actingUnit.Skills[0];
            Vector3 pos = actingUnit.View.gameObject.transform.position;
            arrow.On(pos, actingSkill);
        }
    }

    int AiSelectSkill(){
        int index = -1;
        for (int i = 0; i < actingUnit.Skills.Count;i++){
            if (actingUnit.Skills[i].Counting > 0)
                continue;
            if (actingUnit.Skills[i].Range < enemyDisMin)
                continue;
            if (index == -1)
                index = i;
            if (actingUnit.Skills[i].Priority > actingUnit.Skills[index].Priority)
                index = i;
        }
        return index;
    }

    void ConfirmingTarget(){
        Debug.Log("Releasing Skill " + actingUnit.Name + "," + actingSkill.DescriptionLang);
        arrow.Off();
        BattleMapManager.Instance.MapView.ResetState();
        //Todo 释放技能特效
        ReleasingSkill();
    }

    List<BattleUnit> GetTargets(List<BattleGrid> targetGrids){
        //TestCode
        string str = "";
        for (int i = 0; i < targetGrids.Count; i++)
            str += targetGrids[i].Position;
        Debug.Log("targetGrids = " + str);

        List<BattleUnit> targets = new List<BattleUnit>();

        for (int i = 0; i < AllyList.Count;i++){
        //Debug.Log(AllyList[i].Name + " position = " + AllyList[i].Position);
            for (int j = 0; j < targetGrids.Count;j++){
                if(targetGrids[j].Position==AllyList[i].Position)
                    targets.Add(AllyList[i]);
            }
        }
        for (int i = 0; i < EnemyList.Count;i++){
          //Debug.Log(EnemyList[i].Name + " position = " + EnemyList[i].Position);
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
    //Todo 多个目标，多个技能效果

    public void ReleasingSkill(){
        List<BattleUnit> targets = GetTargets(arrow.targetGrids);
        Debug.Log(targets.Count + " targets found!");
        for (int i = 0; i < targets.Count; i++)
        {
            CastSkillEffect(actingUnit, targets[i], actingSkill);
        }
        //扣除其它单位CD
        TimeChange(actingSkill.CD);
        //增加角色及技能CD
        actingUnit.CD += actingSkill.CD;
        actingSkill.Counting += actingSkill.CD;
        //回合检测
        CheckRound();
    }

    public void CastSkillEffect(BattleUnit attacker,BattleUnit target,Skill skill){
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
        Debug.Log(skill.NameLang + " released, effect--> " + methodName + ", result--> " + result);
    }

    //**************
    //CD变更
    //**************
    void TimeChange(float t){
        for (int i = 0; i < AllyList.Count; i++)
        {
            ChangeCD(AllyList[i],t);
        }
        for (int i = 0; i < EnemyList.Count;i++){
            ChangeCD(EnemyList[i],t);
        }
    }

    void ChangeCD(BattleUnit unit,float t){
        if (unit.SingTime > t)
            unit.SingTime -= t;
        else{
            unit.CD = GetNewCD(unit.CD, t - unit.SingTime);
            unit.SingTime = 0;
        }
        for (int i = 0; i < unit.Skills.Count;i++){
            unit.Skills[i].Counting = GetNewCD(unit.Skills[i].Counting, t);
        }
        //Todo Item
    }

    float GetNewCD(float cd, float t){
        return cd > t ? cd - t : 0;
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
        BattleGrid _grid = BattleMapManager.Instance.GetBattleGridByPos(targetPos);
        if (State==BattleState.Walking){
            if (_grid.Walkable)
                UnitMove(targetPos);
            else
                Debug.Log(targetPos + " can not reachable : Occupied = "+_grid.Occupied+", Reachable = "+_grid.Reachable);
        }else if(State==BattleState.SelectingTarget){
            ConfirmingTarget();
        }else{
            Debug.Log("State Error");
        }
    }

    public void HotKeyRespond(int index){
        if(!actingUnit.HotKeys[index].IsInteractive){
            Debug.Log("Can not use hotkey : "+DefaultConfigs.HotKeyCode[index]);
            return;
        }
    }

}

public enum BattleState{
    Waiting,
    Walking,
    SelectingTarget
}
