using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavClick : MonoBehaviour {
	NavMeshAgent agent;
	RaycastHit hitInfo = new RaycastHit();
	public Transform hitInfoPoint;

	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray.origin, ray.direction, out hitInfo)) {
				agent.destination = hitInfo.point;
                if(hitInfoPoint != null)
    				hitInfoPoint.position = hitInfo.point;
			}
		}
	}
}
