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

    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI weaponInfoText;
    [SerializeField] private AbilityUi abilityUi;
    [SerializeField] private Transform abilityPanel;
    
    private WeaponBase currentWeapon;

    private void Start()
    {
        player.weaponHolder.OnWeaponChanged.AddListener(WeaponChanged);
        //Instantiate(abilityUi, abilityPanel).Setup(player.dash);
    }

    private void WeaponChanged()
    {
        // Отписываемся от событий предыдущего оружия.
        currentWeapon?.OnShootEvent.RemoveListener(RefreshUI);
        currentWeapon = player.weaponHolder.currentWeapon;
        // Подписываемся на новое.
        currentWeapon?.OnShootEvent.AddListener(RefreshUI);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (currentWeapon == null) return;
        string weaponInfo = default;
        weaponInfo = currentWeapon.DisplayName;
        if (currentWeapon is FirearmWeapon)
        {
            weaponInfo = weaponInfo + " | " + (player.weaponHolder.currentWeapon as FirearmWeapon).CurrentAmmo.ToString();
        }
        weaponInfoText.text = weaponInfo;
        //weaponInfoText.text = $"{currentWeapon.DisplayName} | {ammoCount}";
    }

}