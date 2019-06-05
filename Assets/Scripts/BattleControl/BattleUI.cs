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
    public Transform SkillContainer;
    public Transform ItemContainer;
    public Image HeadImage;

    private GameObject skillPrefab;
    private GameObject itemPrefab;
    private Text hpText;
    private Text mpText;
    private List<GameObject> SkillList;
    private List<GameObject> ItemList;
    private List<Image> SkillCovers;
    private List<Image> ItemCovers;

    private void Awake()
    {
        hpText = HpSlider.GetComponentInChildren<Text>();
        mpText = MpSlider.GetComponentInChildren<Text>();
        skillPrefab = Resources.Load("Prefabs/Skill", typeof(GameObject)) as GameObject;
        itemPrefab = Resources.Load("Prefabs/Item", typeof(GameObject)) as GameObject;

        SkillList = new List<GameObject>();
        ItemList = new List<GameObject>();
        SkillCovers = new List<Image>();
        ItemCovers = new List<Image>();

    }

    void InitBattleUI(BattleUnit unit){
        InitHeadImage(unit.Image);

        InitSkills(unit.Skills);
        //Item
        UpdateHp(unit.HP,unit.HpMax);
        UpdateMp(unit.MP, unit.MpMax);
    }

    void InitHeadImage(string path){
        Sprite sprite = Resources.Load("UIAvatar/" + path, typeof(Sprite)) as Sprite;
        HeadImage.sprite = sprite;
    }

    void InitSkills(List<Skill> _skills){
        GameObject go;
        for (int i = 0; i < _skills.Count;i++){
            if(i<SkillList.Count)
            {
                go = SkillList[i];
            }else{
                go = Instantiate(skillPrefab) as GameObject;
                go.transform.SetParent(SkillContainer);
                go.transform.localScale = Vector2.one;
                go.transform.localPosition = GetCellPosition(i, 0);
            }
            go.SetActive(true);
            go.GetComponent<Button>().interactable = true;
            Image[] images = go.GetComponentsInChildren<Image>();
            images[0].sprite = Resources.Load("Skill/" + _skills[i].Image, typeof(Sprite)) as Sprite;
            SkillCovers.Add(images[1]);
            images[1].fillAmount = 0;
        }
    }

    /// <summary>
    /// 更新由于时间变化导致的界面表现。实际运算放在BattleManager里
    /// </summary>
    /// <param name="_skills">Skills.</param>
    /// <param name="_items">Items.</param>
    void UpdateUI(List<Skill> _skills,List<Item> _items){
        //Skill CD
        for (int i = 0; i < _skills.Count;i++){
            if (_skills[i].CountingDown > 0)
            {
                SkillCovers[i].fillAmount = GetFillAmount(_skills[i].CD, _skills[i].CountingDown);
                SkillList[i].GetComponent<Button>().interactable = false;
            }else{
                SkillList[i].GetComponent<Button>().interactable = true;
            }
        }

        //Item CD
    }

    //void InitItems(){}


    float GetFillAmount(float CD,float CountingDown){
        return 1 - CountingDown / CD;
    }

    void HideSkillBtn(){
        for (int i = 0; i < SkillList.Count;i++){
            SkillList[i].SetActive(false);
        }
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
