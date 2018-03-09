using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy AI by: Michael Frye
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

    private float startingSpeed;
    private float speedStartTime;
    private float fadeTime;
    private float startWaitTime;
    private int waypointIndexCount;
    private GameObject player;
    private GameObject[] waypoints;
    private NavMeshAgent NavAgent;

	// Use this for initialization
	void Start ()
    {
        startWaitTime = waypointWaitTime;
        NavAgent = GetComponent<NavMeshAgent>();
        startingSpeed = NavAgent.speed;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
	}
	
	// Update is called once per frame
	void Update ()
    {

        //If the player is close to the enemy
		if(player != null)
        {
            NavAgent.destination = player.transform.position;

            //If the player is far from the enemy pick a random waypoint and forget about the player
            if (NavAgent.remainingDistance >= playerFollowDistanceLimit)
            {
                NavAgent.destination = waypoints[GetWaypointIndex()].transform.position;
                player = null;
            }
            //If acceleration has been boosted, fade it out
            else if (NavAgent.speed > startingSpeed)
            {
                fadeTime = (Time.time - speedStartTime) / boostEaseOutDuration;
                NavAgent.acceleration = Mathf.SmoothStep(speedBoost, startingSpeed, fadeTime);
            }
        }
        else
        {
            //If we're close enough to the waypoint, stop
            if(NavAgent.remainingDistance <= waypointFollowDistanceLimit)
            {
                NavAgent.isStopped = true;

                //If we've stopped wait a few given amount of time then pick another waypoint
                if(NavAgent.isStopped)
                {
                    waypointWaitTime -= Time.deltaTime;

                    if(waypointWaitTime <= 0)
                    {
                        NavAgent.destination = waypoints[GetWaypointIndex()].transform.position;
                        waypointWaitTime = startWaitTime;
                        NavAgent.isStopped = false;
                    }
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
        if(other.transform.tag == "Player")
        {
            player = other.gameObject;

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
            //player.getcomponent<Player>().ApplyDamage()
            //player = null;
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
