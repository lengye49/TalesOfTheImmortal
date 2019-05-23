using UnityEngine;
using UnityEngine.UI;


public class BattleUnitView : MonoBehaviour
{
    private Slider hpSlider;
    private Image avatar;

    void Start()
    {

    }

    public void Init(){
        hpSlider.value = 1.0f;

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
