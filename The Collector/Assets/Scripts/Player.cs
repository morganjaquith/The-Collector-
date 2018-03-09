using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float movementSensitivity = 0.5f;
    public float rotationSensitivity = 0.25f;

	public Transform itemDestination;
	
    private float yRotInput;
    private float zInput;

    private Transform cameraObject;
    private GameObject item;

    private void Start()
    {
        cameraObject = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update () 
	{

        //Get inputs
        yRotInput = Input.GetAxis("Horizontal") * rotationSensitivity;
        zInput = Input.GetAxis("Vertical") * movementSensitivity;

        transform.eulerAngles += new Vector3(0, yRotInput,0); ;
        transform.position += new Vector3 (cameraObject.transform.forward.x*zInput,0,cameraObject.transform.forward.z * zInput);

        //if(holdingItem && Input.GetButtonDown("Drop"))
            //Unparent the item object

            //Drop item in front of player by adding player's velocity to item
            //item.transform.velocity += movementSpeed;

            //half the movement speed
            //movementSensitivity =* 2

            //holdingItem = false

    }

    private void OnCollisionEnter(Collision collision)
    {
        //if we collided with object tagged "item" && !holdingItem

            //item = collision.gameObject;

            //Move item above the player and parent the item to the player, indicating it's been taken

            //half the movement speed
            //movementSensitivity =/ 2

            //holdingItem = true
    }
	
	public void ApplyDamage()
	{
		//Play death animation or equivalent
		//Disable scripts and just have the player fall over
		//After a few seconds Destory this playerObject
	}
	
}
