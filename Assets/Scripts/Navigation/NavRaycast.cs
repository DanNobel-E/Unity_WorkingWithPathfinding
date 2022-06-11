using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NavRaycast : MonoBehaviour {
	public Transform start, end, blocked;
    public bool AllAreas; 
    public string AreaNameAccepted; //A bitfield mask specifying which NavMesh areas can be passed when tracing the ray

    NavMeshHit hit;
	bool blockedRay = false;
    int areaMask;

    void Update() {
        areaMask = AllAreas ? NavMesh.AllAreas : 1 << NavMesh.GetAreaFromName(AreaNameAccepted);

        blockedRay = NavMesh.Raycast(start.position, end.position, out hit, areaMask);
		Debug.DrawLine(start.position, end.position, blockedRay ? Color.red : Color.green);
        blocked.gameObject.SetActive(blockedRay);
        Debug.Log("hit.mask: "+ hit.mask);
        if (blockedRay)
        {
            blocked.position = hit.position;
        }
	}
}