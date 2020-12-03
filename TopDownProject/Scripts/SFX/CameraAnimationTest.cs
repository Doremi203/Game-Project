using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CameraAnimationTest : MonoBehaviour
{

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Actor.ActorDied += ActorDied;      
    }

    private void OnDestroy()
    {
        Actor.ActorDied -= ActorDied;
    }

    private void ActorDied(Actor obj)
    {
        animator.SetTrigger("play");
    }

}