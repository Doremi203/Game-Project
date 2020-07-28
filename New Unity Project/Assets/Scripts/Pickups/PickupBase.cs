using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WeaponHolder>())
        {
            if (OnPickedUp(other.GetComponent<WeaponHolder>())) Destroy(this.gameObject);
        }
    }

    protected abstract bool OnPickedUp(WeaponHolder player);

}