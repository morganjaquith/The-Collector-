using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour {

    GameObject mainCamera;
    bool carrying;
    GameObject carriedObject;
    public float distance;
    public float smooth;

    void Start ()
    {
        mainCamera=GameObject.FindWithTag("MainCamera");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(carrying)
        {
            carry(carriedObject);
            checkDrop();
        }
        else
        {
            pickup();
        }
        
    }

    void carry(GameObject o)
    {
        o.transform.position = Vector3.Lerp(o.transform.position,mainCamera.transform.position + mainCamera.transform.forward * distance,Time.deltaTime*smooth);

    }
    void pickup()
    {
        if(Input.GetMouseButtonDown(0))//Input.GetKeyDown(KeyCode.E)
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray =mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);//new Vector3(x, y)
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Pickupable p = hit.collider.GetComponent<Pickupable>();
                if(p !=null)
                {
                    carrying = true;
                    carriedObject = p.gameObject;
                    p.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                }
            }
        }
    }

    void checkDrop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dropObject();
        }

    }

    void dropObject()
    {
        carrying = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject = null;

    }
    //NOTE : Zone and GameManager scripts have not been created currently, but write the code as if it was finished, just comment out any red lines when done.
    //2nd NOTE : Player script will handle item dropping logic, any extra stuff I forget to mention is on me

    //This is an item script that's meant to be attached to an item object in the level.
    //When the player touches this item, the item will then move over the player's head, visually indicating that it's been taken.

    //OnCollisionEnter, check and see if the object that hit us is tagged "player".
    //If that's the case use GetComponent<Player>() to get the destination Transform for where this item is going.
    //Make it so that this object moves over to where the destination Transform is.
    //When the items makes it to it's destination, parent this object to the player. (Example transform.parent = player.transform)

    //Should have a float variable called 'points' (or equivalent). You can assign whatever value you want to 'points'
    //The Zone script will be grabbing this 'points' variable when this item is placed in the zone.

    //private void OnCollisionEnter(Collision collision)
    //{
    //if collision object tag == "player"

    //Get the destination of where the object is going (Transform) (from the player script)

    //Move item over to the destination that was given

    //}
}
