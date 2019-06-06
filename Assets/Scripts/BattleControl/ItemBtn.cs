using UnityEngine;
using UnityEngine.UI;

public class ItemBtn : MonoBehaviour {

    public void OnClick(){
        string hotkey = GetComponentInChildren<Text>().text;
        BattleUI.Instance.i(hotkey);
    }
}
