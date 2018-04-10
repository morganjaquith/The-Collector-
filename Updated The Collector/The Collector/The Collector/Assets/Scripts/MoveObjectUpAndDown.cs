using UnityEngine;

public class MoveObjectUpAndDown : MonoBehaviour
{
    //adjust this to change speed
    public float speed = 5f;
    //adjust this to change how high it goes
    public float height = 0.5f;

    private Vector3 pos;

    private void Start()
    {
        //get the objects current position and put it in a variable so we can access it later with less code
        pos = transform.localPosition;
    }

    void Update()
    {
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed);
        //set the object's Y to the new calculated Y
        transform.localPosition = new Vector3(pos.x, newY, pos.z) * height;
    }

    private void OnDisable()
    {
        transform.localPosition = pos;
    }
}