using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorDeathAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Actor actor;

    private void Awake()
    {
        actor.OnDeath.AddListener(OnDeath);
    }

    private void OnDeath()
    {
        animator.SetTrigger("death");
    }

}