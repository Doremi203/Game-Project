using System;
using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
=======
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    public Sprite skillSprite;
    public UnityEvent Casted;

>>>>>>> КАК-ДВИГАТЬСЯ-БЛЯТЬ
    private const float timeEpsilon = 0.0001f;
    public void Cast()
    {
        if (Time.time - nextUseTime > timeEpsilon)//(isAvaible)
        {
            DoCast();
            //isAvaible = false;
            nextUseTime = Time.time + coolDown;
<<<<<<< HEAD
        }
    }

    protected abstract void DoCast();
    //public bool isAvaible { get; set; } = true;
    public abstract float coolDown { get; }

=======
            Casted.Invoke();
            
        }
    }
    
    
    protected abstract void DoCast();
    //public bool isAvaible { get; set; } = true;
    public abstract float coolDown { get; }
    public abstract string[] axises { get; }
    
>>>>>>> КАК-ДВИГАТЬСЯ-БЛЯТЬ
    private void Awake()
    {
        nextUseTime = Time.time;
    }

    public float nextUseTime { get; set; }
<<<<<<< HEAD

=======
    
    //protected abstract Sprite skillImage { get; }
>>>>>>> КАК-ДВИГАТЬСЯ-БЛЯТЬ
}