using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BloodSpawner : MonoBehaviour
{

    [SerializeField] private DecalProjector bloodDecalPrefab;
    [SerializeField] private DecalProjector bloodPuddlePrefab;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private float minSplashDelay;
    [SerializeField] private float maxSplashDelay;
    [Header("Blood Puddle")]
    [SerializeField] private Transform puddleParent;
    [SerializeField] private float minPuddleDelay;
    [SerializeField] private float maxPpuddleDelay;

    public void SpawnBlood() => StartCoroutine(SpawnBloodWithDelay());

    public void SpawnBloodPuddle() => StartCoroutine(SpawnPuddleWithDelay());

    private IEnumerator SpawnBloodWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(minSplashDelay, maxSplashDelay));
        SpawnDecal(bloodDecalPrefab, transform.position, Vector3.up * -1f, true);
        SpawnDecal(bloodDecalPrefab, transform.position, Vector3.right, true);
        SpawnDecal(bloodDecalPrefab, transform.position, Vector3.right * -1f, true);
        SpawnDecal(bloodDecalPrefab, transform.position, Vector3.forward, true);
        SpawnDecal(bloodDecalPrefab, transform.position, Vector3.forward * -1f, true);
    }

    private IEnumerator SpawnPuddleWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(minPuddleDelay, maxPpuddleDelay));
        SpawnDecal(bloodPuddlePrefab, puddleParent.transform.position, Vector3.up * -1f, false);
    }

    private void SpawnDecal(DecalProjector decal, Vector3 origin, Vector3 direction, bool setSize)
    {
        Ray _ray = new Ray(origin, direction);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, range, layerMask))
        {
            Quaternion _rotation = Quaternion.LookRotation(-_hit.normal);
            Vector3 _scale = new Vector3(Random.Range(minScale.x, maxScale.x), Random.Range(minScale.y, maxScale.y), 0.2f);
            DecalProjector decalProjector =  Instantiate(decal, _hit.point, _rotation);
            if(setSize) decalProjector.size = _scale;
        }
    }

}