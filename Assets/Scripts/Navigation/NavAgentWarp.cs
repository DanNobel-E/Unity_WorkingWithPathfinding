using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentWarp : MonoBehaviour {
	NavMeshAgent agent;
	public bool performWarp;
    public Transform warpDestination;

	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	void Update () {
		if (performWarp) {
			performWarp = false;
			agent.Warp (warpDestination.position);
		}
	}
}
