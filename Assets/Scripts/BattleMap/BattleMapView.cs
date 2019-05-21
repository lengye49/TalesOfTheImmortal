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
    private Vector2 offset = new Vector2(-800, 420);


    private void Start()
    {
        cellPrafab = Resources.Load("Prefabs/BattleMapCell",typeof(GameObject)) as GameObject;
        standard = Resources.Load("BattleMap/standard", typeof(Sprite)) as Sprite;
        walkable = Resources.Load("BattleMap/walkable", typeof(Sprite)) as Sprite;
        selecting = Resources.Load("BattleMap/selecting", typeof(Sprite)) as Sprite;
        targeting = Resources.Load("BattleMap/targeting", typeof(Sprite)) as Sprite;
        InitBattleMapView();
    }

    void InitBattleMapView(){
        cellList = new GameObject[20][];
        for (int i = 0; i < 20;i++){
            cellList[i] = new GameObject[10];
            for (int j = 0; j < 10; j++)
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
        o.GetComponent<BattleCell>().ResetState();

        //test code:
        if (xCount < 5)
            o.transform.GetComponent<Image>().sprite = walkable;
        else if(xCount<7)
            o.transform.GetComponent<Image>().sprite = targeting;
        else if(xCount<9)
            o.transform.GetComponent<Image>().sprite = selecting;
        else
            o.transform.GetComponent<Image>().sprite = standard;

        return o;
    }

    Vector2 GetCellPosition(int xCount, int yCount)
    {
        float x = offset.x + (xCount - 1) * cellWidth;
        float y = offset.y - (yCount - 1) * cellHeight;
        if (xCount % 2 == 1)
            y -= 0.5f * cellHeight;
        return new Vector2(x, y);
    }
}
