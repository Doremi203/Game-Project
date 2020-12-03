using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(BloodSpawner))]
public class HeadlessBloodSpawner : MonoBehaviour
{

    [SerializeField] private DecalProjector[] bloodDecalPrefabs;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private float startDelayTime;
    [SerializeField] private float activeTime;
    [SerializeField] private Transform targetBone;

    private BloodSpawner bloodSpawner;
    private float spawnTime;
    private float nextSpawnTime;

    private void Awake() => bloodSpawner = this.GetComponent<BloodSpawner>();

    private void Start() => spawnTime = Time.time;

    private void Update()
    {
        if (Time.time < spawnTime + startDelayTime) return;
        if (Time.time >= spawnTime + activeTime)
        {
            this.enabled = false;
            return;
        }
        if (Time.time >= nextSpawnTime)
        {
            if (bloodSpawner.TrySpawnBlood(new Ray(targetBone.position, targetBone.up)))
                nextSpawnTime = Time.time + 0.25f;
        }
    }

}