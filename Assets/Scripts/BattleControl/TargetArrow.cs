using UnityEngine;
using System.Collections;

public class TargetArrow : MonoBehaviour
{
    //public RectTransform arrow;
    //private bool isOn = false;
    //private TargetHover lastHover = null;
    //private TargetHover thisHover = null;

    //void Start()
    //{
    //    Off();
    //}

    //void Update()
    //{
    //    if (isOn)
    //    {
    //        RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, -Vector2.up);
    //        if (hit.collider == null)
    //        {
    //            if (lastHover != null)
    //                lastHover.Recover();
    //        }
    //        else
    //        {
    //            thisHover = hit.collider.GetComponent<TargetHover>();
    //            if (thisHover != lastHover)
    //            {
    //                if (lastHover != null)
    //                    lastHover.Recover();

    //                lastHover = thisHover;

    //                if (lastHover != null)
    //                    lastHover.HighLight();
    //            }
    //        }
    //    }
    //}

    //public void On(Vector3 pos)
    //{
    //    arrow.gameObject.SetActive(true);
    //    arrow.localPosition = pos;
    //    isOn = true;
    //}

    //public void Off()
    //{
    //    arrow.gameObject.SetActive(false);
    //    arrow.sizeDelta = new Vector2(arrow.sizeDelta.x, 5f);
    //    isOn = false;
    //    if (lastHover != null)
    //        lastHover.Recover();
    //}

    //public void UpdateArrow(Vector3 basePos, Vector3 targetPos)
    //{

    //    float d = Vector3.Distance(basePos, targetPos);
    //    arrow.sizeDelta = new Vector2(arrow.sizeDelta.x, d);

    //    float a = Mathf.Atan2((targetPos.x - basePos.x), (targetPos.y - basePos.y));
    //    arrow.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -a * 180 / Mathf.PI));
    //}
}
