using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Sets a slider value to a specific playerPref
/// If that specific playerPref isn't found, the value becomes 1
/// </summary>
public class MenuSlider : MonoBehaviour {

    public string PrefToCheck;

    private void Awake()
    {
        GetComponent<Slider>().value = PlayerPrefs.GetFloat(PrefToCheck, 1);
    }
}
