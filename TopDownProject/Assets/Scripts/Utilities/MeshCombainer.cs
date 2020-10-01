using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombainer : MonoBehaviour
{

    public void CombineMeshesAdvanced()
    {
        MeshFilter[] _meshFilters = GetComponentsInChildren<MeshFilter>();

        List<Material> _materials = new List<Material>();
        MeshRenderer[] _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer _renderer in _meshRenderers)
        {
            if (_renderer.transform == this.transform) continue;
            Material[] _localMaterials = _renderer.sharedMaterials;
            foreach (Material _localMaterial in _localMaterials)
            {
                if (!_materials.Contains(_localMaterial)) _materials.Add(_localMaterial);
            }
        }

        List<Mesh> _subMeshes = new List<Mesh>();
        foreach (Material _material in _materials)
        {
            List<CombineInstance> _combiners = new List<CombineInstance>();
            foreach (MeshFilter _meshFilter in _meshFilters)
            {
                MeshRenderer _renderer = _meshFilter.GetComponent<MeshRenderer>();
                if(_renderer == null)
                {
                    Debug.LogError(_meshFilter.name + " has no MeshRenderer");
                    continue;
                }

                Material[] _localMaterials = _renderer.sharedMaterials;
                for (int i = 0; i < _localMaterials.Length; i++)
                {
                    if (_localMaterials[i] != _material) continue;

                    CombineInstance _combineInstance = new CombineInstance();
                    _combineInstance.mesh = _meshFilter.sharedMesh;
                    _combineInstance.subMeshIndex = i;
                    _combineInstance.transform = _meshFilters[i].transform.localToWorldMatrix;
                    _combiners.Add(_combineInstance);
                }
            }

            Mesh _mesh = new Mesh();
            _mesh.CombineMeshes(_combiners.ToArray(), true);
            _subMeshes.Add(_mesh);
        }

        List<CombineInstance> _finalCombiners = new List<CombineInstance>();
        foreach (Mesh _mesh in _subMeshes)
        {
            CombineInstance _combineInstance = new CombineInstance();
            _combineInstance.mesh = _mesh;
            _combineInstance.subMeshIndex = 0;
            _combineInstance.transform = Matrix4x4.identity;
            _finalCombiners.Add(_combineInstance);
        }
        Mesh _finalMesh = new Mesh();
        _finalMesh.CombineMeshes(_finalCombiners.ToArray(), false);
        this.GetComponent<MeshFilter>().sharedMesh = _finalMesh;

        foreach (Transform item in transform)
            item.gameObject.SetActive(false);
    }

    public void CombineMeshes()
    {
        MeshFilter[] _meshFilters = GetComponentsInChildren<MeshFilter>();

        CombineInstance[] _combineInstances = new CombineInstance[_meshFilters.Length];

        for (int i = 0; i < _meshFilters.Length; i++)
        {
            if (_meshFilters[i].transform == transform) continue;
            _combineInstances[i].subMeshIndex = 0;
            _combineInstances[i].mesh = _meshFilters[i].sharedMesh;
            _combineInstances[i].transform = _meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh _finalMesh = new Mesh();
        _finalMesh.CombineMeshes(_combineInstances, false);

        this.GetComponent<MeshFilter>().sharedMesh = _finalMesh;

        foreach (Transform item in transform)
            item.gameObject.SetActive(false);
    }

}