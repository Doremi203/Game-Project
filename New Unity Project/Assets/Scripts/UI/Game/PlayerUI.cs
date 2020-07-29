using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI weaponInfoText;

    private WeaponBase currentWeapon;

    private void Start()
    {
        player.weaponHolder.OnWeaponChanged.AddListener(WeaponChanged);
        player.weaponHolder.ammoContainer.OnAmmoChangedEvent.AddListener(RefreshUI);
    }

    private void WeaponChanged()
    {
        // Отписываемся от событий предыдущего оружия.
        currentWeapon?.OnUsingStartEvent.RemoveListener(RefreshUI);
        currentWeapon = player.weaponHolder.currentWeapon;
        weaponInfoText.text = currentWeapon.DisplayName;
        // Подписываемся на новое.
        currentWeapon?.OnUsingStartEvent.AddListener(RefreshUI);
        RefreshUI();
    }

    private void RefreshUI()
    {
        string ammoCount = player.weaponHolder.ammoContainer.GetAmountOfAmmo((currentWeapon as FirearmWeapon)?.AmmoType).ToString();
        weaponInfoText.text = currentWeapon.DisplayName + " | " + ammoCount;
    }

}