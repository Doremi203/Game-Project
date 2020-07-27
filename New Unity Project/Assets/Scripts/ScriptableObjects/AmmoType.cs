using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AmmoType", menuName = "Weapons/ Ammo/ NewAmmoType")]
public class AmmoType : ScriptableObject
{

    // Path to all the AmmoTypes in the Resources folder
    private const string AmmoTypesPath = "AmmoTypes";

    // Get all the AmmoTypes from the Resources folder
    public static List<AmmoType> GetAmmoTypes()
    {
        List<AmmoType> ammoTypes = new List<AmmoType>();
        foreach (var item in Resources.LoadAll<AmmoType>(AmmoTypesPath))
        {
            ammoTypes.Add(item);
        }
        return ammoTypes;
    }

    // Max amount of ammo that player can pick up
    public int maxAmount;

}