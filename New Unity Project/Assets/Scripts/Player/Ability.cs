using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    public Sprite skillSprite;
    public UnityEvent Casted;

    private const float timeEpsilon = 0.0001f;
    public void Cast()
    {
        if (Time.time - nextUseTime > timeEpsilon)//(isAvaible)
        {
            DoCast();
            //isAvaible = false;
            nextUseTime = Time.time + coolDown;
            Casted.Invoke();
            
        }
    }
    
    
    protected abstract void DoCast();
    //public bool isAvaible { get; set; } = true;
    public abstract float coolDown { get; }
    public abstract string[] axises { get; }
    
    private void Awake()
    {
        nextUseTime = Time.time;
    }

    public float nextUseTime { get; set; }
    
    //protected abstract Sprite skillImage { get; }
}