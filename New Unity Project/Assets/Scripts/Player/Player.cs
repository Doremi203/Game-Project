using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AmmoContainer))]
public class Player : MonoBehaviour
{

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