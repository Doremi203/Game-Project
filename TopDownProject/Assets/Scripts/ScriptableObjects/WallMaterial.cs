using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wall Material", menuName = "New Wall Material")]
public class WallMaterial : ScriptableObject
{

    public static WallMaterial[] GetWallMaterials() => Resources.LoadAll<WallMaterial>("WallMaterials");

    public Material Material => material;
    public ParticleSystem HitParticles => hitParticles;

    [SerializeField] private Material material;
    [SerializeField] private ParticleSystem hitParticles;

}