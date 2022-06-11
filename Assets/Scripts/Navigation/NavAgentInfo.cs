using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentInfo : MonoBehaviour
{
    public bool displayInfo = true;
    public Vector2 offset;

    public bool updateRotation, updatePosition, simulateUpdatePosition;
    public Transform agentDestination, agentNextPosition, steeringTargetPos;
    public Transform[] pathCorners;

    string info;
    NavMeshAgent navAgent;
    Vector3 velocity, desiredVelocity, nextPosition, steeringTarget;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {

        if (steeringTargetPos == null || agentDestination == null || agentNextPosition == null)
        {
            Debug.Log("Please set all the public Transform references");
            return;
        }
        navAgent.updateRotation = updateRotation;
        navAgent.updatePosition = updatePosition;
        //Check hasPath agent property
        float pathLength = 0;
        int pathCornersNum = 0;
        if (navAgent.hasPath)
        {
            agentDestination.position = navAgent.destination;
            agentNextPosition.position = nextPosition;
            //steeringTarget is the next corner along the path
            steeringTargetPos.position = navAgent.steeringTarget;
            Vector3[] corners = navAgent.path.corners;
            if (pathCorners != null && pathCorners.Length >= corners.Length) { 
                pathCornersNum = corners.Length;
                for (int i = 0; i < pathCorners.Length - 1; i++)
                {
                    if(pathCorners[i] != null)
                        pathCorners[i].gameObject.SetActive(false);

                }
                for (int i = 0; i < corners.Length - 1; i++)
                {
                    pathLength += Mathf.Abs((corners[i] - corners[i + 1]).magnitude);
                    if (pathCorners[i] != null) { 
                        pathCorners[i].position = corners[i];
                        pathCorners[i].gameObject.SetActive(true);
                    }
                }
            }
        }

        float acceleration = navAgent.acceleration;
        velocity = navAgent.velocity;
        desiredVelocity = navAgent.desiredVelocity;
        nextPosition = navAgent.nextPosition;
        if (simulateUpdatePosition)
            navAgent.transform.position = nextPosition;
        steeringTarget = navAgent.steeringTarget;
        int areaMask = navAgent.areaMask;
        bool hasPath = navAgent.hasPath;
        bool isOnNavMesh = navAgent.isOnNavMesh;
        bool isOnOffMeshLink = navAgent.isOnOffMeshLink;
        bool pathPending = navAgent.pathPending;
        bool isPathStale = navAgent.isPathStale;
        NavMeshPathStatus pathStatus = navAgent.pathStatus;
        float remainingDistance = navAgent.remainingDistance;
        info = string.Format("NavMeshAgentInfo" +
            "\nvelocity: {0}" +
            "\ndesiredVelocity: {1}" +
            "\nnextPosition: {2}" +
            "\nsteeringTarget: {3}" +
            "\nareaMask: {4}" +
            "\nhasPath: {5}" +
            "\npathPending: {6}" +
            "\nisPathStale: {7}" +
            "\npathStatus: {8}" +
            "\nisOnNavMesh: {9}" +
            "\nisOnOffMeshLink: {10}" +
            "\nremainingDistance: {11}" +
            "\npathLength: {12}" +
            "\npathCornersNum: {13}" +
            "\nupdatePosition: {14}" +
            "\nupdateRotation: {15}"
            , velocity, desiredVelocity, nextPosition, steeringTarget, areaMask,
            hasPath, pathPending, isPathStale, pathStatus, isOnNavMesh, isOnOffMeshLink,
            remainingDistance, pathLength, pathCornersNum, updatePosition, updateRotation);
    }

    void OnGUI()
    {
        if (!displayInfo) return;
        GUILayout.BeginHorizontal();
        GUILayout.Space(offset.x);
        GUILayout.BeginVertical();
        GUILayout.Space(offset.y);
        GUILayout.BeginVertical("box");
        GUILayout.Label(info);
        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}
