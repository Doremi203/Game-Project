using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCameraShake : WeaponComponent
{

    [SerializeField] private float intensity;
    [SerializeField] private float time;

    public override void OnShoot()
    {
        if (weapon.Owner != Player.Instance.Actor) return;
        CinemachineShake.instance.ShakeCamera(intensity, time);
    }

}