using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour {

    public void OnClick(){
        string hotkey = GetComponentInChildren<Text>().text;
        BattleUI.Instance.SkillHotKeyDown(hotkey);
    }
}
