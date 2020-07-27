using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Pickup
{

    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int amount;

    public void Setup(AmmoType ammoType, int amount)
    {
        this.ammoType = ammoType;
        this.amount = amount;
    }

    protected override bool OnPickedUp(Player player)
    {
        return player.ammoContainer.AddAmmo(ammoType, amount);
    }

}