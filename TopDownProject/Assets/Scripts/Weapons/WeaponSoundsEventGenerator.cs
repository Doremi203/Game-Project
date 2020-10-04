using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponSoundsEventGenerator : WeaponComponent
{

    [SerializeField] private float soundEventDistance;

    public override void OnShoot()
    {
        Actor _owner = weapon.Owner;
        SoundEventGenerator.GenerateSoundEvent(_owner, _owner.transform.position, soundEventDistance);
    }

    private void OnDrawGizmos() 
    {
        if (weapon == null) return;
        Actor _owner = weapon.Owner;
        if (_owner == null) return;
        Gizmos.DrawSphere(_owner.transform.position, soundEventDistance / 2);
    }

}