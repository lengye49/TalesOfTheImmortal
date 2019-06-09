using UnityEngine;
using UnityEngine.UI;

public class HotKeyUI : MonoBehaviour
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

    public void Init(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void UpdateCD(float CD, float counting){
        cover.fillAmount= 1 - counting / CD;
    }

    public void OnClick()
    {
        string hotkey = GetComponentInChildren<Text>().text;
        BattleUI.Instance.HotKeyDown(hotkey);
    }
}
