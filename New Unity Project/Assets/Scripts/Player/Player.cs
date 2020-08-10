using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public Dictionary<Type, Ability> abilities = new Dictionary<Type, Ability>();

    // Test
    [SerializeField] private Transform armBone;
    [SerializeField] private Transform spineBone;

    [SerializeField] private Ability dash;

    [SerializeField] private LayerMask weaponsMask;

    public void TakeWeapon()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, 4f, weaponsMask);
        WeaponBase closestWeapon = null;
        foreach (var item in hits)
        {
            WeaponBase target = item.GetComponent<WeaponBase>();
            if (target == null) continue;
            if (target.IsDrop == false) continue;
            if (closestWeapon == null)
            {
                closestWeapon = target;
            }
            else
            {
                float a = Vector3.Distance(this.transform.position, closestWeapon.transform.position);
                float b = Vector3.Distance(this.transform.position, target.transform.position);
                if (a > b) closestWeapon = target;
            }
        }

        if (closestWeapon == null) return;

        if (weaponB) weaponB.Drop();

        weaponB = closestWeapon;

        EquipWeapon(1);

    }

    public void SetWeapons(WeaponBase weaponA, WeaponBase weaponB)
    {
        this.weaponA = Instantiate(weaponA, this.transform);
        //this.weaponB = Instantiate(weaponB, this.transform);
        EquipWeapon(0);
    }

    // Test
    public void EquipWeapon(int i)
    {
        if(i == 0)
        {
            EquipWeapon(weaponA);
            if (weaponB) HideWeapon(weaponB);
        }
        else
        {
            if (weaponB)
            {
                EquipWeapon(weaponB);
                HideWeapon(weaponA);
            }
        }
    }

    private void EquipWeapon(WeaponBase weapon)
    {
        weaponHolder.EquipWeapon(weapon);
        weapon.transform.SetParent(armBone, false);
        weapon.transform.position = armBone.position;
        weapon.transform.localRotation = Quaternion.identity;
    }

    private void HideWeapon(WeaponBase weapon)
    {
        weapon.transform.SetParent(spineBone, false);
        weapon.transform.position = spineBone.position;
        weapon.transform.localRotation = Quaternion.identity;
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
        if (Input.GetKeyDown(KeyCode.E)) TakeWeapon();
        if (Input.GetKeyDown(KeyCode.Space)) dash.Cast();
    }

    protected override void Death()
    {
        UnityEngine.Application.LoadLevel(0);
        base.Death();
    }

}