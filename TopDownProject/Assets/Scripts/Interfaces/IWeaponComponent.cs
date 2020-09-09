using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponComponent 
{

    void OnShoot(Weapon weapon);
    void OnDroped(Weapon weapon);
    bool IsReadyToShoot(Weapon weapon);
    void DrawDebug(Weapon weapon);
  
}