using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilePresenter))]
public class TilePresenterEditor : Editor
{

    public override void OnInspectorGUI()
    {
        TilePresenter _target = (TilePresenter)target;

        GUILayout.Label("Variations");

        for (int i = 0; i < _target.TargetTile.Variations.Count; i++)
        {
            if (GUILayout.Button(_target.TargetTile.Variations[i].name))
            {
                _target.SelectVariation(i);
            }
        }

        EditorGUILayout.Space();

        GUILayout.Label("Rotations");

        if (GUILayout.Button("FlipX")) _target.FlipX();
        //if (GUILayout.Button("FlipZ")) _target.FlipZ();

        if (GUILayout.Button("Rotate +90")) _target.Rotate90();
        if (GUILayout.Button("Rotate -90")) _target.RotateMinus90();
    }

}