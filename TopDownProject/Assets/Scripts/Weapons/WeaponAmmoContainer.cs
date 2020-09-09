using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoContainer : MonoBehaviour, IWeaponComponent
{

    public int CurrentAmmo { get; private set; }

    [SerializeField] private int startingAmmoMin;
    [SerializeField] private int startingAmmoMax;

    public bool IsReadyToShoot(Weapon weapon) => CurrentAmmo > 0;

    public void OnDroped(Weapon weapon) { }

    public void OnShoot(Weapon weapon)
    {
        if (!weapon.WeaponHolder.InfinityAmmo) CurrentAmmo--;
    }
    public void DrawDebug(Weapon weapon) { }

    private void Awake() => CurrentAmmo = Random.Range(startingAmmoMin, startingAmmoMax + 1);

}