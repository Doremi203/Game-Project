using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RagdollController))]
public class ActorDeathAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Actor actor;
    [SerializeField] private float ragdollDelay = 0.2f;

    private RagdollController ragdollController;

    private void Awake()
    {
        ragdollController = GetComponent<RagdollController>();
        actor.HealthComponent.Died += Death;
    }

    private void Death(DamageInfo info)
    {
        animator.SetTrigger("death");
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(ragdollDelay);
        animator.enabled = false;
        ragdollController.StartRagdolling();
    }

}