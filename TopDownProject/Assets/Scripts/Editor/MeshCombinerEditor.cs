using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCombainer))]
public class MeshCombinerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        MeshCombainer _meshCombiner = target as MeshCombainer;
        if (GUILayout.Button("Combine Meshes"))
        {
            _meshCombiner.CombineMeshes();
        }
    }

    [MenuItem("Window/Combine Walls")]
    public static void CombineMeshes()
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
            
        }

    }

}