using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola,
    Curve
}

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkCustomMover : MonoBehaviour
{
    public OffMeshLinkMoveMethod Method = OffMeshLinkMoveMethod.Parabola;
    public AnimationCurve AnimCurve = new AnimationCurve();
    public float TravelDuration = 0.5f;
    public float ParabolaHeight = 4.0f;
    public Transform startPosDebug, endPosDebug;
    public float endPosReachedNormalDistance = 0.05f;
    NavMeshAgent agent;
    bool prevStatus = false;
    bool currStatus = false;
    float normalizedTime;

    OffMeshLinkData data;
    Vector3 endPos, startPos;

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
                normalizedTime = 0;
            }
            if (Method == OffMeshLinkMoveMethod.Teleport)
            {
                normalizedTime = 1;
            }
            if (Method == OffMeshLinkMoveMethod.NormalSpeed || Method == OffMeshLinkMoveMethod.Teleport)
            {
                if (NormalSpeed())
                    agent.CompleteOffMeshLink();
            }
            else if (Method == OffMeshLinkMoveMethod.Parabola)
                Parabola(ParabolaHeight);
            else if (Method == OffMeshLinkMoveMethod.Curve)
                Curve();
            if(normalizedTime >= 1.0)
                agent.CompleteOffMeshLink();
        }
        prevStatus = currStatus;
    }

    void SetStartEndPos()
    {
        data = agent.currentOffMeshLinkData;
        startPos = agent.transform.position;
        endPos = data.endPos + Vector3.up * agent.baseOffset;
        if(startPosDebug != null)
            startPosDebug.position = startPos;
        if (endPosDebug != null)
            endPosDebug.position = endPos;
    }

    bool NormalSpeed()
    {
        if (Vector3.Distance(agent.transform.position,endPos) > endPosReachedNormalDistance)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
            return false;
        }
        return true;
    }

    void Parabola(float height)
    {
        if (normalizedTime < 1.0f)
        {
            float yOffset = height * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / TravelDuration;
        }
    }

    void Curve()
    {
        if(normalizedTime < 1.0f)
        {
            float yOffset = AnimCurve.Evaluate(normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / TravelDuration;
        }
    }
}
