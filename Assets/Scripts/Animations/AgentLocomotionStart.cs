using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class AgentLocomotionStart : MonoBehaviour {
	Animator anim;
	NavMeshAgent agent;
	Vector2 velocity;

	//--
	public bool RootM_byAgent, RootM_byAnimations;
	public float VelBoost;
    Vector3 worldDeltaPosition;

    //--
    public bool PullAvatarTowardsAgent, PullAgentTowardsAvatar;

	void Start () {
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    void Update () {
		//--- Update BTrees Animations looking at NavAgent movements

        // Map 'worldDeltaPosition' to local space
		
		// Update Animator parameters

		// Advanced - LookAt
		AgentLookAtStart lookAt = GetComponent<AgentLookAtStart> ();
		if (lookAt)
            //We add + transform.forward because otherwise while in IDLE the Agent would look at his feet
            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;
	}

	void OnAnimatorMove () {
        if (RootM_byAgent) {
			// Update postion using agent position
		}
		else if(RootM_byAnimations){
			// Update position based on animations movements, using navigation surface height
		}
	}
}
