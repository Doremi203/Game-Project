using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Videocam : MonoBehaviour
{

    [SerializeField] private float lookingAroundCooldown = 5f;
    [SerializeField] private float lookingAroundCooldownMax = 10f;

    private Animator animator;
    private float nextChangeTime;

    private void Awake() => animator = this.GetComponent<Animator>();

    private void Start()
    {
        nextChangeTime = Time.time + Random.Range(lookingAroundCooldown, lookingAroundCooldownMax);
    }

    private void Update()
    {
        if (Time.time >= nextChangeTime) ChangeDirection();
    }

    private void ChangeDirection()
    {
        nextChangeTime = Time.time + Random.Range(lookingAroundCooldown, lookingAroundCooldownMax);
        animator.SetTrigger("Look");
    }

}