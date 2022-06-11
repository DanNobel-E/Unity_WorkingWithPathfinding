using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

//Orient NavMeshAgent according to terrain
public class NavAgentFloorOrientation : MonoBehaviour {
	NavMeshAgent agent;
	public bool QLookRotSmooth, useFloorNormalAsUpVector;
	public float smoothFactor;
	public Transform nextPositionObj;
	Vector3 prevPosition, prevDirection, direction;

	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.updateRotation = false;
		prevPosition = agent.nextPosition;
	}

	void FixedUpdate () {
		if (!agent.hasPath)
			return;
		
		//Calculate the direction of the agent: the agent is going from the previous position to the next position
		direction = agent.nextPosition - prevPosition;

		//If the direction vector is too small, we could have problems with lookRotation functions. This is why
		//we store the last non-zero valid direction
		if (direction.magnitude <= 0.1f)
			direction = prevDirection;

		prevPosition = agent.transform.position;
		prevDirection = direction;

		RaycastHit hitInfo;
		if (!Physics.Raycast (transform.position, Vector2.down, out hitInfo))
			return;

		//Draw the terrain normal
		Debug.DrawLine (hitInfo.point, (hitInfo.point + hitInfo.normal * 3), Color.green);

		//We have to orient the Mesh of our agent (which is our first child), not the agent itself
		Transform childT = transform.GetChild (0);

		if (!QLookRotSmooth) {
			if (useFloorNormalAsUpVector)
				//In this case, when we are climbing stair steps, we orient our agent parallel to the floor
				childT.rotation = Quaternion.LookRotation (Vector3.ProjectOnPlane (direction, hitInfo.normal), hitInfo.normal);
			else
				//In this case, when we are climbing stair steps, we orient our agent upstairs/downstairs
				childT.rotation = Quaternion.LookRotation (direction, hitInfo.normal);
		}
		else {
			if(useFloorNormalAsUpVector)
				childT.rotation = Quaternion.Slerp (childT.rotation, Quaternion.LookRotation (Vector3.ProjectOnPlane (direction, hitInfo.normal), hitInfo.normal), Time.deltaTime * smoothFactor);
			else
				childT.rotation = Quaternion.Slerp (childT.rotation, Quaternion.LookRotation (direction, hitInfo.normal), Time.deltaTime * smoothFactor);
		}

	}
}
