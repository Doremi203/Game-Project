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
    
    private Weapon currentWeapon;

    private void Start()
    {
        player.weaponHolder.OnWeaponChanged.AddListener(WeaponChanged);
        //Instantiate(abilityUi, abilityPanel).Setup(player.dash);
    }

    private void WeaponChanged()
    {
        // Отписываемся от событий предыдущего оружия.
        currentWeapon?.OnShootEvent.RemoveListener(RefreshUI);
        currentWeapon = player.weaponHolder.CurrentWeapon;
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