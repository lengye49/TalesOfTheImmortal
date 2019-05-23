using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetArrow : MonoBehaviour
{
    private bool isOn = false;

    Vector3 attackerPosition;
    Vector3 mouseScreenPosition;
    Vector3 mouseWorldPosition;
    Vector3 mouseVector;
    Vector3 standardVector;

    Dictionary<GridDirection, List<BattleGrid>> targetGridList;
    List<BattleGrid> targetGrids;

    Skill skillCasting;

    void Start()
    {
        float angle = Vector3.Angle(Vector3.right, Vector3.up);
        Debug.Log("right-->up = " + angle);
        angle = Vector3.Angle(Vector3.right, Vector3.down);
        Debug.Log("right-->down = " + angle);
        angle = Vector3.Angle(Vector3.right, Vector3.left);
        Debug.Log("right-->back = " + angle);

        Skill testSkill = new Skill();
        On(new Vector3(0f,0f,-10f),testSkill);
    }

    void Update()
    {
        if (isOn)
        {
            mouseScreenPosition = Input.mousePosition;
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            //Debug.Log(mouseWorldPosition);
            mouseVector = mouseWorldPosition - attackerPosition;
            float angle = Vector3.Angle(Vector3.right, mouseVector);
            bool above = mouseWorldPosition.y > standardVector.y;
            GridDirection direction = GetPointingDirection(angle, above);
            SetTargetGrids(direction);
            Debug.Log(direction.ToString());
        }
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
            return;
        }
        switch(skillCasting.RangeType){
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



    /// <summary>
    /// 激活选择目标，pos为技能释放者的世界坐标.
    /// </summary>
    public void On(Vector3 pos,Skill skill)
    {
        attackerPosition = pos;
        standardVector = pos + Vector3.one;
        isOn = true;

        targetGridList = new Dictionary<GridDirection, List<BattleGrid>>();
        targetGrids = new List<BattleGrid>();

        skillCasting = skill;
    }

    public void Off()
    {
        isOn = false;

        targetGridList = null;
        targetGrids = null;
    }

}
