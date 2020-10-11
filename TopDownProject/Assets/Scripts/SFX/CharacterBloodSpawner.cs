using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(BloodSpawner))]
public class CharacterBloodSpawner : MonoBehaviour
{

    [SerializeField] private ParticleSystem bloodEffectPrefab;
    [SerializeField] private float minSplashDelay;
    [SerializeField] private float maxSplashDelay;

    private Actor owner;
    private BloodSpawner bloodSpawner;

    private void Awake()
    {
        owner = this.GetComponent<Actor>();
        bloodSpawner = this.GetComponent<BloodSpawner>();
        owner.OnDamageTaken += OwnerDamageTaken;
    }

    private void OnDestroy() => owner.OnDamageTaken -= OwnerDamageTaken;

    private void OwnerDamageTaken(DamageInfo info)
    {
        // Particles
        Vector3 _relativePos = this.transform.position + info.Direction - transform.position;
        _relativePos.y = 0;
        Quaternion _rotation = Quaternion.LookRotation(_relativePos, Vector3.up);

        Destroy(Instantiate(bloodEffectPrefab, this.transform.position, _rotation), 2f);

        // Decals 
        StartCoroutine(SpawnBloodWithDelay(info.Direction, 10));
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

            if (bloodSpawner.TrySpawnBlood(new Ray(_origin, _direction)))
            {
                _successfulRays--;
                yield return new WaitForSeconds(0.005f);
            }
        }

        for (int a = 0; a < _raysCount; a++)
        {
            Vector3 _offset = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-1f, -0.25f), Random.Range(-0.25f, 0.25f));
            Vector3 _direction = damageDirection + _offset;

            if (bloodSpawner.TrySpawnBlood(new Ray(_origin, _direction)))
            {
                _successfulRays--;
                yield return new WaitForSeconds(0.005f);
            }
        }

        for (int a = 0; a < _successfulRays; a++)
        {
            Vector3 _offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, -1f), Random.Range(-0.5f, 0.5f));
            Vector3 _direction = damageDirection + _offset;

            if (bloodSpawner.TrySpawnBlood(new Ray(_origin, _direction)))
            {
                yield return new WaitForSeconds(0.005f);
            }
        }

    }

}