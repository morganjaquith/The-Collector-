using UnityEngine;

public class PlayerOneInputSetter : MonoBehaviour {

    private void Start()
    {
        //If VR is connected,
        //Spawn VR related objects
        //Delete this camera object

        //Default value since at this point in runtime we haven't asked the player what mode
        PlayerPrefs.SetInt("PromptedPlayMode", 0);

        if (Input.GetJoystickNames().Length > 0)
        {
            if (Input.GetJoystickNames()[0] == "Controller (Xbox One For Windows)")
            {
                PlayerPrefs.SetInt("PlayerOneInputDevice", 0);
                Debug.Log("External Xbox One Controller Detected");
            }
            else if (Input.GetJoystickNames()[0] == "Controller (Xbox 360 For Windows)")
            {
                PlayerPrefs.SetInt("PlayerOneInputDevice", 0);
                Debug.Log("External Xbox 360 Controller Detected");
            }
            else
            {
                PlayerPrefs.SetInt("PlayerOneInputDevice", 1);
                Debug.Log("No External Controller Detected");
            }
        }
        else
        {
            PlayerPrefs.SetInt("PlayerOneInputDevice", 1);
            Debug.Log("No External Controller Detected");
        }

        PlayerPrefs.Save();
    }
}
