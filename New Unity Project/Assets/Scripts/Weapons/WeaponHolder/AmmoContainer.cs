using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmmoContainer : MonoBehaviour
{

    public UnityEvent OnAmmoChangedEvent;

    [SerializeField] private bool infiniteAmmo;

    private Dictionary<AmmoType, int> ammoInventory = new Dictionary<AmmoType, int>();

    // This returns true if ammo was added and false if it did not
    public bool AddAmmo(AmmoType type, int amount)
    {

        if(ammoInventory.TryGetValue(type, out var _slot) == false)
        {
            ammoInventory.Add(type, amount);
            OnAmmoChangedEvent.Invoke();
            return true;
        }
        else
        {
            if (_slot + amount <= type.maxAmount)
            {
                ammoInventory[type] += amount;
                OnAmmoChangedEvent.Invoke();
                return true;
            }
        }

        return false;

    }

    // This returns true if it is possible to spend this type of ammo
    public bool SpendAmmo(AmmoType type, int amount)
    {
        if (infiniteAmmo == true) return true;

        if (ammoInventory.TryGetValue(type, out var _slot))
        {
            if (_slot - amount >= 0)
            {
                ammoInventory[type] -= amount;
                OnAmmoChangedEvent.Invoke();
                return true;
            }
        }

        return false;

    }

    public int GetAmountOfAmmo(AmmoType type)
    {

        if (infiniteAmmo) return 9999;

        if (ammoInventory.TryGetValue(type, out var _slot))
        {
            return _slot;
        }

        return 0;

    }

}