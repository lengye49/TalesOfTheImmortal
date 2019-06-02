using UnityEngine;
using System.Collections;

public class BattleCell : MonoBehaviour
{

    public void OnClick()
    {
        Debug.Log("Clicking " + gameObject.name);

        BattleManager.Instance.ClickCellRespond(ParamSplit.StrToVector2Int(gameObject.name));
    }
}
