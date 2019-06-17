using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool IsCounting = false;
    float hoverTime = 0;
    void Update(){
        if (!IsCounting)
            return;
        hoverTime += Time.deltaTime;
        if (hoverTime > 2f)
        {
            Debug.Log("Show skill range");
            IsCounting = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        IsCounting = true;
    }

    public void OnPointerExit(PointerEventData eventData){
        hoverTime = 0;
        IsCounting = false;
        Debug.Log("exit");
    }
}
