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

    public abstract Type Tick();

}
