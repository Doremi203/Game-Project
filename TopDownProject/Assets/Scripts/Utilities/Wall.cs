using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SurfaceMaterial))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Wall : MonoBehaviour
{

    public WallMaterial CurrentWallMaterial;
    
    public void SetMaterial(WallMaterial material)
    {
        CurrentWallMaterial = material;
        this.GetComponent<MeshRenderer>().sharedMaterial = material.Material;
        this.GetComponent<SurfaceMaterial>().BulletHitEffect = material.HitParticles;
    }

}