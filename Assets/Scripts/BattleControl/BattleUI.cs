using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleUI : MonoBehaviour
{
    private static BattleUI _instance;
    public static BattleUI Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("BattleUI").GetComponent<BattleUI>();
            return _instance;
        }
    }

    public Slider HpSlider;
    public Slider MpSlider;
    public Transform HotKeyContainer;
    public Image HeadImage;

    private GameObject skillPrefab;
    private GameObject itemPrefab;
    private Text hpText;
    private Text mpText;
    private HotKeyUI[] HotKeys;

    private void Awake()
    {
        hpText = HpSlider.GetComponentInChildren<Text>();
        mpText = MpSlider.GetComponentInChildren<Text>();
        skillPrefab = Resources.Load("Prefabs/Skill", typeof(GameObject)) as GameObject;
        itemPrefab = Resources.Load("Prefabs/Item", typeof(GameObject)) as GameObject;

        HotKeys = HotKeyContainer.GetComponentsInChildren<HotKeyUI>();
    }

    public void InitBattleUI(BattleUnit unit){
        InitHeadImage(unit.Image);

        InitHotKeys(unit.Skills,unit.Items);
        //Item
        UpdateHp(unit.HP,unit.HpMax);
        UpdateMp(unit.MP, unit.MpMax);
    }

    void InitHeadImage(string path){
        Sprite sprite = Resources.Load("UIAvatar/" + path, typeof(Sprite)) as Sprite;
        HeadImage.sprite = sprite;
    }

    void InitHotKeys(List<Skill> skills,List<Item> items){
        for (int i = 0; i < skills.Count; i++)
        {
            int index = skills[i].HotKeyIndex;
            HotKeys[index].Init(skills[i].Image);
            HotKeys[index].UpdateCD(skills[i].CD, skills[i].Counting);
        }
        //Todo Item
    }



    //监听快捷键
    private void Update()
    {
        if (BattleManager.Instance.State == BattleState.Waiting)
            return;
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Debug.Log("1 is down");
            ChangeSkill(0);
        }else if(Input.GetKeyDown(KeyCode.Alpha2)){
            Debug.Log("2 is down");
            ChangeSkill(1);
        }

    }

    public void HotKeyDown(string hotKey){
        if (!DefaultConfigs.HotKeyCode.Contains(hotKey))
            return;
        int index = DefaultConfigs.HotKeyCode.IndexOf(hotKey);
        BattleManager.Instance.HotKeyRespond(index);
    }

   

    void ChangeSkill(int index){

    }

    void UpdateHp(int hp,int hpMax){
        float value = hp / hpMax;
        HpSlider.value = value;
        hpText.text = hp + "/" + hpMax;
    }

    void UpdateMp(int mp,int mpMax){
        float value = mp / mpMax;
        MpSlider.value = value;
        mpText.text = mp + "/" + mpMax;
    }

}
