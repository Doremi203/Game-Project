using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshCombinerEditor : Editor
{

    [MenuItem("Tools/Combine Selected Meshes")]
    public static void CombineMeshes()
    {

        Dictionary<UnityEngine.Material, List<MeshRenderer>> _targets = new Dictionary<UnityEngine.Material, List<MeshRenderer>>();

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

}