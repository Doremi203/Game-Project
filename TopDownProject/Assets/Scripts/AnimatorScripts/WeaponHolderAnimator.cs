﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private WeaponHolder weaponHolder;

    private WeaponBase currentWeapon;

    private void Awake()
    {
        weaponHolder.OnWeaponChanged.AddListener(OnWeaponUpdated);
    }

    private void OnWeaponUpdated()
    {
        animator.ResetTrigger("attack");
        if (currentWeapon) currentWeapon.OnShootEvent.RemoveListener(OnWeaponShoot);
        animator.SetBool("armed", weaponHolder.currentWeapon);
        if (weaponHolder.currentWeapon)
        {
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