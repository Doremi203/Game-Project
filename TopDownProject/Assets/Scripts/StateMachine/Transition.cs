using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Transition 
{

    public Transition(IState to, Func<bool> condition)
    {
        To = to;
        this.condition = condition;
    }

    public IState To { get; }
    private Func<bool> condition;

    public bool ShouldTransist() => condition();

}