using UnityEngine;
using UnityEngine.UI;

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
    public Image HeadImage;

    void InitBattleUI(){
        //HeadImage
        //Skill
        //SecondaryWeapon
        UpdateHp();
    }

    void UpdateHp(){

    }

    void UpdateMp(){

    }




}
