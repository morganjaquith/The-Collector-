using UnityEngine;

public class ObjectRotate : MonoBehaviour {

    public Vector3 rotationDirection;

    private void FixedUpdate()
    {
        transform.Rotate(rotationDirection);
    }
}