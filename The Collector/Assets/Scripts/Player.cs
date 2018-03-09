using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float movementSensitivity = 0.5f;

	public Transform itemDestination;

    public float deadDuration = 3f;

    private bool dead;
    public bool dontDie;
    public bool holdingItem;

    private float xInput;
    private float zInput;
    private float deathStartTime;
    private float destroyTime;
    private float originalMovementSpeed;

    private Rigidbody rig;
    private Transform cameraObject;
    private GameObject item;

    private Vector3 forwardMovement;
    private Vector3 sideWaysMovement;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        originalMovementSpeed = movementSensitivity;
        cameraObject = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update () 
	{
        if (!dead)
        {

            //Get inputs
            xInput = Input.GetAxis("Horizontal") * movementSensitivity * Time.deltaTime;
            zInput = Input.GetAxis("Vertical") * movementSensitivity * Time.deltaTime;

            //Create vectors for the movement
            sideWaysMovement = new Vector3(cameraObject.transform.right.x * xInput, 0, cameraObject.transform.right.z * xInput);
            forwardMovement = new Vector3(cameraObject.transform.forward.x * zInput, 0, cameraObject.transform.forward.z * zInput);

            //Move the object using it's rigidbody 
            rig.MovePosition(sideWaysMovement + forwardMovement + transform.position);
            
            if (holdingItem && Input.GetButtonDown("Fire1"))
            {

                item.transform.parent = null;

                //Drop item in front of player by adding player's velocity to item
                item.transform.GetComponent<Rigidbody>().velocity += rig.velocity;

                //Return to normal movement speed
                movementSensitivity = originalMovementSpeed;

                holdingItem = false;
            }
        }
        else
        {

            destroyTime = (Time.time - deathStartTime) / deadDuration;

            if(destroyTime <= 0)
            {
                Destroy(this.gameObject);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if we collided with object tagged "item" && !holdingItem
        if(collision.transform.tag == "item" && !holdingItem)
        {

            item = collision.gameObject;

            //Move item above the player and parent the item to the player, indicating it's been taken
            //item.GetComponent<Item>().MoveToDestination(itemDestination);

            //half the movement sensitivity
            movementSensitivity /= 2;

            holdingItem = true;

        }
    }

    public void ApplyDamage()
    {
        if (!dontDie)
        {
            dead = true;

            Debug.Log("You're dead");

            deathStartTime = Time.time;

            //if (holdingItem) {  //drop it  }
            
            rig.constraints = RigidbodyConstraints.None;

        }
	}
	
}
