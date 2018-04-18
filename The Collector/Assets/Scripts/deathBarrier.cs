using UnityEngine;

public class deathBarrier : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
