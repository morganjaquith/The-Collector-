using UnityEngine;

public class ItemBase : MonoBehaviour
{

    public GameObject itemObject;
    private GameObject itemInstance;

    //If we ever consider having more than one type of item (Example : One gives 10 points, one gives 25, another gives 50 etc...)
    //public GameObject[] itemObjects;
    
    void Start ()
    {
        SpawnItem();
    }

    private void FixedUpdate()
    {
        if(itemInstance == null)
        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        itemInstance = Instantiate(itemObject, transform.position, transform.rotation) as GameObject;
    }
}
