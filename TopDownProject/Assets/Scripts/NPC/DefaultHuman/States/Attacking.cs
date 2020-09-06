using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attacking : IState
{

    private Actor npc;
    private NavMeshAgent agent;
    private WeaponHolder weaponHolder;
    private NPC_BaseAI ai;
    private float nextPositionTime;
    private Vector3 currentTargetPosition;

    public Attacking(Actor npc, NavMeshAgent agent, WeaponHolder weaponHolder, NPC_BaseAI ai)
    {
        this.npc = npc;
        this.agent = agent;
        this.weaponHolder = weaponHolder;
        this.ai = ai;
    }

    public void OnEnter()
    {
        agent.enabled = true;
        nextPositionTime = 0;
    }

    public void OnExit() => weaponHolder.currentWeapon.Use(false);

    public void Tick()
    {
        Vector3 relativePos = ai.Target.transform.position - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
        {
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }

        //Debug.DrawLine(currentTargetPosition, currentTargetPosition + Vector3.up * 100f);

        if (Time.time >= nextPositionTime) FindNewLocation();

        weaponHolder.currentWeapon.Use(weaponHolder.currentWeapon.CanUse() && ai.GetAngleToTarget() < weaponHolder.currentWeapon.NpcAttackAngle);

    }

    private void FindNewLocation()
    {
        nextPositionTime = Time.time + 4f;

        Vector3 newPosition = new Vector3();

        newPosition = ai.Target.transform.position + Utils.GetRandomPositionInTorus(4f, weaponHolder.currentWeapon.NpcAttackDistance);
        newPosition.y = 0;

        currentTargetPosition = newPosition;
        agent.enabled = true;
        agent.SetDestination(newPosition);
    }

}