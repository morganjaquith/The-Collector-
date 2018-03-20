using UnityEngine;

public class MainMenuCamera : MonoBehaviour {

    public Vector3 rotationDirection;

    private void Start()
    {
        //If VR is connected,
        //Spawn VR related objects
        //Delete this camera object

        if(Input.GetJoystickNames().Length > 0)
        {
            PlayerPrefs.SetInt("PlayerOneInputDevice", 1);
            Debug.Log("External Controller Detected");
        }

    }

    private void FixedUpdate ()
    {
        transform.Rotate(rotationDirection);
	}
}
