﻿using System.Collections;
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

    private void Awake()
    {
        player.weaponHolder.OnWeaponChanged.AddListener(WeaponChanged);
        player.weaponHolder.ammoContainer.OnAmmoChangedEvent.AddListener(RefreshUI);
    }

    private void Start()
    {
        foreach (var ability in player.abilities)
        {
            Instantiate(abilityUi,abilityPanel).Setup(ability.Value);
        }
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
        string weaponInfo = default;
        weaponInfo = currentWeapon.DisplayName;
        if (currentWeapon is FirearmWeapon)
        {
            weaponInfo = weaponInfo + " | " + player.weaponHolder.ammoContainer.GetAmountOfAmmo((currentWeapon as FirearmWeapon)?.AmmoType).ToString();
        }
        weaponInfoText.text = weaponInfo;
        //weaponInfoText.text = $"{currentWeapon.DisplayName} | {ammoCount}";
    }

}