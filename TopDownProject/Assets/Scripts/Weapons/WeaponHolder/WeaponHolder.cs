using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{

    // Название скрипта не окончательное. Я не могу придумать ничего лучше. 
    // Этот скрипт отвечает за управление оружием, какие-нибудь условные турели могут его использовать,
    // игрок может, нпс могут.

    public UnityEvent OnWeaponChanged;
    public WeaponBase currentWeapon { get; private set; }
    public WeaponBase currentWeaponAlt { get; private set; }

    [SerializeField] private bool infinityAmmo;
    [SerializeField] private Transform armBone;
    [SerializeField] private Transform armBoneAlt;

    // Взять оружие в руки.
    public void EquipWeapon(WeaponBase weapon)
    {
        if (currentWeapon) currentWeapon.Drop();
        currentWeapon = weapon;
        currentWeapon.Pickup(GetComponent<Actor>(), infinityAmmo);
        weapon.transform.SetParent(armBone, false);
        weapon.transform.position = armBone.position;
        weapon.transform.localRotation = Quaternion.identity;
        OnWeaponChanged.Invoke();
    }

    public void EquipWeaponAlt(WeaponBase weapon)
    {
        if(currentWeaponAlt) currentWeaponAlt.Drop();
        currentWeaponAlt = weapon;
        currentWeaponAlt.Pickup(GetComponent<Actor>(), infinityAmmo);
        weapon.transform.SetParent(armBoneAlt, false);
        weapon.transform.position = armBoneAlt.position;
        weapon.transform.localRotation = Quaternion.identity;
        OnWeaponChanged.Invoke();
    }

    public void Drop()
    {
        if (currentWeapon == null) return;
        if (currentWeaponAlt == null) currentWeapon.Drop();
        currentWeapon = null;
        OnWeaponChanged.Invoke();
    }

    public void DropAlt()
    {
        if (currentWeaponAlt == null) return;
        currentWeaponAlt.Drop();
        currentWeaponAlt = null;
        OnWeaponChanged.Invoke();
    }

    private void Awake()
    {       
        if (GetComponent<Actor>() == null) Debug.LogError("There is no actor component on " + gameObject.name);
    }

}