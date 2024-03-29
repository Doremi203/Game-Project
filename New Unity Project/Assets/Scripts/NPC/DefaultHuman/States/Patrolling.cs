﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : IState
{

    private HumanNPC npc;
    private NavMeshAgent agent;
    private NPC_BaseAI ai;

    private float nextFindTargetTime;

    private PatrollingPoint lastPoint;
    private PatrollingPoint targetPoint;

    public Patrolling(HumanNPC newTestNPC, NavMeshAgent agent, NPC_BaseAI ai)
    {
        this.npc = newTestNPC;
        this.agent = agent;
        this.ai = ai;
    }

    public void OnEnter()
    {
        agent.enabled = true;
        FindNextPoint();
        nextFindTargetTime = 0;
    }

    public void OnExit()
    {
        agent.enabled = false;
    }

    public void Tick()
    {
        if (Time.time >= nextFindTargetTime) TryFindTarget();

        if (targetPoint == null) return;

        Vector3 relativePos = agent.steeringTarget - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
        {
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }

        if (Utils.GetDistance2D(npc.transform.position, targetPoint.transform.position) < 0.5f)
        {
            FindNextPoint();
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

            foreach (var item in targetPoint.patrollingPoints)
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

    private void TryFindTarget()
    {
        ai.Target = ai.GetClosestEnemyActor();
        nextFindTargetTime = Time.time + 0.5f;
    }

}