using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIdle : IState
{

    private Turret turret;
    private float nextFindTargetTime;

    public TurretIdle(Turret turret)
    {
        this.turret = turret;
    }

    public void OnEnter()
    {
        nextFindTargetTime = 0f;
    }

    public void OnExit()
    {
       
    }

    public void Tick()
    {
        if (Time.time >= nextFindTargetTime)
        {
            turret.TryFindTarget();
            nextFindTargetTime = Time.time + 0.5f;
        }
    }

}