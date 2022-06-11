using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathVisualizer : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Vector3 Offset;
    NavMeshPath currentPath;
    Vector3[] pathCorners;
    RaycastHit hitInfo;
    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {

        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                NavMeshPath newPath = new NavMeshPath();
                if (NavMesh.CalculatePath(Agent.transform.position,hitInfo.point, Agent.areaMask, newPath))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                        Agent.isStopped = true;
                    else
                        Agent.isStopped = false;

                    Agent.path = newPath;
                    currentPath = Agent.path;
                    NavMeshPathStatus status = Agent.pathStatus;
                    

                    if (status==NavMeshPathStatus.PathComplete)
                    {
                        lr.startColor = Color.green;
                        lr.endColor = Color.green;
                    }
                    else
                    {
                        lr.startColor = Color.yellow;
                        lr.endColor = Color.yellow;
                    }

                    if (currentPath.corners != null)
                    {
                        pathCorners = currentPath.corners;

                        lr.positionCount = pathCorners.Length + 1;

                        lr.SetPosition(0, Agent.transform.position+Offset);

                        int vIndex = 1;

                        for (int i = 0; i < pathCorners.Length; i++)
                        {
                            lr.SetPosition(vIndex, pathCorners[i]+Offset);
                            vIndex++;

                        }

                    }

                }
                else
                {

                    Agent.isStopped=true;

                    lr.startColor = Color.red;
                    lr.endColor = Color.red;

                    lr.positionCount = 2;

                    lr.SetPosition(0, Agent.transform.position+Offset);
                    lr.SetPosition(1, hitInfo.point+Offset);


                }



            }
        }

        if (Agent.pathStatus!= NavMeshPathStatus.PathInvalid && pathCorners!=null)
        {
            float remainingDist = (Agent.transform.position - pathCorners[pathCorners.Length - 1]).magnitude;
            if (!Agent.isStopped && remainingDist <= Agent.stoppingDistance)
            {
                lr.positionCount = 0;
            }
        }

     
    }






}

