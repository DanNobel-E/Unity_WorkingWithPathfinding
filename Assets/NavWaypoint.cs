using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavWaypoint : MonoBehaviour
{
    public Transform Root;
    public float StayDuration=3f;


    NavMeshAgent navAgent;
    Transform[] waypoints;
    Transform currWaypoint;
    float timer;
    int waypointIndex;
    bool patrolling;
    Transform ObjToFollow;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = new Transform[Root.childCount];

        for (int i = 0; i < Root.childCount; i++)
        {
            waypoints[i] = Root.GetChild(i);
        }
        navAgent = GetComponent<NavMeshAgent>();

        waypointIndex = 0;
        currWaypoint = waypoints[waypointIndex];
        navAgent.destination = currWaypoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolling)
        {


            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {

                timer += Time.deltaTime;

                if (timer >= StayDuration)
                {
                    timer = 0;
                    waypointIndex++;

                    if (waypointIndex > waypoints.Length - 1)
                        waypointIndex = 0;

                    currWaypoint = waypoints[waypointIndex];
                    navAgent.destination = currWaypoint.position;
                }
            }
        }

        if ((transform.position - ObjToFollow.position).magnitude <= 10)
        {
            patrolling = true;
            navAgent.destination = ObjToFollow.position;
        }
        else
            patrolling = false;
    }
}
