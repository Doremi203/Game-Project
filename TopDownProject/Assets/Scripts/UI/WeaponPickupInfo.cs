using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponPickupInfo : MonoBehaviour
{

    [SerializeField] private RectTransform parent;
    [SerializeField] private GameObject weaponPickupPanel;
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private GameObject weaponAmmoPanel;
    [SerializeField] private TextMeshProUGUI weaponAmmoText;
    [SerializeField] private KeyCodeParameter pickupBinding;

    private Weapon closestWeapon;

    private void Start()
    {
        weaponPickupPanel.SetActive(false);
        weaponAmmoPanel.SetActive(false);
        Player.Instance.OnClosestWeaponUpdated += ClosestWeaponUpdated;
    }

    private void OnDisable() => Player.Instance.OnClosestWeaponUpdated -= ClosestWeaponUpdated;

    private void Update()
    {
        if (closestWeapon == null) return;
        parent.position = Camera.main.WorldToScreenPoint(closestWeapon.transform.position);
    }

    private void ClosestWeaponUpdated(Weapon closestWeapon)
    {
        this.closestWeapon = closestWeapon;
        weaponPickupPanel.SetActive(closestWeapon != null);
        weaponAmmoPanel.SetActive(closestWeapon != null);
        if (closestWeapon == null) return;
        weaponNameText.text = closestWeapon.DisplayName;
        WeaponAmmoContainer _ammoContainer = closestWeapon.GetComponent<WeaponAmmoContainer>();
        weaponAmmoPanel.SetActive(_ammoContainer != null);
        if (_ammoContainer == null) return;      
        weaponAmmoText.text = "Ammo: " + _ammoContainer.CurrentAmmo.ToString();
    }

}