using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TilePresenter))]
public class TilePresenterEditor : Editor
{

    public override void OnInspectorGUI()
    {
        TilePresenter _target = target as TilePresenter;

        GUILayout.Label("Rotations");

        if (GUILayout.Button("FlipX")) _target.FlipX();

        if (GUILayout.Button("Rotate +90")) _target.Rotate90();
        if (GUILayout.Button("Rotate -90")) _target.RotateMinus90();

        EditorGUILayout.Space();

        GUILayout.Label("Variations");

        for (int i = 0; i < _target.TargetTile.Variations.Count; i++)
        {
            if (GUILayout.Button(_target.TargetTile.Variations[i].name))
            {
                _target.SelectVariation(i);
            }
        }

        EditorGUILayout.Space();

        GUILayout.Label("Extra");

        if (GUILayout.Button("Reset Tile")) _target.ResetTile();

        EditorGUILayout.Space();

        TileInspector.DrawInspector(_target.TargetTile);
    }

    private void OnSceneGUI()
    {
        TilePresenter _target = target as TilePresenter;

        Handles.matrix = _target.transform.localToWorldMatrix;

        if (Handles.Button(Vector3.up * 2f, Quaternion.identity, 0.5f, 0.25f, Handles.SphereHandleCap))
        {
            int _i = _target.CurrentVariantIndex;
            if (_i + 1 >= _target.TargetTile.Variations.Count)
                _i = 0;
            else
                _i = _i + 1;
            _target.SelectVariation(_i);
        }
        if (Handles.Button(Vector3.up * 1.5f, Quaternion.identity, 0.5f, 0.22f, Handles.SphereHandleCap))
        {
            int _i = _target.CurrentVariantIndex;
            if (_i - 1 < 0)
                _i = _target.TargetTile.Variations.Count - 1;
            else
                _i = _i - 1;
            _target.SelectVariation(_i);
        }
        if (Handles.Button(Vector3.up * 3f, Quaternion.identity, 0.5f, 0.5f, Handles.SphereHandleCap))
            _target.Rotate90();
    }

}