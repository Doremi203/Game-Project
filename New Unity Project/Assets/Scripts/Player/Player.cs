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
	
    public Dictionary<Type,Ability> abilities =new Dictionary<Type, Ability>();

    // Test
    [SerializeField] private Transform armBone;
    [SerializeField] private Transform spineBone;

    public void SetWeapons(WeaponBase weaponA, WeaponBase weaponB)
    {
        this.weaponA = Instantiate(weaponA, this.transform);
        this.weaponB = Instantiate(weaponB, this.transform);
        EquipWeapon(0);
    }

    // Test
    public void EquipWeapon(int i)
    {
        if(i == 0)
        {
            weaponHolder.EquipWeapon(this.weaponA);
            weaponA.transform.SetParent(armBone, false);
            weaponA.transform.position = armBone.position;
            weaponB.transform.SetParent(spineBone, false);
            weaponB.transform.position = spineBone.position;
        }
        else
        {
            weaponHolder.EquipWeapon(this.weaponB);
            weaponA.transform.SetParent(spineBone, false);
            weaponA.transform.position = spineBone.position;
            weaponB.transform.SetParent(armBone, false);
            weaponB.transform.position = armBone.position;
        }
    }

    protected override void Awake()
    {
        base.Awake();
		foreach (var ability in GetComponents<Ability>())
        {
            abilities.Add(ability.GetType(),ability);
        }
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
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
    }

    protected override void Death()
    {
        base.Death();
    }

}