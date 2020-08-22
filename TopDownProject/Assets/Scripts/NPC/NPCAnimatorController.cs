using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAnimatorController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private WeaponHolder weaponHolder;

    private WeaponBase currentWeapon;

    public void Death()
    {
        animator.SetTrigger("Death");
    }

    private void Awake()
    {
        //agent = GetComponent<NavMeshAgent>();
        weaponHolder.OnWeaponChanged.AddListener(OnWeaponUpdated);
    }

    private void Start()
    {
        //weaponHolder.OnWeaponChanged.AddListener(OnWeaponUpdated);
    }

    private void Update()
    {
        Vector3 moveDirection = agent.velocity;
        moveDirection = transform.InverseTransformDirection(moveDirection);
        animator.SetFloat("Horizontal", moveDirection.x);
        animator.SetFloat("Vertical", moveDirection.z);
        animator.SetFloat("Speed", agent.velocity.magnitude / 2f);
    }

    private void OnWeaponUpdated()
    {
        animator.SetBool("armed", weaponHolder.currentWeapon);
        if (weaponHolder.currentWeapon)
        {
            if (currentWeapon) currentWeapon.OnShootEvent.RemoveListener(OnWeaponShoot);
            currentWeapon = weaponHolder.currentWeapon;
            animator.SetInteger("weaponType", (int)weaponHolder.currentWeapon.AnimationType);
            weaponHolder.currentWeapon.OnShootEvent.AddListener(OnWeaponShoot);
        }
    }

    private void OnWeaponShoot()
    {
        animator.SetTrigger("attack");
    }

}