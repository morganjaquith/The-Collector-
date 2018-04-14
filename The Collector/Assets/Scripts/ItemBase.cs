using UnityEngine;

public class ItemBase : MonoBehaviour
{

    public GameObject itemObject;
    public bool Respawn;
    private GameObject itemInstance;
    
    
    void Start ()
    {
        SpawnItem();
    }

    private void FixedUpdate()
    {
        if(itemInstance == null&&Respawn)
        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        itemInstance = Instantiate(itemObject, transform.position, transform.rotation) as GameObject;
    }
}
