﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (OnPickedUp(other.GetComponent<Player>())) Destroy(this.gameObject);
        }
    }

    protected abstract bool OnPickedUp(Player player);

}