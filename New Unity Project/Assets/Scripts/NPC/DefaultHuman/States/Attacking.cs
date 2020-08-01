using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attacking : IState
{

    private NewTestNPC npc;
    private NavMeshAgent agent;
    private WeaponHolder weaponHolder;
    private float nextPositionTime;
    private Vector3 currentTargetPosition;

    public Attacking(NewTestNPC newTestNPC, NavMeshAgent agent, WeaponHolder weaponHolder)
    {
        npc = newTestNPC;
        this.agent = agent;
        this.weaponHolder = weaponHolder;
    }

    public void OnEnter()
    {
        agent.enabled = true;
        nextPositionTime = 0;
    }

    public void OnExit()
    {
        weaponHolder.currentWeapon.Use(false);
        agent.enabled = false;
    }

    public void Tick()
    {
        Vector3 relativePos = npc.Target.transform.position - npc.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        npc.desireRotation = rotation;

        Debug.DrawLine(currentTargetPosition, currentTargetPosition + Vector3.up * 100f);

        if (Time.time >= nextPositionTime) FindNewLocation();
        //if (Vector3.Distance(npc.transform.position, currentTargetPosition) < 0.5f) FindNewLocation();

        weaponHolder.currentWeapon.Use(weaponHolder.currentWeapon.CanUse());
    }

    private void FindNewLocation()
    {
        nextPositionTime = Time.time + 4f;

        Vector3 newPosition = new Vector3();

        //newPosition = npc.Target.transform.position + UnityEngine.Random.onUnitSphere * 10f;

        newPosition = npc.Target.transform.position + Utils.GetRandomPositionInTorus(4f, npc.AttackRange);
        newPosition.y = 0;

        currentTargetPosition = newPosition;
        agent.enabled = true;
        agent.SetDestination(newPosition);
    }

}