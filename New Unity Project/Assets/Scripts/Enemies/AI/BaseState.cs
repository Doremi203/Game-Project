using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
   
    public BaseState (GameObject target)
    {
        this.target = target;
    }

    protected GameObject target;

    public abstract void OnStateEnter();
    public abstract Type Tick();

}
