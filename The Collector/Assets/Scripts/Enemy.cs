using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
/// Enemy AI by: Michael Frye
/// 
/// On Start the AI will grab all objects tagged "Waypoint" in the scene
/// 
/// It'll select a waypoint and go to it if player == null
/// 
/// If the player enters the enemy's trigger (OnTriggerEnter), the enemy will get a boost of speed that'll decrease over time
/// If the enemy touches the player (OnCollisionEnter), the enemy will kill the player
/// 
/// If the player gets far enough from the AI, it'll stop persueing the player and player will equal null
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

    public bool stopped;
    public float destinationDistance;
    public float startingSpeed;
    public float speedStartTime;
    public float fadeTime;
    public float startWaitTime;
    public int waypointIndexCount;
    public GameObject player;
    public GameObject[] waypoints;
    public NavMeshAgent NavAgent;

	// Use this for initialization
	void Start ()
    {
        startWaitTime = waypointWaitTime;
        NavAgent = GetComponent<NavMeshAgent>();
        startingSpeed = NavAgent.speed;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        stopped = NavAgent.isStopped;
        destinationDistance = Vector3.Distance(transform.position, NavAgent.destination);

        //If the player is close to the enemy
        if (player != null)
        {

            NavAgent.destination = player.transform.position;

            //If the player is far from the enemy pick a random waypoint and forget about the player
            if (destinationDistance >= playerFollowDistanceLimit)
            {
                NavAgent.destination = waypoints[GetWaypointIndex()].transform.position;
                player = null;
            }
            //If acceleration has been boosted, fade it out
            else if (NavAgent.speed > startingSpeed)
            {
                fadeTime = (Time.time - speedStartTime) / boostEaseOutDuration;
                NavAgent.speed = Mathf.SmoothStep(speedBoost, startingSpeed, fadeTime);
            }
        }
        else
        {

            //If we're close enough to the waypoint, stop
            if (destinationDistance <= waypointFollowDistanceLimit && player == null)
            {
                NavAgent.isStopped = true;

                //If we've stopped wait a few given amount of time then pick another waypoint
                if (NavAgent.isStopped)
                {
                    waypointWaitTime -= Time.deltaTime;

                    if (waypointWaitTime < 0)
                    {
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

    /// <summary>
    /// When the player enters the general area of enemy the AI chases the player
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            player = other.gameObject;

            NavAgent.destination = player.transform.position;

            if (NavAgent.isStopped)
            {
                NavAgent.isStopped = false;
            }

            speedStartTime = Time.time;
            NavAgent.speed = speedBoost;
        }
    }

    /// <summary>
    /// When the player hits the enemy damage is applied to the player
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            //If there's a problem with collisionEntering when on the otherside of the wall, use a raycast in here to check if the enemy can see the player

            NavAgent.speed = startingSpeed;
            collision.transform.GetComponent<Player>().ApplyDamage();
            player = null;
        }
    }

    /// <summary>
    /// Returns an int representing a specific waypoint index
    /// Returns a random index if goToRandomWaypoint is true,
    /// else it'll increment through each waypoint instead
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
}
