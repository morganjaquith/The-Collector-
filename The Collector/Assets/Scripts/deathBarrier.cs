using UnityEngine;

public class DeathBarrier : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            other.transform.GetComponent<Player>().ApplyDamage();
        }
    }
}
