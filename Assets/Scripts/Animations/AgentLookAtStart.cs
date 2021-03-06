// LookAt.cs
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class AgentLookAtStart : MonoBehaviour {
	public Transform head = null;
	public Vector3 lookAtTargetPosition; //Set by LocomotionSimpleAgent.cs

	//Good values: 1f, 0.2f, 0.5f, 0.7f, 0.5f
	public float lookAtW, bodyW, headW, eyesW, clampW; 
    public float LookDeltaSpeed;
	Vector3 lookAtPosition;
	Animator anim;

	void Start ()
	{
		anim = GetComponent<Animator> ();
        head = anim.GetBoneTransform(HumanBodyBones.Head);
		lookAtTargetPosition = head.position + transform.forward;
		lookAtPosition = lookAtTargetPosition;
	}

	void OnAnimatorIK ()
	{
        //NB: At runtime, lookAtTargetPosition is updated by AgentLocomotion.cs!

        //...do Stuff
    }
}
