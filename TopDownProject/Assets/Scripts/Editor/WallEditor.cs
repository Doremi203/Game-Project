using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Wall)), CanEditMultipleObjects]
public class WallEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Wall _wall = target as Wall;
        WallMaterial _material = (WallMaterial)EditorGUILayout.ObjectField(_wall.CurrentWallMaterial, typeof(WallMaterial), false, GUILayout.Height(50));
        if (_material != _wall.CurrentWallMaterial && _material != null)
        {
            foreach (var item in targets)
            {
                Wall _currentWall = item as Wall;
                _currentWall.SetMaterial(_material);
                EditorUtility.SetDirty(item);
            }
        }
    }

}