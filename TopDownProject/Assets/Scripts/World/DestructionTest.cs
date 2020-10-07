using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestructionTest : MonoBehaviour, IDamageable
{

    [SerializeField] private GameObject goodModel;
    [SerializeField] private GameObject badModel;

    public void ApplyDamage(Actor damageCauser, float damage, DamageType damageType, Vector3 damageDirection)
    {
        goodModel.SetActive(false);
        badModel.SetActive(true);
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<NavMeshObstacle>().enabled = false;
    }

    private void Awake()
    {
        goodModel.SetActive(true);
        badModel.SetActive(false);
    }

}