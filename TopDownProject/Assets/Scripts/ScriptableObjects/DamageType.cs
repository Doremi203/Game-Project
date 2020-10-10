using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DamageType", menuName = "DamageType")]
public class DamageType : ScriptableObject
{

    public bool SpawnBlood => spawnBlood;
    public int BloodAmount => bloodAmount;

    [SerializeField] private bool spawnBlood;
    [SerializeField] private int bloodAmount;

}