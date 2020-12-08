using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(BloodSpawner))]
public class CharacterBloodSpawner : MonoBehaviour
{

    [SerializeField] private ParticleSystem meleeBloodEffectPrefab;
    [SerializeField] private ParticleSystem bulletBloodEffectPrefab;
    [SerializeField] private float minSplashDelay;
    [SerializeField] private float maxSplashDelay;

    private Actor owner;
    private BloodSpawner bloodSpawner;

    private void Awake()
    {
        owner = GetComponent<Actor>();
        bloodSpawner = GetComponent<BloodSpawner>();
    }

    private void Start()
    {
        owner.HealthComponent.Damaged += OwnerDamageTaken;
    }

    private void OnDestroy()
    {
        owner.HealthComponent.Damaged -= OwnerDamageTaken;
    }

    private void OwnerDamageTaken(DamageInfo info, float beforeHealth, float currentHealth)
    {
        // Particles
        Vector3 direction = transform.position + info.Direction - transform.position;
        direction.y = 0;
        Quaternion _rotation = Quaternion.LookRotation(direction, Vector3.up);
        ParticleSystem effectToSpawn = info.DamageType == DamageType.Melee ? meleeBloodEffectPrefab : bulletBloodEffectPrefab;
        Destroy(Instantiate(effectToSpawn, transform.position, _rotation), 2f);

        // Decals 
        int bloodAmount = info.DamageType == DamageType.Melee ? 10 : 3;
        StartCoroutine(SpawnBloodWithDelay(info.Direction, bloodAmount));
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