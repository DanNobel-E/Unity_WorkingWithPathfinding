using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshSamplePosition : MonoBehaviour {
    public bool displayInfo = true;
    public Vector2 offset;

    public float SamplingDistance;
    public Transform SphereSamplingDistance;
    public Transform SamplingPoint;
    public bool SampleAllAreas;
    public string AreaNameToSample;

    NavMeshAgent agent;
    NavMeshHit navMeshHit;
    string info;

    void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	void Update () {
        int areaMask;
        areaMask = SampleAllAreas ? NavMesh.AllAreas : 1 << NavMesh.GetAreaFromName(AreaNameToSample);
        SphereSamplingDistance.localScale = new Vector3(SamplingDistance * 2, SamplingDistance * 2, SamplingDistance * 2);
		//..or sampling the navMesh at a specific navPoint inside a specific radious range (looking at agent.transform.position with a radius of 0 will result in sampling where we are now). True if a nearest point is found.
		if (NavMesh.SamplePosition (agent.transform.position, out navMeshHit, SamplingDistance, areaMask)) {
            info = "NavMesh.SamplePosition TRUE";
        }
        else
        {
            info = "NavMesh.SamplePosition FALSE";
        }
        if (SampleAllAreas || ((navMeshHit.mask & areaMask) != 0))
        {
            int areaIndex;
        areaIndex = IndexFromMask(navMeshHit.mask);

        //NavMeshOwner is valid only if we use the New NavSystem
        string navMeshOwnerName;
        if (agent.navMeshOwner != null)
            navMeshOwnerName = agent.navMeshOwner.name;
        else
            navMeshOwnerName = "---";

        info += string.Format(
                    "\nSearching for Area: {0} (id:{1})" +
                    "\nFound Area id: {2}" +
                    "\nAt distance: {3}" +
                    "\nAnd position: {4}" +
                    "\nArea Cost: {5}" +
                    "\nNormal: {6}" +
                    "\nSamplingDistance: {7}"
                    , AreaNameToSample, NavMesh.GetAreaFromName(AreaNameToSample),
                    areaIndex, navMeshHit.distance, navMeshHit.position,
                    areaIndex >=0 ? NavMesh.GetAreaCost(areaIndex).ToString() : "---",
                    navMeshHit.normal, 
                    SamplingDistance);
            SamplingPoint.gameObject.SetActive(true);
            SamplingPoint.position = navMeshHit.position;
        }
        else
            SamplingPoint.gameObject.SetActive(false);
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

    //Test for each possible area mask if it is the same of the current one.
    //Remember: the areaMask is encoded with a 32bit integer. For example, for Areamask 3 we will have:
    //Areamask = 00000000 00000000 00000000 00000100
    private int IndexFromMask(int mask) {
		for (int i = 0; i < 32; ++i){
			if ((1 << i) == mask)
				return i;
		}
		return -1;
	}
}
