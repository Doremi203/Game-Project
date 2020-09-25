using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class WallMaterialsWindow : EditorWindow
{

    [MenuItem("Window/Wall Materials")]
    public static void ShowWindow() => GetWindow<WallMaterialsWindow>("Tile Wizard");

    private WallMaterial[] materials;
    private WallMaterial selectedMaterial;

    private void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
        RefreshMaterialsList();
    }

    private void OnDestroy() => SceneView.duringSceneGui -= this.OnSceneGUI;

    private void OnSceneGUI(SceneView sceneView)
    {
        if (selectedMaterial == null) return;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0 && e.isMouse)
        {
            Ray _ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(new Vector3(e.mousePosition.x, Screen.height - e.mousePosition.y - 36, 0));
            RaycastHit _hit = new RaycastHit();
            if (Physics.Raycast(_ray, out _hit, 1000.0f))
            {
                Wall _targetWall = _hit.transform.GetComponent<Wall>();
                if (_targetWall == null) return;

                _targetWall.SetMaterial(selectedMaterial);

                Event.current.Use();
            }
        }
    }

    private void Awake()
    {
        RefreshMaterialsList();
        if (materials[0]) selectedMaterial = materials[0];
    }

    private void OnGUI()
    {
        if (selectedMaterial)
            if (GUILayout.Button("Clear Selection")) selectedMaterial = null;

        foreach (var item in materials)
        {
            if(item == selectedMaterial) GUI.contentColor = Color.yellow;
            if (GUILayout.Button(item.name)) selectedMaterial = item;
            GUI.contentColor = Color.white;
        }
    }

    private void RefreshMaterialsList() => materials = WallMaterial.GetWallMaterials();

}