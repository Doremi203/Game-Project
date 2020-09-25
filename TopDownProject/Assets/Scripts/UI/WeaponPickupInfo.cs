using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeaponPickupInfo : MonoBehaviour
{

    [SerializeField] private RectTransform parent;
    [SerializeField] private GameObject weaponPickupPanel;
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private GameObject weaponAmmoPanel;
    [SerializeField] private TextMeshProUGUI weaponAmmoText;
    [SerializeField] private BoolParameter showPickupInfo;

    private Weapon closestWeapon;

    private void Start()
    {
        weaponPickupPanel.SetActive(false);
        weaponAmmoPanel.SetActive(false);
        Player.Instance.OnClosestWeaponUpdated += ClosestWeaponUpdated;
        BaseParameter.OnSettingsChanged += SettingsUpdated;
    }

    private void OnDestroy()
    {
        Player.Instance.OnClosestWeaponUpdated -= ClosestWeaponUpdated;
        BaseParameter.OnSettingsChanged -= SettingsUpdated;
    }

    private void Update()
    {
        if (!showPickupInfo.GetValue()) return;
        if (closestWeapon == null) return;
        parent.position = Camera.main.WorldToScreenPoint(closestWeapon.transform.position);
    }

    private void ClosestWeaponUpdated(Weapon closestWeapon)
    {
        if (!showPickupInfo.GetValue()) return;
        this.closestWeapon = closestWeapon;
        weaponPickupPanel.SetActive(closestWeapon != null);
        weaponAmmoPanel.SetActive(closestWeapon != null);
        if (closestWeapon == null) return;
        weaponNameText.text = closestWeapon.DisplayName;

        parent.position = Camera.main.WorldToScreenPoint(closestWeapon.transform.position);

        WeaponAmmoContainer _ammoContainer = closestWeapon.GetComponent<WeaponAmmoContainer>();
        weaponAmmoPanel.SetActive(_ammoContainer != null);
        if (_ammoContainer == null) return;      
        weaponAmmoText.text = "Ammo: " + _ammoContainer.CurrentAmmo.ToString();
    }

    private void SettingsUpdated()
    {
        parent.gameObject.SetActive(showPickupInfo.GetValue());
    }

}