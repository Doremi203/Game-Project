using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Actor))]
public class BloodSpawner : MonoBehaviour
{

    [SerializeField] private DecalProjector[] bloodDecalPrefabs;
    [SerializeField] private ParticleSystem bloodEffectPrefab;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private float minSplashDelay;
    [SerializeField] private float maxSplashDelay;
    //[SerializeField] private int bloodAmount = 10;

    private Actor owner;

    private void Awake()
    {
        owner = this.GetComponent<Actor>();
        owner.OnDamageTaken += OwnerDamageTaken;
    }

    private void OnDestroy() => owner.OnDamageTaken -= OwnerDamageTaken;

    private void OwnerDamageTaken(DamageInfo info)
    {
        if (!info.DamageType.SpawnBlood) return;

        // Particles
        Vector3 _relativePos = this.transform.position + info.Direction - transform.position;
        _relativePos.y = 0;
        Quaternion _rotation = Quaternion.LookRotation(_relativePos, Vector3.up);

        Destroy(Instantiate(bloodEffectPrefab, this.transform.position, _rotation), 2f);

        // Decals 
        StartCoroutine(SpawnBloodWithDelay(info.Direction, info.DamageType.BloodAmount));
    }

    private IEnumerator SpawnBloodWithDelay(Vector3 damageDirection, int bloodAmount)
    {
        yield return new WaitForSeconds(Random.Range(minSplashDelay, maxSplashDelay));

        Vector3 _origin = owner.transform.position + Vector3.up;
        int _raysCount = bloodAmount;
        int _successfulRays = _raysCount;

        for (int a = 0; a < 3; a++)
        {
            Vector3 _offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, -1f), Random.Range(-0.5f, 0.5f));
            Vector3 _direction = damageDirection / 2 + _offset;

            int i = Random.Range(0, bloodDecalPrefabs.Length);
            if (SpawnDecal(bloodDecalPrefabs[i], _origin, _direction, 4f, true))
            {
                _successfulRays--;
                yield return new WaitForSeconds(0.005f);
            }
        }

        for (int a = 0; a < _raysCount; a++)
        {
            Vector3 _offset = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-1f, -0.25f), Random.Range(-0.25f, 0.25f));
            Vector3 _direction = damageDirection + _offset;

            int i = Random.Range(0, bloodDecalPrefabs.Length);
            if (SpawnDecal(bloodDecalPrefabs[i], _origin, _direction, 4f, true))
            {
                _successfulRays--;
                yield return new WaitForSeconds(0.005f);
            }
        }

        for (int a = 0; a < _successfulRays; a++)
        {
            Vector3 _offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, -1f), Random.Range(-0.5f, 0.5f));
            Vector3 _direction = damageDirection + _offset;

            int i = Random.Range(0, bloodDecalPrefabs.Length);
            if (SpawnDecal(bloodDecalPrefabs[i], _origin, _direction, 4f, true))
            {
                yield return new WaitForSeconds(0.005f);
            }
        }

    }

    private bool SpawnDecal(DecalProjector decal, Vector3 origin, Vector3 direction, float distance, bool setSize)
    {
        Ray _ray = new Ray(origin, direction);
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, distance, layerMask))
        {
            Debug.DrawLine(origin, _hit.point, Color.green, 4f);

            Quaternion _rotation = Quaternion.LookRotation(-_hit.normal);
            Vector3 _scale = new Vector3(Random.Range(minScale.x, maxScale.x), Random.Range(minScale.y, maxScale.y), 0.2f);
            DecalProjector _decalProjector =  Instantiate(decal, _hit.point + _hit.normal * 0.1f, _rotation);
            if(setSize) _decalProjector.size = _scale;
            _decalProjector.transform.eulerAngles = new Vector3(_decalProjector.transform.eulerAngles.x, _decalProjector.transform.eulerAngles.y, Random.Range(0, 360));
            return true;
        }
        else
        {
            Debug.DrawRay(origin, direction * distance, Color.yellow, 4f);
        }
        return false;
    }

}