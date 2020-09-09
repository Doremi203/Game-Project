using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceMaterial : MonoBehaviour
{

    public ParticleSystem BulletHitEffect => bulletHitEffect;

    [SerializeField] private ParticleSystem bulletHitEffect;

}