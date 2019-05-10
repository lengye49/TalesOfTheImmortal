using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapView : MonoBehaviour {

    private GameObject cellPrafab;
    private GameObject[][] cellList;
    private float cellSize = 64;
    private Vector2 offset = new Vector2(32, 640);


    private void Start()
    {
        cellPrafab = new GameObject();
    }

    void InitBattleMapView(){

    }

    GameObject InitGrid(int xCount, int yCount)
    {
        GameObject o = Instantiate(cellPrafab) as GameObject;
        o.transform.parent = this.transform;
        o.transform.localScale = Vector2.one;
        o.transform.localPosition = GetCellPosition(xCount, yCount);
        o.GetComponent<BattleCell>().ResetState();
        return o;
    }

    Vector2 GetCellPosition(int xCount, int yCount)
    {
        float x = offset.x + (xCount - 1) * cellSize;
        float y = offset.y - (yCount - 1) * cellSize;
        if (yCount % 2 == 1)
            y -= 0.5f * cellSize;
        return new Vector2(x, y);
    }
}
