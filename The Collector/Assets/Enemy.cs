using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    [Header("Acceleration Preferences")]
    public float speedBoost = 10f;
    public float boostEaseOutDuration = 5f;

    [Header("Player Follow Preferences")]
    public float playerFollowDistanceLimit = 15f;

    [Header("Waypoint Preferences")]
    public float waypointWaitTime = 2f;
    public float waypointFollowDistanceLimit = 2f;

    private float startingSpeed;
    private float speedStartTime;
    private float fadeTime;
    private float startWaitTime;
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
                NavAgent.destination = waypoints[GetRandomWaypointIndex()].transform.position;
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
                        NavAgent.destination = waypoints[GetRandomWaypointIndex()].transform.position;
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
            //player drops item

            //If player isn't meant to be one shotted
                //Player gets pushed a little bit
                //Enemy stops for a bit
                //Continues to follow
        }
    }

    private int GetRandomWaypointIndex()
    {
        return Random.Range(0, waypoints.Length);
    }
}
