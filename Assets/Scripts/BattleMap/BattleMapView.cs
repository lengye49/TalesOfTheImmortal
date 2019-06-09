using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMapView : MonoBehaviour {

    private GameObject cellPrafab;
    private GameObject[][] cellList;

    private Sprite standard;
    private Sprite walkable;
    private Sprite selecting;
    private Sprite targeting;

    private float cellWidth = 104;
    private float cellHeight = 120;
    private Vector2 offset;

    private List<BattleGrid> temp;


    private void Start()
    {
        cellPrafab = Resources.Load("Prefabs/BattleMapCell",typeof(GameObject)) as GameObject;
        standard = Resources.Load("BattleMap/standard", typeof(Sprite)) as Sprite;
        walkable = Resources.Load("BattleMap/walkable", typeof(Sprite)) as Sprite;
        selecting = Resources.Load("BattleMap/selecting", typeof(Sprite)) as Sprite;
        targeting = Resources.Load("BattleMap/targeting", typeof(Sprite)) as Sprite;
        float x = -(DefaultConfigs.BattleMapSize.x-3) * cellWidth / 2;
        float y = (DefaultConfigs.BattleMapSize.y-1) * cellHeight / 2;
        offset = new Vector2(x, y);
    }

    public void InitBattleMapView(){
        cellList = new GameObject[DefaultConfigs.BattleMapSize.x][];
        for (int i = 0; i < DefaultConfigs.BattleMapSize.x; i++){
            cellList[i] = new GameObject[DefaultConfigs.BattleMapSize.y];
            for (int j = 0; j < DefaultConfigs.BattleMapSize.y; j++)
            {
                cellList[i][j]= InitGrid(i, j);
            }
        }
    }

    GameObject InitGrid(int xCount, int yCount)
    {
        GameObject o = Instantiate(cellPrafab) as GameObject;
        o.transform.SetParent(this.transform);
        o.transform.localScale = Vector2.one;
        o.transform.localPosition = GetCellPosition(xCount, yCount);
        //o.GetComponent<BattleCell>().ResetState();
        o.transform.GetComponent<Image>().sprite = standard;
        o.name = xCount + "|" + yCount;
        o.GetComponentInChildren<Text>().text = o.name;
        return o;
    }

    public Vector2 GetCellPosition(int xCount, int yCount)
    {
        float x = offset.x + (xCount - 1) * cellWidth;
        float y = offset.y - (yCount - 1) * cellHeight;
        if (xCount % 2 == 1)
            y -= 0.5f * cellHeight;
        return new Vector2(x, y);
    }

    /// <summary>
    /// 设置进场时各角色站位状态
    /// </summary>
    public void SetEnterBattleState(List<BattleGrid> cells){
        for (int i = 0; i < cells.Count;i++){
            //GetCellImageByBattleUnit(cells[i]).sprite = standard;
        }
    }

    /// <summary>
    /// 设置进入回合行走时格子的状态
    /// </summary>
    public void SetWalkingState(BattleGrid standingPoint,List<BattleGrid> walkableRange){

        temp = walkableRange;
        for (int i = 0; i < walkableRange.Count;i++){
            if (!walkableRange[i].Occupied)
            {
                GetCellImageByBattleUnit(walkableRange[i]).sprite = walkable;
            }
        }
        GetCellImageByBattleUnit(standingPoint).sprite = standard;
    }

    public void SetSelectingState(BattleGrid standingPoint,List<BattleGrid> selectingRange){
        //Debug.Log("Set Targeting State");
        temp = selectingRange;
        for (int i = 0; i < selectingRange.Count;i++){
            GetCellImageByBattleUnit(selectingRange[i]).sprite = selecting;
        }
    }

    public void ResetState(){
        for (int i = 0; i < temp.Count;i++){
            GetCellImageByBattleUnit(temp[i]).sprite = standard;
        }
    }

    Image GetCellImageByBattleUnit(BattleGrid unit){
        //Debug.Log("Geting Unit Image by Position = " + unit.Position);
        return cellList[unit.Position.x][unit.Position.y].GetComponent<Image>();
    }
}
