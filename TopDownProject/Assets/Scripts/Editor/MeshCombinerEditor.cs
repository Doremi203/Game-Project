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

}