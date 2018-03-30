using UnityEngine;

public class ShrinkObjectOnCall : MonoBehaviour {

    public float minSizeLimit = 0f;
    public float currentSizeValue = 0f;
    public float sizeValueDecrement = 0.025f;
    public bool destroyWhenDone = true;
    bool done = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!done)
        {
            currentSizeValue -= sizeValueDecrement;
            transform.localScale = new Vector3(currentSizeValue, currentSizeValue, currentSizeValue);

            if (currentSizeValue <= minSizeLimit)
            {
                done = true;

                if(destroyWhenDone)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void Shrink()
    {
        done = false;
    }
}
