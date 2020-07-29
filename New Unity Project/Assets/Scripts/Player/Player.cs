using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponHolder))]
[RequireComponent(typeof(PlayerController))]
public class Player : Actor
{

    public WeaponHolder weaponHolder { get; private set; }
    public PlayerController controller { get; private set; }

    [Header("FOR TESTING")]
    [SerializeField] private WeaponBase prefabWeaponA;
    [SerializeField] private WeaponBase prefabWeaponB;

    private WeaponBase weaponA;
    private WeaponBase weaponB;

    public void SetWeapons(WeaponBase weaponA, WeaponBase weaponB)
    {
        this.weaponA = Instantiate(weaponA, this.transform);
        this.weaponB = Instantiate(weaponB, this.transform);
        weaponHolder.EquipWeapon(this.weaponA);
    }

    protected override void Awake()
    {
        weaponHolder = this.GetComponent<WeaponHolder>();
        controller = this.GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Это для теста. В реальной игре оружия будут появлятся из сохранений.
        SetWeapons(prefabWeaponA, prefabWeaponB);
    }

    private void Update()
    {
        PlayerInput();
    }

    // Я не уверен, что это должно быть тут, но пусть пока будет.
    private void PlayerInput()
    {
        if (weaponHolder.currentWeapon != null) weaponHolder.currentWeapon.Use(Input.GetMouseButton(0));
        if (Input.GetKeyDown(KeyCode.Alpha1)) weaponHolder.EquipWeapon(weaponA);
        if (Input.GetKeyDown(KeyCode.Alpha2)) weaponHolder.EquipWeapon(weaponB);
    }

}