using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesSpawner : MonoBehaviour
{

    [SerializeField] private GameObject prefab;
    [SerializeField] private float destroyDelay = 1f;

    public void Spawn()
    {
        Destroy(Instantiate(prefab, this.transform.position, Quaternion.identity), destroyDelay);
    }

}