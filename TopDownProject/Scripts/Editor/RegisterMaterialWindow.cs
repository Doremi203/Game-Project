using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class RegisterMaterialWindow : EditorWindow
{

    const string SavePath = "Assets/Textures/MaterialPresentersPreviews/";

    [MenuItem("Window/Register Material")]
    public static void ShowWindow() => GetWindow<RegisterMaterialWindow>("Register Material");

    private string displayName;
    private Material material;

    private void OnGUI()
    {
        GUILayout.Label("Material Name");
        displayName = GUILayout.TextField(displayName);

        GUILayout.Label("Material");
        material = (Material)EditorGUILayout.ObjectField(material, typeof(Material), false, GUILayout.Height(50));
        
        if (!material) return;

        if (GUILayout.Button("Register Material"))
        {
            GameObject _cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _cube.transform.position = new Vector3(0, 10000, 0);
            _cube.GetComponent<MeshRenderer>().sharedMaterial = material;

            Camera _camera = new GameObject().AddComponent<Camera>();
            _camera.transform.position = new Vector3(0, 10000, -1);
            _camera.enabled = false;

            RenderTexture _currentRenderTexture = RenderTexture.active;

            RenderTexture _targetRenderTexture = RenderTexture.GetTemporary(50, 50);

            _camera.targetTexture = _targetRenderTexture;
            RenderTexture.active = _targetRenderTexture;

            _camera.Render();

            Texture2D _preview = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
            _preview.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
            _preview.Apply();

            RenderTexture.active = _currentRenderTexture;

            _targetRenderTexture.Release();

            DestroyImmediate(_cube.gameObject);
            DestroyImmediate(_camera.gameObject);

            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            string _uniqueName = AssetDatabase.GenerateUniqueAssetPath(SavePath + "/" + displayName + ".asset");

            AssetDatabase.CreateAsset(_preview, _uniqueName);

            MaterialPresenter.CreateMaterialPresenter(displayName, _preview, material);

            displayName = default;
            material = default;
        }
    }

}