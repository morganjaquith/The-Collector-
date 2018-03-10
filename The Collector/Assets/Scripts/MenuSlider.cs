using UnityEngine.UI;
using UnityEngine;

public class MenuSlider : MonoBehaviour {

    public string PrefToCheck;

    private void Awake()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat(PrefToCheck, 1);
    }
}
