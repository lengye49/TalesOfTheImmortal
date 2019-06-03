using UnityEngine;
using UnityEngine.UI;


public class BattleUnitView : MonoBehaviour
{
    private Slider hpSlider;
    private Image avatar;

    void Awake()
    {
        hpSlider = GetComponentInChildren<Slider>();
        avatar = GetComponent<Image>();
    }

    public void Init(){
        hpSlider.value = 1.0f;

    }

    public void UpdateHp(float value){
        hpSlider.value = value;
    }


    /// <summary>
    /// 单位移动到某位置
    /// </summary>
    public void MoveTo(Vector2 targetPos){
        transform.localPosition = targetPos;
    }

    public void SetPos(Vector2 targetPos){
        transform.localPosition = targetPos;
    }
}
