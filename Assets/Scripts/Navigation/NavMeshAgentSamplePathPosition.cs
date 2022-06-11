using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentSamplePathPosition : MonoBehaviour {
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
        //Area mask will be always AllAreas
        int areaMask = 1 << NavMesh.GetAreaFromName(AreaNameToSample);

        SphereSamplingDistance.localScale = new Vector3(SamplingDistance * 2, SamplingDistance * 2, SamplingDistance * 2);
        //If we want to know the NavMesh area our agent is walking on, we can..
        //..look ahead a specified distance along the current path (looking at 0 distance will result in sampling where we are now). TRUE if terminated before reaching the position at maxDistance, false otherwise.
        if (agent.SamplePathPosition(NavMesh.AllAreas, SamplingDistance, out navMeshHit)){
            info = "agent.SamplePathPosition TRUE";
        }
        else
        {
            info = "agent.SamplePathPosition FALSE";
        }
        int areaIndex = -1;

        if (SampleAllAreas || ((navMeshHit.mask & areaMask) != 0))
        {
            areaIndex = IndexFromMask(navMeshHit.mask);
            info += string.Format(
                        "\nSearching for Area: {0} (id:{1})" +
                        "\nFound Area id: {2}" +
                        "\nAt distance: {3}" +
                        "\nAnd position: {4}" +
                        "\nArea Cost: {5}" +
                        "\nNormal: {6}" +
                        "\nSamplingDistance: {7}"
                        , SampleAllAreas ? "AllAreas" : AreaNameToSample,
                        SampleAllAreas ? "AllAreas" : NavMesh.GetAreaFromName(AreaNameToSample).ToString(),
                        areaIndex,
                        navMeshHit.distance, navMeshHit.position,
                        areaIndex >= 0 ? NavMesh.GetAreaCost(areaIndex).ToString() : "---",
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
