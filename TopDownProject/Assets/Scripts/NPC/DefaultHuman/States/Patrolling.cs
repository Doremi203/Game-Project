using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : IState
{

    private Actor npc;
    private NavMeshAgent agent;

    private PatrollingPoint lastPoint;
    private PatrollingPoint targetPoint;

    public Patrolling(Actor npc, NavMeshAgent agent)
    {
        this.npc = npc;
        this.agent = agent;
    }

    public void OnEnter()
    {
        agent.autoBraking = false;
        agent.ResetPath();
        FindNextPoint();
    }

    public void OnExit()
    {
        agent.autoBraking = true;
        agent.ResetPath();
    }

    public void Tick()
    {
        RotationUpdate();

        if (targetPoint == null) return;

        if (GameUtilities.GetDistance2D(npc.transform.position, targetPoint.transform.position) < 0.5f)
            FindNextPoint();
    }

    private void RotationUpdate()
    {
        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
        {
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }
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