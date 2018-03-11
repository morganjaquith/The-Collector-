using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Player Preferences")]
    public float movementSensitivity = 0.5f;
    public float deathDuration = 3f;

	public Transform itemDestination;

    [Header("Debug Preferences")]
    public bool dontDie;

    [Header("Debug")]
    public bool dead;
    public bool holdingItem;

    private float xInput;
    private float zInput;
    private float originalMovementSpeed;

    private Rigidbody rig;
    private Transform cameraObject;
    public GameObject pauseMenu;
    private GameObject item;

    private Vector3 ZMovement;
    private Vector3 XMovement;

    private void Start ()
    {
        //Find the pause menu in the scene and disable it
        pauseMenu = GameObject.Find("Canvas");
        pauseMenu.SetActive(false);

        //Keep track of the original movementSensitivity since it'll be modified during runtime (item will cause movementSensitivity to decrease)
        originalMovementSpeed = movementSensitivity;

        //Get the rigibody from this gameObject and get the cameraObject 
        rig = GetComponent<Rigidbody>();
        cameraObject = transform.GetChild(0);
    }

    // Update is called once per frame
    void FixedUpdate () 
	{
        //If the player isn't dead
        if (!dead)
        {
            //Get inputs
            xInput = Input.GetAxis("Horizontal") * movementSensitivity * Time.deltaTime;
            zInput = Input.GetAxis("Vertical") * movementSensitivity * Time.deltaTime;

            //Create vectors for the movement
            XMovement = new Vector3(cameraObject.transform.right.x * xInput, 0, cameraObject.transform.right.z * xInput);
            ZMovement = new Vector3(cameraObject.transform.forward.x * zInput, 0, cameraObject.transform.forward.z * zInput);

            //Move the object using it's rigidbody 
            rig.MovePosition(XMovement + ZMovement + transform.position);
            
            //If the player is holding and item and left mouse button / "Fire1" is pressed
            if (holdingItem && Input.GetButtonDown("Fire1"))
            {
                //Unparent the item from the player
                item.transform.parent = null;

                //Drop item in front of player by adding player's velocity to item
                item.transform.GetComponent<Rigidbody>().velocity += rig.velocity;

                //Return to normal movement speed
                movementSensitivity = originalMovementSpeed;

                //Indicate we aren't holding an item anymore
                holdingItem = false;
            }
            //If the player hit the escape key, 'pause' the game
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Enable pause menu
                pauseMenu.SetActive(true);

                //Call the pause function
                pauseMenu.GetComponent<MenuOptions>().PauseObjects();
            }
        }
        else
        {
            //decrement deathDuration using time
            deathDuration -= Time.deltaTime;

            //if deathDuration is 0 or less
            if(deathDuration <= 0)
            {
                //Destroy this gameobject
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// When the player collides with an object, check to see if it's an "item".
    /// If we aren't already holding an item, 'take' that item object.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter (Collision collision)
    {
        //if we collided with object tagged "item" and we aren't currently holding an item
        if(collision.transform.tag == "item" && !holdingItem)
        {
            //We've found an item
            item = collision.gameObject;

            //Call a function on the item to move item to a point above the player 
            //item.GetComponent<Item>().MoveToDestination(itemDestination);
            
            //Parent the item to the player, indicating it's been taken
            item.transform.parent = transform;

            //half the movement sensitivity
            movementSensitivity /= 2;

            //Indicate that we are holding an item
            holdingItem = true;

        }
    }

    /// <summary>
    /// 'Kills' the player by calling a funcion on the thirdPersonCamera, setting rigibody constraints to none, and setting dead to true.
    /// </summary>
    public void ApplyDamage ()
    {
        //If the dontDie debug variable is false
        if (!dontDie)
        {
            //Tell the thirdPersonCamera that the player is dead
            cameraObject.GetComponent<ThirdPersonCamera>().PlayerIsDead();

            //If we are holding an item currently
            if(holdingItem)
            {
                //drop it  
            }
            
            //Set the rigibody constraints on the player to 'none' so it falls over
            rig.constraints = RigidbodyConstraints.None;

            //Indicate that the player is dead
            dead = true;

            //Output message to Unity console
            Debug.Log("You're dead");
        }
	}
	
}
