using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class TurnThenMove : MonoBehaviour
{
    public float RotDuration;
    NavMeshAgent agent;
    RaycastHit hitInfo;
    float timer = 0;
    bool rotate;
    Vector3 target;
    Quaternion startRot;
    Quaternion endRot;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
                if (agent.CalculatePath(hitInfo.point, newPath))
                {
                    agent.path = newPath;
                    agent.isStopped = true;
                    agent.updateRotation = false;
                    rotate = true;

                    startRot = agent.transform.rotation;

                    target = new Vector3(agent.steeringTarget.x, agent.transform.position.y, agent.steeringTarget.z);
                    Vector3 dir = (target - agent.transform.position).normalized;
                    endRot = Quaternion.LookRotation(dir, agent.transform.up);

                }


            }
            else
            {
                agent.isStopped = true;

            }
        }
        if (rotate)
        {
            float fraction = timer / RotDuration;

            agent.transform.rotation = Quaternion.Slerp(startRot,endRot,fraction);

            timer+=Time.deltaTime;

            if (timer >= RotDuration)
            {
                agent.transform.rotation = endRot;
                timer = 0;
                rotate = false;
                agent.isStopped = false;
                agent.updateRotation = true;
            }
        }

        if(target!= null && agent.isStopped)
            Debug.DrawLine(agent.transform.position, target,Color.green);

        if (agent.steeringTarget != null)
            Debug.DrawLine(agent.transform.position, agent.steeringTarget, Color.red);

        Debug.DrawLine(agent.transform.position, agent.transform.position + (agent.transform.forward*3), Color.blue);

    }
}
