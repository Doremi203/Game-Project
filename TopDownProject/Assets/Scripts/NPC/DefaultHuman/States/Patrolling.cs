using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : IState, IStateEnterCallbackReciver, IStateExitCallbackReciver, IStateTickCallbackReciver
{

    private HumanAI ai;
    private Actor npc;
    private NavMeshAgent agent;

    private PatrollingPoint lastPoint;
    private PatrollingPoint targetPoint;

    public Patrolling(HumanAI ai, Actor npc, NavMeshAgent agent)
    {
        this.ai = ai;
        this.npc = npc;
        this.agent = agent;
    }

    public void OnEnter()
    {
        agent.autoBraking = false;
        agent.ResetPath();
        //agent.speed = ai.DefaultSpeed;
        FindNextPoint();
    }

    public void OnExit()
    {
        agent.autoBraking = true;
    }

    public void Tick()
    {
        npc.GetComponent<RotationController>().LookAtIgnoringY(agent.steeringTarget);

        if (targetPoint == null) return;

        if (GameUtilities.GetDistanceIgnoringY(npc.transform.position, targetPoint.transform.position) < 0.5f)
            FindNextPoint();
    }

    private void FindNextPoint()
    {
        if (targetPoint == null)
        {
            targetPoint = PatrollingPoint.GetClosestPoint(npc.transform.position);
        }
        else
        {
            List<PatrollingPoint> potentialPoints = new List<PatrollingPoint>();

            foreach (var item in targetPoint.PatrollingPoints)
            {
                if (item == targetPoint) continue;
                if (item == lastPoint) continue;
                potentialPoints.Add(item);
            }

            if (potentialPoints.Count > 0)
            {
                lastPoint = targetPoint;
                targetPoint = potentialPoints[Random.Range(0, potentialPoints.Count)];
            }
            else
            {
                lastPoint = targetPoint;
                targetPoint = lastPoint;
            }

        }

        if(targetPoint != null) agent.SetDestination(targetPoint.transform.position);
    }

}