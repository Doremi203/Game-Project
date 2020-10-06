using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BloodSpawner : MonoBehaviour
{

    [SerializeField] private DecalProjector[] bloodDecalPrefabs;
    [SerializeField] private DecalProjector[] bloodPuddlePrefabs;
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
        //SpawnDecal(bloodDecalPrefab, transform.position, Vector3.up * -1f, true);
        //SpawnDecal(bloodDecalPrefab, transform.position, Vector3.right, true);
        //SpawnDecal(bloodDecalPrefab, transform.position, Vector3.right * -1f, true);
        //SpawnDecal(bloodDecalPrefab, transform.position, Vector3.forward, true);
        //SpawnDecal(bloodDecalPrefab, transform.position, Vector3.forward * -1f, true);

        for (int a = 0; a < 10; a++)
        {
            Vector3 _direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 0f), Random.Range(-1f, 1f));
            int i = Random.Range(0, bloodDecalPrefabs.Length);
            SpawnDecal(bloodDecalPrefabs[i], transform.position + transform.up, _direction, true);
        }
        int b = Random.Range(0, bloodDecalPrefabs.Length);
        SpawnDecal(bloodDecalPrefabs[b], transform.position, Vector3.up * -1f, true);
    }

    private IEnumerator SpawnPuddleWithDelay()
    {
        yield return new WaitForSeconds(Random.Range(minPuddleDelay, maxPpuddleDelay));
        int i = Random.Range(0, bloodPuddlePrefabs.Length);
        SpawnDecal(bloodPuddlePrefabs[i], puddleParent.transform.position, Vector3.up * -1f, false);
    }

    private void SpawnDecal(DecalProjector decal, Vector3 origin, Vector3 direction, bool setSize)
    {
        Ray _ray = new Ray(origin, direction);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, range, layerMask))
        {
            Debug.DrawLine(transform.position, _hit.point, Color.green, 5f);

            Quaternion _rotation = Quaternion.LookRotation(-_hit.normal);
            Vector3 _scale = new Vector3(Random.Range(minScale.x, maxScale.x), Random.Range(minScale.y, maxScale.y), 0.2f);
            DecalProjector _decalProjector =  Instantiate(decal, _hit.point, _rotation);
            if(setSize) _decalProjector.size = _scale;
            _decalProjector.transform.eulerAngles = new Vector3(_decalProjector.transform.eulerAngles.x, _decalProjector.transform.eulerAngles.y, Random.Range(0, 360));
        }
    }

}