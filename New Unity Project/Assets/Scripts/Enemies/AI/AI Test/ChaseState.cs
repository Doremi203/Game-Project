using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{

    private TestEnemy baseEnemy;

    public ChaseState(TestEnemy target) : base(target.gameObject)
    {
        baseEnemy = target;
    }

    public override void OnStateEnter()
    {
        Debug.Log("Chase");
        baseEnemy.Agent.isStopped = false;
    }

    public override Type Tick()
    {
        Player player = GameObject.FindObjectOfType<Player>();

        if (!player) return typeof(WanderState);

        float f = Vector3.Distance(baseEnemy.transform.position, player.transform.position);

        if (f > baseEnemy.DetectRadius) return typeof(WanderState);

        if (f <= 10)
        {
            if (baseEnemy.CanSeeThePlayer()) return typeof(AttackState);
        }

        baseEnemy.Agent.SetDestination(player.transform.position);

        return null;
    }

}
