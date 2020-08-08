using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{

    // Название скрипта не окончательное. Я не могу придумать ничего лучше. 
    // Этот скрипт отвечает за управление оружием, какие-нибудь условные турели могут его использовать,
    // игрок может, нпс могут.

    public UnityEvent OnWeaponChanged;
    public WeaponBase currentWeapon { get; private set; }

    [SerializeField] private bool infinityAmmo;

    // Взять оружие в руки.
    public void EquipWeapon(WeaponBase weapon)
    {
        currentWeapon = weapon;
        currentWeapon.Pickup(GetComponent<Actor>(), infinityAmmo);
        OnWeaponChanged.Invoke();
    }

    private void Awake()
    {       
        if (GetComponent<Actor>() == null) Debug.LogError("There is no actor component on " + gameObject.name);
    }

}