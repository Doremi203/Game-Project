using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoContainer : MonoBehaviour
{

    [SerializeField] private bool infiniteAmmo;

    // Ammo inventory
    private List<AmmoSlot> inventory = new List<AmmoSlot>();

    // This returns true if ammo was added and false if it did not
    public bool AddAmmo(AmmoType type, int amount)
    {
        // Maybe we already have this type of ammo in the inventory
        foreach (var item in inventory)
        {
            if(item.type == type)
            {
                if(item.amount + amount <= item.type.maxAmount)
                {
                    item.amount += amount;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        // If we do not, we will create it
        inventory.Add(new AmmoSlot(type, amount));
        return true;
    }

    // This returns true if it is possible to spend this type of ammo
    public bool SpendAmmo(AmmoType type, int amount)
    {

        // Если включены бесконечные патроны
        if (infiniteAmmo == true) return true;

        // Иначе ищем нужные патроны в инвентаре
        foreach (var item in inventory)
        {
            if (item.type == type)
            {
                if (item.amount - amount >= 0)
                {
                    item.amount -= amount;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        // If we do not have this type of ammo, we cannot spend it
        return false;
    }

    public int GetAmountOfAmmo(AmmoType type)
    {
        // Если включены бесконечные патроны (Хуйня какая-то, я перепишу потом)
        if (infiniteAmmo) return 9999;

        foreach (var item in inventory)
        {
            if (item.type == type)
            {
                return item.amount;
            }
        }
        return 0;
    }

}

[System.Serializable]
public class AmmoSlot
{
    public AmmoSlot(AmmoType type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }

    public AmmoType type;
    public int amount;

}