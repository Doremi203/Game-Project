using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI weaponInfoText;
    
    private Weapon currentWeapon;

    private void Start()
    {
        WeaponChanged();
        Player.Instance.WeaponHolder.OnWeaponChanged.AddListener(WeaponChanged);
    }

    private void OnDestroy() => Player.Instance.WeaponHolder.OnWeaponChanged.RemoveListener(WeaponChanged);

    private void WeaponChanged()
    {
        // Отписываемся от событий предыдущего оружия.
        currentWeapon?.OnShootEvent.RemoveListener(RefreshUI);
        currentWeapon = Player.Instance.WeaponHolder.CurrentWeapon;
        // Подписываемся на новое.
        currentWeapon?.OnShootEvent.AddListener(RefreshUI);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (currentWeapon == null) return;

        string weaponInfo = currentWeapon.DisplayName;

        WeaponAmmoContainer _weaponAmmoContainer = currentWeapon.GetComponent<WeaponAmmoContainer>();

        if (_weaponAmmoContainer)
        {
            weaponInfo = weaponInfo + " | " + _weaponAmmoContainer.CurrentAmmo.ToString();
        }
        weaponInfoText.text = weaponInfo;
        //weaponInfoText.text = $"{currentWeapon.DisplayName} | {ammoCount}";
    }

}