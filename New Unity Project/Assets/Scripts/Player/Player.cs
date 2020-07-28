using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AmmoContainer))]
public class WeaponHolder : MonoBehaviour 
{

    // Название скрипта не окончательное. Я не могу придумать ничего лучше. 
    // Этот скрипт отвечает за управление оружием, какие-нибудь условные турели могут его использовать,
    // игрок может, нпс могут.

    [SerializeField] private WeaponBase weaponPrefab;

    public AmmoContainer ammoContainer { get; private set; }
    private WeaponBase currentWeapon;

    private void Awake()
    {
        ammoContainer = GetComponent<AmmoContainer>();
        currentWeapon = Instantiate(weaponPrefab, this.transform);
        currentWeapon.SetOwner(this);
    }

    private void Update()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Use(Input.GetMouseButton(0));
        }
    }

}