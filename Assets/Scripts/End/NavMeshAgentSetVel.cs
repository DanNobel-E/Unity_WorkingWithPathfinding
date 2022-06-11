using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentSetVel : MonoBehaviour
{
    public float speedT = 10;
    public float speedR = 50;
    public string HAxisName = "Horizontal";
    public string VAxisName = "Vertical";
    NavMeshAgent nav;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float HVal = Input.GetAxis(HAxisName);
        float VVal = Input.GetAxis(VAxisName);

        Vector3 velocity = new Vector3(HVal, 0, VVal).normalized;
        nav.velocity = velocity * nav.speed * Time.deltaTime;

        Debug.DrawLine(transform.position, transform.position + (velocity*2), Color.yellow);

    }


}
