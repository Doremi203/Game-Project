using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemFadeOut : MonoBehaviour
{

    [SerializeField] private float fadeOutSpeed = 10f;

    private ParticleSystem target;
    private float startEmissionRate;

    private void Awake() => target = this.GetComponent<ParticleSystem>();

    private void Start() => startEmissionRate = target.emission.rateOverTime.constant;

    private void Update()
    {
        var emissionModule = target.emission;
        emissionModule.rateOverTime = emissionModule.rateOverTime.constant - Time.deltaTime * (startEmissionRate / fadeOutSpeed);
    }

}