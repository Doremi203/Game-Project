using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BloodPuddleSpawner : MonoBehaviour
{

    [SerializeField] private Actor actor;
    [SerializeField] private Rigidbody targetBone;
    [SerializeField] private DecalProjector[] bloodPuddlePrefabs;
    [SerializeField] private float timeWithoutMovingToSpawnPuddle;
    [SerializeField] private LayerMask layerMask;

    private bool isDead;
    private float timeWithoutMoving;

    private void Awake() => actor.DeathEvent.AddListener(OnDeath);

    private void OnDeath() => isDead = true;

    private void Update()
    {
        if (!isDead) return;
        if (targetBone.IsSleeping()) timeWithoutMoving += Time.deltaTime;
        if (timeWithoutMoving >= timeWithoutMovingToSpawnPuddle)
        {
            SpawnBloodPuddle();
            this.enabled = false;
        }
    }
    private void SpawnBloodPuddle()
    {
        int i = UnityEngine.Random.Range(0, bloodPuddlePrefabs.Length);

        Ray _ray = new Ray(targetBone.transform.position, Vector3.up * -1f);
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, 2f, layerMask))
        {
            Quaternion _rotation = Quaternion.LookRotation(-_hit.normal);
            DecalProjector _decalProjector = Instantiate(bloodPuddlePrefabs[i], _hit.point, _rotation);
            _decalProjector.transform.eulerAngles = new Vector3(_decalProjector.transform.eulerAngles.x, _decalProjector.transform.eulerAngles.y, UnityEngine.Random.Range(0, 360));
        }
    }

}