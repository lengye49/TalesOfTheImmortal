using UnityEngine;
using UnityEngine.UI;

public class HotKey : MonoBehaviour
{
    private Image icon;
    private Image cover;
    private Button button;
    private Skill _skill;
    private Item _item;

    void Awake()
    {
        Image[] images = GetComponentsInChildren<Image>();
        icon = images[0];
        cover = images[1];
        button = GetComponent<Button>();
    }

    public void Init(HotKeyInfo info)
    {
        if (info.type == 0)
        {
            _skill = LoadFiles.GetSkill();
            icon.sprite = Resources.Load("Skill/" + _skill.Image, typeof(Sprite)) as Sprite;
        }
        else
        {
            _item = LoadFiles.GetItem();
            icon.sprite = null;
        }
    }

    public void TimeChange(float CD, float counting){
        cover.fillAmount= 1 - counting / CD;
    }

    public void OnClick(int index)
    {
        string hotkey = GetComponentInChildren<Text>().text;
        BattleUI.Instance.HotKeyDown(hotkey);
    }
}
