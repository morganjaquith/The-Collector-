using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Player Preferences")]
    public float movementSensitivity = 0.5f;
    public float deathDuration = 3f;

    public GameObject playerMesh;
	public Transform itemDestination;

    [Header("Debug Preferences")]
    public bool dontDie;

    [Header("Debug")]
    public bool dead;
    public bool holdingItem;
    public bool paused;
    public bool isPlayer2;
    public bool isNotSinglePlayer;
    public bool usingKeyboard = true;

    private float xInput;
    private float zInput;
    private float originalMovementSpeed;

    private Rigidbody rig;
    private ThirdPersonCamera cameraObject;
    private GameObject pauseMenu;
    private GameObject item;

    private Vector3 ZMovement;
    private Vector3 XMovement;

    private void Awake ()
    {
        //Find the pause menu in the scene and disable it
        pauseMenu = GameObject.Find("Canvas");

        //Keep track of the original movementSensitivity since it'll be modified during runtime (item will cause movementSensitivity to decrease)
        originalMovementSpeed = movementSensitivity;

        //Get the rigibody from this gameObject and get the cameraObject 
        rig = GetComponent<Rigidbody>();
        cameraObject = transform.GetChild(0).GetComponent<ThirdPersonCamera>();

        int inputValue;

        if(!isPlayer2)
        {
            inputValue = PlayerPrefs.GetInt("PlayerOneInputDevice");
        }
        else
        {
            inputValue = PlayerPrefs.GetInt("PlayerTwoInputDevice");
        }


        usingKeyboard = (inputValue == 1) ? true : false;

    }

    // Update is called once per frame
    void FixedUpdate () 
	{
        if (!paused)
        {
            //If the player isn't dead
            if (!dead)
            {
                if (usingKeyboard)
                {
                    //Get inputs
                    xInput = Input.GetAxis("Horizontal") * movementSensitivity * Time.deltaTime;
                    zInput = -Input.GetAxis("Vertical") * movementSensitivity * Time.deltaTime;
                }
				else
				{
                    //Controller Inputs
                    xInput = Input.GetAxis("ControllerHorizontal") * movementSensitivity * Time.deltaTime;
                    zInput = Input.GetAxis("ControllerVertical") * movementSensitivity * Time.deltaTime;
                }

                //Create vectors for the movement
                XMovement = new Vector3(playerMesh.transform.right.x * xInput, 0, playerMesh.transform.right.z * xInput);
                ZMovement = new Vector3(playerMesh.transform.forward.x * zInput, 0, playerMesh.transform.forward.z * zInput);

                //Move the object using it's rigidbody 
                rig.MovePosition(XMovement + ZMovement + transform.position);

                //If the player is holding and item and left mouse button / "Fire1" is pressed
                if (holdingItem && ((Input.GetButtonDown("Fire1") && usingKeyboard )||( Input.GetButton("ControllerFire1") && !usingKeyboard)))
                {
                    DropItem();
                }
                //If the player hit the cancel key, 'pause' the game
                else if (Input.GetButtonDown("Cancel") && !isNotSinglePlayer || (Input.GetButtonDown("CancelController") && !usingKeyboard && !isNotSinglePlayer))
                {
                    pauseMenu.SetActive(true);

                    //Enable pause menu
                    pauseMenu.transform.GetChild(1).gameObject.SetActive(true);

                    //Call the pause function
                    pauseMenu.GetComponent<MenuOptions>().PauseObjects(!usingKeyboard);

                    GameObject.Find("GameUI").GetComponent<GameManager>().Pause();
                }
            }
            else
            {
                //decrement deathDuration using time
                deathDuration -= Time.deltaTime;

                //if deathDuration is 0 or less
                if (deathDuration <= 0)
                {
                    //Destroy this gameobject
                    Destroy(this.gameObject);
                }
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

    private void DropItem()
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

    public void Pause(bool pauseThirdPersonCamera = true)
    {
        if (pauseThirdPersonCamera)
        {
            cameraObject.Pause();
        }

        paused = true;
    }

    public void Unpause(bool pauseThirdPersonCamera = true)
    {
        if (pauseThirdPersonCamera)
        {
            cameraObject.Unpause();
        }

        paused = false;
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
            cameraObject.PlayerIsDead();

            //If we are holding an item currently
            if(holdingItem)
            {
                //drop it
                DropItem();
            }
            
            //Set the rigibody constraints on the player to 'none' so it falls over
            rig.constraints = RigidbodyConstraints.None;

            //Indicate that the player is dead
            dead = true;
        }
	}
}