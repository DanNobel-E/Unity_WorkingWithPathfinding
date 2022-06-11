using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavFollower : MonoBehaviour {
	NavMeshAgent agent;
	public Transform objToFollowT;

	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		agent.destination = objToFollowT.position;
	}
}
