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
    private HotKey[] HotKeys;

    private void Awake()
    {
        hpText = HpSlider.GetComponentInChildren<Text>();
        mpText = MpSlider.GetComponentInChildren<Text>();
        skillPrefab = Resources.Load("Prefabs/Skill", typeof(GameObject)) as GameObject;
        itemPrefab = Resources.Load("Prefabs/Item", typeof(GameObject)) as GameObject;

        HotKeys = HotKeyContainer.GetComponentsInChildren<HotKey>();
    }

    public void InitBattleUI(BattleUnit unit){
        InitHeadImage(unit.Image);

        InitHotKeys(unit.UnitHotKeys);
        //Item
        UpdateHp(unit.HP,unit.HpMax);
        UpdateMp(unit.MP, unit.MpMax);
    }

    void InitHeadImage(string path){
        Sprite sprite = Resources.Load("UIAvatar/" + path, typeof(Sprite)) as Sprite;
        HeadImage.sprite = sprite;
    }

    void InitHotKeys(Dictionary<int,HotKeyInfo> hotKeyInfos){
        foreach(int key in hotKeyInfos.Keys){
            if (key < HotKeys.Length)
                HotKeys[key].Init(hotKeyInfos[key]);
        }
    }



    /// <summary>
    /// 更新由于时间变化导致的界面表现。实际运算放在BattleManager里
    /// </summary>
    /// <param name="_skills">Skills.</param>
    /// <param name="_items">Items.</param>
    public void UpdateUI(Dictionary<int, HotKeyInfo> hotKeyInfos)
    {

    }

    //void InitItems(){}


    float GetFillAmount(float CD,float CountingDown){
        return 1 - CountingDown / CD;
    }

    void HideSkillBtn(){
        for (int i = 0; i < HotKeyList.Count;i++){
            HotKeyList[i].SetActive(false);
        }
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
        switch(hotKey){
            case "1":ChangeSkill(0);
                break;
            case "2":ChangeSkill(1);
                break;
            default:
                break;
        }
    }


    //public void SkillHotKeyDown(string hotKey){
    //    for (int i = 0; i < DefaultConfigs.SkillHotKeys.Length;i++){
    //        if(hotKey == DefaultConfigs.SkillHotKeys[i])
    //        {
    //            ChangeSkill(i);
    //            return;
    //        }
    //    }
    //    Debug.Log("Cannot find skillhotkey : " + hotKey);
    //}

    void ChangeSkill(int index){

    }

    //public void ItemHotKeyDown(string hotKey){

    //}

    public void TryChangeItem(string hotKey){
        for (int i = 0; i < DefaultConfigs.ItemHotKeys.Length; i++)
        {
            if (hotKey == DefaultConfigs.ItemHotKeys[i])
            {
                ChangeItem(i);
                return;
            }
        }
        Debug.Log("Cannot find itemhotkey : " + hotKey);
    }

    void ChangeItem(int index)
    {

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

    /// <summary>
    /// Gets the cell position.
    /// </summary>
    /// <returns>The cell position.</returns>
    /// <param name="index">Index.</param>
    /// <param name="type">Type. 0Skill, 1Item</param>
    Vector2 GetCellPosition(int index,int type){
        float x = 100 + 80 * index;
        float y = (type == 0 ? -480 : -400);
        return new Vector2(x, y);
    }


}
