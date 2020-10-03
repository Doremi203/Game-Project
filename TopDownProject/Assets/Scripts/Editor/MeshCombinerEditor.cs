using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshCombinerEditor : Editor
{

    [MenuItem("Tools/Combine Selected Meshes")]
    public static void CombineMeshes()
    {

        Dictionary<Material, List<MeshRenderer>> _targets = new Dictionary<Material, List<MeshRenderer>>();

        foreach (var item in Selection.transforms)
        {

            foreach (var meshRenderer in item.GetComponentsInChildren<MeshRenderer>())
            {

                if (_targets.ContainsKey(meshRenderer.sharedMaterial))
                {
                    _targets[meshRenderer.sharedMaterial].Add(meshRenderer);
                }
                else
                {
                    List<MeshRenderer> _newList = new List<MeshRenderer>();
                    _newList.Add(meshRenderer);
                    _targets.Add(meshRenderer.sharedMaterial, _newList);
                }

            }

            item.gameObject.SetActive(false);

        }

        foreach (var key in _targets.Keys)
        {
            CombineInstance[] _combineInstances = new CombineInstance[_targets[key].Count];

            for (int i = 0; i < _targets[key].Count; i++)
            {
                _combineInstances[i].subMeshIndex = 0;
                _combineInstances[i].mesh = _targets[key][i].GetComponent<MeshFilter>().sharedMesh;
                _combineInstances[i].transform = _targets[key][i].transform.localToWorldMatrix;
            }

            Mesh _finalMesh = new Mesh();
            _finalMesh.CombineMeshes(_combineInstances, true);

            Unwrapping.GenerateSecondaryUVSet(_finalMesh);

            GameObject _newGameObject = new GameObject();
            _newGameObject.name = key.name + " (Generated Mesh)";

            _newGameObject.isStatic = true;

            _newGameObject.AddComponent<MeshFilter>().sharedMesh = _finalMesh;
            _newGameObject.AddComponent<MeshRenderer>().sharedMaterial = key;
            _newGameObject.AddComponent<MeshCollider>();
        }

    }

    public static void CombineMeshesLegacy()
    {

        Dictionary<WallMaterial, List<Wall>> _walls = new Dictionary<WallMaterial, List<Wall>>();

        foreach (var item in GameObject.FindObjectsOfType<Wall>())
        {
            if (item.CurrentWallMaterial == null)
            {
                Selection.activeObject = item.gameObject;
                return;
            }
            if (_walls.ContainsKey(item.CurrentWallMaterial))
            {
                _walls[item.CurrentWallMaterial].Add(item);
            }
            else
            {
                List<Wall> _newList = new List<Wall>();
                _newList.Add(item);
                _walls.Add(item.CurrentWallMaterial, _newList);             
            }
        }

        foreach (var key in _walls.Keys)
        {
            CombineInstance[] _combineInstances = new CombineInstance[_walls[key].Count];

            for (int i = 0; i < _walls[key].Count; i++)
            {
                _combineInstances[i].subMeshIndex = 0;
                _combineInstances[i].mesh = _walls[key][i].GetComponent<MeshFilter>().sharedMesh;
                _combineInstances[i].transform = _walls[key][i].transform.localToWorldMatrix;
            }

            Mesh _finalMesh = new Mesh();
            _finalMesh.CombineMeshes(_combineInstances, true);

            Unwrapping.GenerateSecondaryUVSet(_finalMesh);

            GameObject _newGameObject = new GameObject();
            _newGameObject.name = key.name;

            _newGameObject.AddComponent<MeshFilter>().sharedMesh = _finalMesh;
            _newGameObject.AddComponent<MeshRenderer>().sharedMaterial = key.Material;
            _newGameObject.AddComponent<SurfaceMaterial>().BulletHitEffect = key.HitParticles;
            _newGameObject.AddComponent<MeshCollider>();
        }

    }

}