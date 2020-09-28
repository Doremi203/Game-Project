using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Actor))]
public class WeaponHolder : MonoBehaviour
{

    // Название скрипта не окончательное. Я не могу придумать ничего лучше. 
    // Этот скрипт отвечает за управление оружием, какие-нибудь условные турели могут его использовать,
    // игрок может, нпс могут.

    public UnityEvent OnWeaponChanged;
    public Weapon CurrentWeapon { get; private set; }
    public Actor Owner { get; private set; }
    public bool InfinityAmmo => infinityAmmo;

    [SerializeField] private bool infinityAmmo;
    [SerializeField] private Transform armBone;
    [SerializeField] private Weapon weaponPrefab;

    // Взять оружие в руки.
    public void EquipWeapon(Weapon weapon)
    {
        Drop();
        CurrentWeapon = weapon;
        CurrentWeapon.Pickup(this);
        weapon.transform.SetParent(armBone, true);
        weapon.transform.position = armBone.position;
        weapon.transform.localRotation = Quaternion.identity;
        OnWeaponChanged.Invoke();
    }

    public void Drop()
    {
        if (CurrentWeapon == null) return;
        CurrentWeapon.transform.SetParent(null);
        CurrentWeapon.Drop();
        CurrentWeapon = null;
        OnWeaponChanged.Invoke();
    }

    private void Awake()
    {
        Owner = this.GetComponent<Actor>();
    }

    private void Start()
    {
        if (weaponPrefab) EquipWeapon(Instantiate(weaponPrefab));
    }

}