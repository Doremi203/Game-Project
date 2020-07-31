using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : IState
{

    private Turret turret;

    public TurretAttack(Turret turret)
    {
        this.turret = turret;
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }

}