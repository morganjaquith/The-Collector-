using UnityEngine;

public class Zone : MonoBehaviour {

    public bool isPlayerOnesZone;
    private float pointValue;
    private Item itemObject;
    private GameManager gameManager;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            itemObject = other.GetComponent<Item>();

            if (itemObject.OwnerShip() == 1 && isPlayerOnesZone)
            {
                AddPointsToGameManager();
            }
            else if (itemObject.OwnerShip() == 2 && !isPlayerOnesZone)
            {
                AddPointsToGameManager();
            }
            else
            {
                //Do nothing
            }
        }
    }

    void AddPointsToGameManager()
    {
        pointValue = itemObject.pointValue;
        itemObject.Shrink();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.AddPoints(pointValue, isPlayerOnesZone, true);
    }
}
