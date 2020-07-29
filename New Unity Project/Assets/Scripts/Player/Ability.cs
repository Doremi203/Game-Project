using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    private const float timeEpsilon = 0.0001f;
    public void Cast()
    {
        if (Time.time - nextUseTime > timeEpsilon)//(isAvaible)
        {
            DoCast();
            //isAvaible = false;
            nextUseTime = Time.time + coolDown;
        }
    }

    protected abstract void DoCast();
    //public bool isAvaible { get; set; } = true;
    public abstract float coolDown { get; }

    private void Awake()
    {
        nextUseTime = Time.time;
    }

    public float nextUseTime { get; set; }

}