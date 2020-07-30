using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : BaseState
{

    private TestEnemy baseEnemy;
    private float nextChangeTime;

    public WanderState(TestEnemy target) : base(target.gameObject)
    {
        baseEnemy = target;
    }

    public override void OnStateEnter()
    {
        Debug.Log("Wander");
        baseEnemy.Agent.isStopped = false;
        nextChangeTime = 0;
    }

    public override Type Tick()
    {
        Player player = GameObject.FindObjectOfType<Player>();

        if (player)
        {
            if (Vector3.Distance(baseEnemy.transform.position, player.transform.position) <= baseEnemy.DetectRadius)
            {
                return typeof(ChaseState);
            }
        }

        if(Time.time >= nextChangeTime)
        {
            ChangeTargetLocation();
        }

        return null;
    }

    private void ChangeTargetLocation()
    {
        Vector3 newDestination = new Vector3();
        newDestination.x = baseEnemy.transform.position.x + UnityEngine.Random.Range(-10f, 10f);
        newDestination.z = baseEnemy.transform.position.z + UnityEngine.Random.Range(-10f, 10f);
        baseEnemy.Agent.SetDestination(newDestination);
        nextChangeTime = Time.time + 2f;
    }

}