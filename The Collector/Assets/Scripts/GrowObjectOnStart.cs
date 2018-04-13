using UnityEngine;

public class GrowObjectOnStart : MonoBehaviour {

    public float maxSizeLimit = 1f;
    public float currentSizeValue = 0f;
    public float sizeValueIncrement = 0.05f;
    bool done;

    // Use this for initialization
    void Start ()
    {
        transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if(!done)
        {
            currentSizeValue += sizeValueIncrement;
            transform.localScale = new Vector3(currentSizeValue, currentSizeValue, currentSizeValue);

            if(currentSizeValue >= maxSizeLimit)
            {
                done = true;
            }
        }
	}
}
