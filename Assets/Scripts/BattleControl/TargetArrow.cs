using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetArrow : MonoBehaviour
{
    private bool isOn = false;
    private GridDirection lastDirection = GridDirection.None;

    Vector2 attackerPosition;
    Vector2 mousePosition;
    Vector2 mouseVector;
    Vector2 standardVector;

    Dictionary<GridDirection, List<BattleGrid>> targetGridList;
    public List<BattleGrid> targetGrids;

    Skill skillCasting;

    void Start()
    {

    }

    void Update()
    {
        if (isOn)
        {
            mousePosition = Input.mousePosition;
            mouseVector = mousePosition - attackerPosition;
            float angle = Vector3.Angle(Vector3.right, mouseVector);
            bool above = mousePosition.y > standardVector.y;
            GridDirection direction = GetPointingDirection(angle, above);
            if (direction != lastDirection)
            {
                lastDirection = direction;
                SetTargetGrids(direction);
                //Debug.Log(direction.ToString());
            }
        }
    }


    //Todo 这里涉及了vector3和vector2的加减
    public void AiSelectingTarget(Vector3 attackPos,Vector3 defendPos,Skill skill){
        InitState(attackPos, skill);

        mouseVector = defendPos - attackPos;
        float angle = Vector3.Angle(Vector3.right, mouseVector);
        bool above = defendPos.y > standardVector.y;
        GridDirection direction = GetPointingDirection(angle, above);
        SetTargetGrids(direction);
    }

    GridDirection GetPointingDirection(float angle,bool isAbove){
        if(angle<60 && angle>0 ){
            if (isAbove)
                return GridDirection.RightUp;
            else
                return GridDirection.RightDown;
        }else if(angle>60 && angle<120){
            if (isAbove)
                return GridDirection.Up;
            else
                return GridDirection.Down;
        }else{
            if (isAbove)
                return GridDirection.LeftUp;
            else
                return GridDirection.LeftDown;
        }
    }

    void SetTargetGrids(GridDirection direction){
        if (targetGridList.ContainsKey(direction))
        {
            targetGrids = targetGridList[direction];
        }
        else
        {
            switch (skillCasting.RangeType)
            {
                case SkillRangeType.Line:
                    targetGrids = BattleMapManager.Instance.LineTargets(direction, skillCasting.Range);
                    break;
                case SkillRangeType.Fan:
                    targetGrids = BattleMapManager.Instance.FanTargets(direction, skillCasting.Range);
                    break;
                case SkillRangeType.Circle:
                    targetGrids = BattleMapManager.Instance.CircleTargets(skillCasting.Range);
                    break;
                default:
                    break;
            }
            targetGridList.Add(direction, targetGrids);
        }

        BattleMapManager.Instance.MapView.ResetState();
        BattleMapManager.Instance.SelectingTarget(targetGrids);
    }



    /// <summary>
    /// 激活选择目标，pos为技能释放者的世界坐标.
    /// </summary>
    public void On(Vector3 pos,Skill skill)
    {
        isOn = true;
        InitState(pos, skill);
    }

    void InitState(Vector3 pos,Skill skill){
        attackerPosition = pos;
        standardVector = pos + Vector3.one;
        targetGridList = new Dictionary<GridDirection, List<BattleGrid>>();
        targetGrids = new List<BattleGrid>();
        skillCasting = skill;
    }

    public void Off()
    {
        isOn = false;

        targetGridList = null;
        //targetGrids = null;
    }

}
