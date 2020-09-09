using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponSoundsEventGenerator : MonoBehaviour, IWeaponComponent
{

    [SerializeField] private float soundEventDistance;

    public void OnShoot(Weapon weapon)
    {
        Actor _owner = weapon.Owner;
        SoundEventGenerator.GenerateSoundEvent(_owner, _owner.transform.position, soundEventDistance);
    }

    public bool IsReadyToShoot(Weapon weapon) => true;

    public void OnDroped(Weapon weapon) { }


    public void DrawDebug(Weapon weapon) 
    {
        #if UNITY_EDITOR
        Actor _owner = weapon.Owner;
        Handles.DrawWireDisc(_owner.transform.position, _owner.transform.up, soundEventDistance);
        #endif
    }

}