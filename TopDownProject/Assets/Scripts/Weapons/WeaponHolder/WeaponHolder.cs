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
    public WeaponBase currentWeapon { get; private set; }

    [SerializeField] private bool infinityAmmo;
    [SerializeField] private Transform armBone;
    [SerializeField] private WeaponBase weaponPrefab;

    private Actor owner;

    // Взять оружие в руки.
    public void EquipWeapon(WeaponBase weapon)
    {
        if (currentWeapon) currentWeapon.Drop();
        currentWeapon = weapon;
        currentWeapon.Pickup(GetComponent<Actor>(), infinityAmmo);
        weapon.transform.SetParent(armBone, true);
        weapon.transform.position = armBone.position;
        weapon.transform.localRotation = Quaternion.identity;
        OnWeaponChanged.Invoke();
    }

    public void Drop()
    {
        if (currentWeapon == null) return;
        currentWeapon.Drop();
        currentWeapon = null;
        OnWeaponChanged.Invoke();
    }

    private void Awake() => owner = this.GetComponent<Actor>();

    private void Start()
    {
        if (weaponPrefab) EquipWeapon(Instantiate(weaponPrefab));
    }

}