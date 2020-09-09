using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private WeaponHolder weaponHolder;

    private Weapon currentWeapon;

    private void Awake()
    {
        weaponHolder.OnWeaponChanged.AddListener(OnWeaponUpdated);
    }

    private void OnWeaponUpdated()
    {
        animator.ResetTrigger("attack");
        if (currentWeapon) currentWeapon.OnShootEvent.RemoveListener(OnWeaponShoot);
        animator.SetBool("armed", weaponHolder.CurrentWeapon);
        if (weaponHolder.CurrentWeapon)
        {
            currentWeapon = weaponHolder.CurrentWeapon;
            animator.SetInteger("weaponType", (int)weaponHolder.CurrentWeapon.AnimationType);
            weaponHolder.CurrentWeapon.OnShootEvent.AddListener(OnWeaponShoot);
        }
    }

    private void OnWeaponShoot()
    {
        animator.SetTrigger("attack");
    }

}