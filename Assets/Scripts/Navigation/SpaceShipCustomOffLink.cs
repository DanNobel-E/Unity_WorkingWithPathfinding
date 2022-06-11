using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class SpaceShipCustomOffLink : MonoBehaviour
{
    
    public AnimationCurve AnimCurveX = new AnimationCurve();
    public AnimationCurve AnimCurveY = new AnimationCurve();
    public AnimationCurve AnimCurveXRot = new AnimationCurve();
    public AnimationCurve AnimCurveYRot = new AnimationCurve();
    public AnimationCurve AnimCurveZRot = new AnimationCurve();


    public float TravelDuration = 1f;
    public float rotMultiplier=1;
    public Transform Ship, Corridor;
    public float endPosReachedNormalDistance = 0.05f;
    NavMeshAgent agent;
    bool prevStatus = false;
    bool currStatus = false;
    float normalizedTime;
    float zSign;

    OffMeshLinkData data;
    Vector3 endPos, startPos;
    private bool firstHalf;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;

    }

    private void Update()
    {
        currStatus = agent.isOnOffMeshLink;
        if (currStatus)
        {
            if(prevStatus != currStatus)
            {
                SetStartEndPos();
                firstHalf = true;
                normalizedTime = 0;
                agent.updateRotation = false;

                float angle=  Vector3.SignedAngle(Ship.transform.forward, Corridor.position, Vector3.up);
                zSign=  Mathf.Sign(angle);
            }

                Curve();

            if (normalizedTime >= 1.0)
            {
                firstHalf = true;
                agent.CompleteOffMeshLink();
                agent.updateRotation = true;

            }
        }
        prevStatus = currStatus;
    }

    void SetStartEndPos()
    {
        data = agent.currentOffMeshLinkData;
        startPos = agent.transform.position;
        endPos = data.endPos + Vector3.up * agent.baseOffset;
      
    }
    void Curve()
    {
        if(normalizedTime < 1.0f)
        {
            float xOffset = AnimCurveX.Evaluate(normalizedTime) * Corridor.position.x;
            float yOffset = AnimCurveY.Evaluate(normalizedTime) *Corridor.position.y;
            float xRot = AnimCurveXRot.Evaluate(normalizedTime) * -30;
            float yRot = AnimCurveYRot.Evaluate(normalizedTime) * 5*zSign;
            float zRot = AnimCurveZRot.Evaluate(normalizedTime) * 90*-zSign;
            Vector3 offset = new Vector3(xOffset, yOffset, 0);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + offset;

           

            Quaternion rot = Quaternion.Euler(xRot, yRot, zRot);
            agent.transform.rotation *= rot;
            normalizedTime += Time.deltaTime / TravelDuration;
        }
    }
}
