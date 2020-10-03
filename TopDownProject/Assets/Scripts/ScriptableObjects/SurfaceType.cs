using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[CreateAssetMenu(fileName = "New SurfaceType", menuName = "Surface Type")]
public class SurfaceType : ScriptableObject
{

    public ParticleSystem HitEffect => hitEffect;
    public DecalProjector HitDecalProjector => hitDecalProjector;

    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private DecalProjector hitDecalProjector;

}