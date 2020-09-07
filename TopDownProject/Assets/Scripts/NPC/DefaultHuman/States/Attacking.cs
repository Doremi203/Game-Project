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
        // Возможно стоит записывать stoppingDistance
        agent.stoppingDistance = weaponHolder.currentWeapon.NpcAttackDistance;
        nextPositionTime = 0;
    }

    public void OnExit()
    {
        agent.stoppingDistance = 0;
        weaponHolder.currentWeapon.Use(false);
    }

    public void Tick()
    {
        UpdateRotation();
        TryShoot();

        //Debug.DrawLine(currentTargetPosition, currentTargetPosition + Vector3.up * 100f);

        //if (Time.time >= nextPositionTime) FindNewLocation();

    }

    private void TryShoot()
    {
        float _weaponAttackAngle = weaponHolder.currentWeapon.NpcAttackAngle;
        bool _shouldShoot = weaponHolder.currentWeapon.CanUse() && ai.AngleToPlayer() <= _weaponAttackAngle;
        weaponHolder.currentWeapon.Use(_shouldShoot);
    }

    private void UpdateRotation()
    {
        Player _player = Player.Instance;
        Vector3 relativePos = _player.transform.position - npc.transform.position;
        relativePos.y = 0;
        if (relativePos != Vector3.zero)
        {
            npc.desiredRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }
    }

    private void FindNewLocation()
    {
        nextPositionTime = Time.time + 4f;

        Vector3 newPosition = new Vector3();

        //newPosition = ai.Target.transform.position + GameUtilities.GetRandomPositionInTorus(1f, weaponHolder.currentWeapon.NpcAttackDistance);
        newPosition.y = 0;

        currentTargetPosition = newPosition;
        agent.enabled = true;
        agent.SetDestination(newPosition);
    }

}