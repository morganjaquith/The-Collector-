using UnityEngine;

public class MainMenuCamera : MonoBehaviour {

    public Vector3 rotationDirection;

    private void Start()
    {
        //If VR is connected,
        //Spawn VR related objects
        //Delete this camera object
    }

    private void FixedUpdate ()
    {
        transform.Rotate(rotationDirection);
	}
}
