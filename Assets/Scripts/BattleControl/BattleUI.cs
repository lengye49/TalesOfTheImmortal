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


    private Text hpText;
    private Text mpText;
    private List<Button> SkillList;
    private List<Button> ItemList;
    private GameObject skillPrefab;
    private GameObject itemPrefab;

    private void Awake()
    {
        hpText = HpSlider.GetComponentInChildren<Text>();
        mpText = MpSlider.GetComponentInChildren<Text>();
        skillPrefab = Resources.Load("Prefabs/Skill", typeof(GameObject)) as GameObject;
        itemPrefab = Resources.Load("Prefabs/Item", typeof(GameObject)) as GameObject;
        SkillList = new List<Button>();
        ItemList = new List<Button>();
    }

    void InitBattleUI(BattleUnit unit){
        UpdateHeadImage(unit.Image);
        InitSkills(unit.Skills);
        //Item
        UpdateHp(unit.HP,unit.HpMax);
    }

    void UpdateHeadImage(string path){
        Sprite sprite = Resources.Load("UIAvatar/" + path, typeof(Sprite)) as Sprite;
        HeadImage.sprite = sprite;
    }

    void InitSkills(List<Skill> skills){
        Button btn;
        for (int i = 0; i < skills.Count;i++){
            if(i<SkillList.Count)
            {
                btn = SkillList[i];
            }else{
                GameObject go = Instantiate(skillPrefab) as GameObject;
                go.transform.SetParent(SkillContainer);
                go.transform.localScale = Vector2.one;
                go.transform.localPosition = GetCellPosition(i, 0);
                btn = gameObject.GetComponent<Button>();
            }
            btn.gameObject.SetActive(true);
            btn.GetComponent<Image>().sprite = Resources.Load("Skill/" + skills[i].Image, typeof(Sprite)) as Sprite;
            btn.GetComponentsInChildren<Image>()[1].fillAmount = 0;
        }
    }

    void InitItems(){}

    /// <summary>
    /// 用于更新技能/物品的CD遮罩
    /// </summary>
    private void Update()
    {
    }

    float GetFillAmount(float CD,float CountingDown){
        return 1 - CountingDown / CD;
    }

    void HideSkillBtn(){
        for (int i = 0; i < SkillList.Count;i++){
            SkillList[i].gameObject.SetActive(false);
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
