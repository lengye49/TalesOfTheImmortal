using UnityEngine;
using UnityEngine.UI;

public class HpBarColor : MonoBehaviour {


	public void ChangeValue () {
        float value = GetComponent<Slider>().value;
        GetComponent<Slider>().fillRect.GetComponent<Image>().color = GetColorByFloat(value);
	}

    Color GetColorByFloat(float value){
        if (value < 0.2f)
            return Color.red;
        else if (value < 0.6f)
            return Color.yellow;
        else
            return Color.green;
    }

}
