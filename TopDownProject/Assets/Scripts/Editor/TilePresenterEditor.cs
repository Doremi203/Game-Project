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

        GUILayout.Label("Add Variation");

        GameObject _newVariation = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), false, GUILayout.Height(30));

        if(_newVariation != null) _target.TargetTile.Variations.Add(_newVariation);

        EditorGUILayout.Space();

        for (int i = 0; i < _target.TargetTile.Variations.Count; i++)
        {
            GameObject _gameObject = (GameObject)EditorGUILayout.ObjectField(_target.TargetTile.Variations[i], typeof(GameObject), false);

            if(_gameObject == null)
            {
                _target.TargetTile.Variations.RemoveAt(i);
            }

            if(_gameObject != _target.TargetTile.Variations[i])
            _target.TargetTile.Variations[i] = _gameObject;
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Reset Tile")) _target.ResetTile();

    }

    private void OnSceneGUI()
    {
        TilePresenter _target = target as TilePresenter;

        Handles.matrix = _target.transform.localToWorldMatrix;

        if (Handles.Button(Vector3.up * 1.5f, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap))
        {
            int _i = _target.CurrentVariantIndex;
            if (_i + 1 >= _target.TargetTile.Variations.Count)
                _i = 0;
            else
                _i = _i + 1;
            _target.SelectVariation(_i);
        }
        if (Handles.Button(Vector3.up * 0.5f, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleHandleCap))
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