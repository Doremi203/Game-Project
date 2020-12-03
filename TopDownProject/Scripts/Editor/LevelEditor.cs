using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Level _targetLevel = target as Level;

        GUILayout.Label("First level");

        SceneAsset _sceneObject = GetSceneObject(_targetLevel.Scene);
        SceneAsset _scene = EditorGUILayout.ObjectField(_sceneObject, typeof(SceneAsset), false) as SceneAsset;
        if (_scene != null)
        {
            _targetLevel.Scene = _scene.name;
            EditorUtility.SetDirty(_targetLevel);
        }
    }

    protected SceneAsset GetSceneObject(string sceneObjectName)
    {
        if (string.IsNullOrEmpty(sceneObjectName)) return null;

        foreach (var editorScene in EditorBuildSettings.scenes)
        {
            if (editorScene.path.IndexOf(sceneObjectName) != -1)
            {
                return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
            }
        }

        Debug.LogWarning("Scene [" + sceneObjectName + "] cannot be used. Add this scene to the 'Scenes in the Build' in build settings.");
        return null;
    }

}