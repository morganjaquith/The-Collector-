using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
/// Enemy AI by: Michael Frye
/// 
/// On Start the AI will grab all objects tagged "Waypoint" in the scene.
/// 
/// It'll select a waypoint and go to it if player == null.
/// 
/// If the player enters the enemy's trigger (OnTriggerEnter), the enemy will get a boost of speed that'll decrease over time.
/// If the enemy touches the player (OnCollisionEnter), the enemy will kill the player.
/// 
/// If the player gets far enough from the AI, it'll stop persueing the player and player will equal null.
/// 
/// </summary>

public class Enemy : MonoBehaviour {

    [Header("Acceleration Preferences")]
    public float speedBoost = 10f;
    public float boostEaseOutDuration = 5f;

    [Header("Player Follow Preferences")]
    public float playerFollowDistanceLimit = 15f;

    [Header("Waypoint Preferences")]
    public float waypointWaitTime = 2f;
    public float waypointFollowDistanceLimit = 2f;
    public bool goToRandomWaypoint = false;

    [Header("Debug")]

    public bool stopped,paused;
    public float destinationDistance;
    public float startingSpeed;
    public float speedStartTime;
    public float fadeTime;
    public float startWaitTime;
    public int waypointIndexCount;
    public GameObject player;
    public Player playerScript;
    public GameObject[] waypoints;
    public NavMeshAgent NavAgent;
    private string assignedWaypointTag;
    private bool searchingForWaypoints;

    // Use this for initialization
    void Start ()
    {
        //Remember the wait time since waypointWaitTime gets modified during runtime
        startWaitTime = waypointWaitTime;

        //Get NavAgent from this object and remember the NavAgent's speed since that gets modified during runtime
        NavAgent = GetComponent<NavMeshAgent>();
        startingSpeed = NavAgent.speed;

        //Get every gameobject in the scene that's tagged "Waypoint"
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(waypoints.Length <= 0)
        {
            waypoints = GameObject.FindGameObjectsWithTag(assignedWaypointTag);
        }

        //If the game isn't paused
        if (!paused && waypoints.Length > 0)
        {
            //Keep track of the NavAgent's desination distance and if it's stopped
            stopped = NavAgent.isStopped;
            destinationDistance = Vector3.Distance(transform.position, NavAgent.destination);

            //If the player is close to the enemy
            if (player != null && !playerScript.IsDead())
            {

                //Tell the NavAgent exactly where the player is
                NavAgent.destination = player.transform.position;

                //If the player is far from the enemy pick a random waypoint and forget about the player
                if (destinationDistance >= playerFollowDistanceLimit)
                {
                    NavAgent.destination = waypoints[GetWaypointIndex()].transform.position;
                    player = null;
                }
                
                //If the enemy's speed has been boosted, slowly move the speed back to it's starting speed
                if (NavAgent.speed > startingSpeed)
                {
                    fadeTime = (Time.time - speedStartTime) / boostEaseOutDuration;
                    NavAgent.speed = Mathf.SmoothStep(speedBoost, startingSpeed, fadeTime);
                }
            }
            else
            {

                //If we're close enough to the waypoint, stop, else don't stop
                if (destinationDistance <= waypointFollowDistanceLimit && (player == null || playerScript.IsDead()))
                {
                    NavAgent.isStopped = true;

                    //If we've stopped
                    if (NavAgent.isStopped)
                    {
                        //Decrement the waypointWaitTime using deltaTime
                        waypointWaitTime -= Time.deltaTime;

                        //If waitTime has hit 0 or less
                        if (waypointWaitTime < 0)
                        {
                            //Pick another random waypoint and reset the waypointWaitTime
                            NavAgent.destination = waypoints[GetWaypointIndex()].transform.position;
                            waypointWaitTime = startWaitTime;
                        }
                    }
                }
                else
                {
                    NavAgent.isStopped = false;
                }
            }
        }
	}

    /// <summary>
    /// When the player enters the general area of enemy the AI chases the player
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //If what entered the trigger is tagged "Player"
        if(other.transform.tag == "Player")
        {
            //NOTE:
            //If there's a problem with triggerEntering when on the otherside of the wall, use a raycast condition to check if the enemy can see the player first

            //Set player to this gameobject
            player = other.gameObject;

            playerScript = player.GetComponent<Player>();

            //Set destination to this player object
            NavAgent.destination = player.transform.position;

            //If we've stopped for any reason, set stopped to false
            if (NavAgent.isStopped)
            {
                NavAgent.isStopped = false;
            }

            //Set the speedStartTime to this point in time
            speedStartTime = Time.time;

            //Give the NavAgent a speed boost
            NavAgent.speed = speedBoost;
        }
    }

    /// <summary>
    /// When the player hits the enemy damage is applied to the player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //If we've collided with the player
        if(collision.transform.tag == "Player" && !playerScript.IsDead())
        {

            //Set the navAgent's speed to it's starting speed
            NavAgent.speed = startingSpeed;

            //Tell the player object to applyDamage/die
            collision.transform.GetComponent<Player>().ApplyDamage();

            collision.rigidbody.AddForceAtPosition(transform.forward*12, collision.contacts[0].point);

            GetComponent<Rigidbody>().velocity = Vector3.zero;

            NavAgent.isStopped = true;
            
            //player is null
            player = null;
        }
    }

    /// <summary>
    /// Allows GameManager to edit these preferences during runtime
    /// </summary>
    /// <param name="newWaypointTime"></param>
    /// <param name="newWaypointFollowDistanceLimit"></param>
    /// <param name="newGoToRandomWaypointPref"></param>
    public void ChangeWaypointPreferences(float newWaypointTime, float newWaypointFollowDistanceLimit, bool newGoToRandomWaypointPref)
    {
        waypointWaitTime = newWaypointTime;
        waypointFollowDistanceLimit = newWaypointFollowDistanceLimit;
        goToRandomWaypoint = newGoToRandomWaypointPref;
    }

    /// <summary>
    /// Searches for waypoints with a given tage name
    /// </summary>
    /// <param name="tagName"></param>
    public void GetWaypointsWithNewTag(string tagName)
    {
        assignedWaypointTag = tagName;
        waypoints = GameObject.FindGameObjectsWithTag(tagName);
    }

    /// <summary>
    /// Returns an int representing a specific waypoint index.
    /// Returns a random index if goToRandomWaypoint is true,
    /// else it'll increment through the waypoints instead.
    /// </summary>
    /// <returns></returns>
    private int GetWaypointIndex()
    {
        if (goToRandomWaypoint)
        {
            return Random.Range(0, waypoints.Length);
        }
        else
        {
            if(waypointIndexCount >= waypoints.Length)
            {
                waypointIndexCount = 0;
            }

            return waypointIndexCount++;
        }
    }

    /// <summary>
    /// Pauses the Ai
    /// </summary>
    public void Pause()
    {
        if (!NavAgent.isStopped)
        {
            NavAgent.isStopped = true;
        }

        paused = true;
    }

    /// <summary>
    /// Unpauses the Ai
    /// </summary>
    public void Unpause()
    {
        NavAgent.isStopped = false;
        paused = false;
    }
}
