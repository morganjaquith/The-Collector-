using UnityEngine;
using UnityEngine.EventSystems;

public class UIControllerCheckerAndSetter : MonoBehaviour {

    /// <summary>
    /// Meant to allow the controller inputs to be used on UI menus
    /// Since they're set to keyboard inputs on default
    /// </summary>
	void Awake ()
    {
        if(PlayerPrefs.GetInt("PlayerOneInputDevice") == 0)
        {
            StandaloneInputModule inputModule = GetComponent<StandaloneInputModule>();

            inputModule.horizontalAxis = "ControllerHorizontal";
            inputModule.verticalAxis = "ControllerVertical";

        }
	}
}
