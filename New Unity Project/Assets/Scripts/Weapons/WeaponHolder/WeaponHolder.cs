using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AmmoContainer))]
public class WeaponHolder : MonoBehaviour
{

    // Название скрипта не окончательное. Я не могу придумать ничего лучше. 
    // Этот скрипт отвечает за управление оружием, какие-нибудь условные турели могут его использовать,
    // игрок может, нпс могут.

    public UnityEvent OnWeaponChanged;
    public AmmoContainer ammoContainer { get; private set; }
    public WeaponBase currentWeapon { get; private set; }

    // Взять оружие в руки.
    public void EquipWeapon(WeaponBase weapon)
    {
        currentWeapon = weapon;
        currentWeapon.SetOwner(this);
        OnWeaponChanged.Invoke();
    }

    private void Awake()
    {
        ammoContainer = GetComponent<AmmoContainer>();
    }

}