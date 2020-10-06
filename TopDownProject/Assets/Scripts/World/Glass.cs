using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NavMeshObstacle))]
public class Glass : MonoBehaviour, IDamageable
{

    [SerializeField] private Material brokenMaterial;

    public void ApplyDamage(Actor damageCauser, float damage, DamageType damageType)
    {
        this.GetComponent<MeshRenderer>().sharedMaterial = brokenMaterial;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<NavMeshObstacle>().enabled = false;
    }

}