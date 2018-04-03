using UnityEngine;

public class Item : MonoBehaviour {

    public float pointValue = 10f;
	public float speed = 2f;
    public float dropTime = 3f;
    public int ownerShip = 0;
    public bool pickedUp;
    public bool justDropped;
    public bool movingItem;
	private Transform destination;
    private float startDropTime = 3f;

    private void Start()
    {
        startDropTime = dropTime;
    }

    void Update ()
	{
		if(movingItem)
		{
            
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, destination.position, step);
			
			if(transform.position == destination.position)
            {
                transform.parent = destination.transform;
                transform.GetChild(0).GetComponent<MoveObjectUpAndDown>().enabled = false;
                movingItem = false;
                pickedUp = true;
			}
			
		}
        else if (justDropped)
        {
            dropTime -= Time.deltaTime;

            if(dropTime <= 0)
            {
                pickedUp = false;
                justDropped = false;
                dropTime = startDropTime;
            }
        }
	}

    public void Dropped()
    {
        justDropped = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.GetChild(0).GetComponent<MoveObjectUpAndDown>().enabled = true;
    }

    public void MoveToPosition(Transform destinationTransform)
    {
		destination = destinationTransform;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
		movingItem = true;
    }

    
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player" && !pickedUp && !collision.transform.GetComponent<Player>().IsDead())
        {
            MoveToPosition(collision.transform.GetChild(2));

            ownerShip = (collision.transform.GetComponent<Player>().IsPlayerTwo()) ? 2 : 1;

            collision.transform.GetComponent<Player>().GetItemInfo(gameObject);
        }
    }

    public void Shrink()
    {
        GetComponent<ShrinkObjectOnCall>().Shrink();
    }
    
    public int OwnerShip()
    {
        return ownerShip;
    }
}