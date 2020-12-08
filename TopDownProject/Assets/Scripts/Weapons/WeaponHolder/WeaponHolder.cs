using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Actor))]
public class WeaponHolder : MonoBehaviour
{

    public UnityEvent OnWeaponChanged;
    public Weapon CurrentWeapon { get; private set; }
    public Actor Owner { get; private set; }
    public bool InfinityAmmo => infinityAmmo;

    [SerializeField] private bool infinityAmmo;
    [SerializeField] private bool dropOnDeath = true;
    [SerializeField] private Transform armBone;
    [SerializeField] private Weapon weaponPrefab;
    [SerializeField] private Weapon defaultWeaponPrefab;

    private WeaponHolderState state;    

    public void EquipWeapon(Weapon weapon)
    {
        if (!weapon) return;
        switch (state)
        {
            case WeaponHolderState.Empty:
                break;
            case WeaponHolderState.Default:
                Destroy(CurrentWeapon.gameObject);
                break;
            case WeaponHolderState.Weapon:
                DropCurrentWeapon();
                break;
            default:
                break;
        }
        test_EquipWeapon(weapon);
        state = WeaponHolderState.Weapon;
        OnWeaponChanged.Invoke();
    }

    public void Drop()
    {
        if (state != WeaponHolderState.Weapon) return;
        DropCurrentWeapon();
        if (defaultWeaponPrefab)
        {
            test_EquipWeapon(Instantiate(defaultWeaponPrefab));
            state = WeaponHolderState.Default;
        }
        else
            state = WeaponHolderState.Empty;
        OnWeaponChanged.Invoke();
    }

    private void Awake()
    {
        Owner = GetComponent<Actor>();
    }

    private void Start()
    {
        Owner.HealthComponent.Died += OwnerDied;
        if (weaponPrefab)
            EquipWeapon(Instantiate(weaponPrefab));
        else if (defaultWeaponPrefab)
        {
            test_EquipWeapon(Instantiate(defaultWeaponPrefab));
            state = WeaponHolderState.Default;
            OnWeaponChanged.Invoke();
        }
    }

    private void OwnerDied(DamageInfo info)
    {
        switch (state)
        {
            case WeaponHolderState.Empty:
                break;
            case WeaponHolderState.Default:
                Destroy(CurrentWeapon.gameObject);
                break;
            case WeaponHolderState.Weapon:
                if (dropOnDeath) Drop();
                break;
            default:
                break;
        }
    }

    private void DropCurrentWeapon()
    {
        CurrentWeapon.transform.SetParent(null);
        CurrentWeapon.Drop();
        CurrentWeapon = null;
    }

    private void test_EquipWeapon(Weapon weapon)
    {
        CurrentWeapon = weapon;
        CurrentWeapon.Pickup(this);
        weapon.transform.SetParent(armBone, true);
        weapon.transform.position = armBone.position;
        weapon.transform.localRotation = Quaternion.identity;
    }

    private enum WeaponHolderState
    {
        Empty,
        Default,
        Weapon
    }

}