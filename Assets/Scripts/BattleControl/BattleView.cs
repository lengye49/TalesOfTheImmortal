using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleView : MonoBehaviour
{
	public void UpdateBattleGridsShow(List<BattleGrid> grids){
        for (int i = 0; i < grids.Count;i++){
            if(grids[i].Walkable)
            {
                //ChangeFrame
            }
            if(grids[i].SelectedCount>0)
            {
                //ChangeColor
            }
        }
    }
}
