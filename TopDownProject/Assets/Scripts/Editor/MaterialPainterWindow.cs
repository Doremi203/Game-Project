using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MaterialPainterWindow : EditorWindow
{

    [MenuItem("Window/Painter")]
    public static void ShowWindow() => GetWindow<MaterialPainterWindow>("Painter");

    private MaterialPresenter selectedMaterial;
    private MaterialPresenter[] materialPresenters;
    private GUIContent guiContent;
    private GameObject lastPaintedObject;

    private void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
        RefreshMaterials();
    }

    private void OnDestroy() => SceneView.duringSceneGui -= this.OnSceneGUI;

    private void RefreshMaterials()
    {
        materialPresenters = MaterialPresenter.GetMaterialPresenters();
    }

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
                if (_hit.distance > 20f) return;

                MeshRenderer _meshRenderer = _hit.transform.GetComponent<MeshRenderer>();
                if (_meshRenderer == null) return;

                Undo.RecordObject(_meshRenderer, "idk");

                _meshRenderer.sharedMaterial = selectedMaterial.Material;

                lastPaintedObject = _meshRenderer.gameObject;

                Event.current.Use();
            }
        }
    }

    private Vector2 scrollPosition;

    private void OnGUI()
    {
        if (GUILayout.Button("Stop Painting", GUILayout.Height(50))) selectedMaterial = null;

        GUILayout.Space(5f);

        if (lastPaintedObject != null) GUILayout.Label("Last painted object: " + lastPaintedObject.name);

        GUILayout.Space(5f);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

        foreach (var item in materialPresenters)
        {
            if (selectedMaterial == item)
            {
                Color _oldColor = GUI.contentColor;
                GUI.contentColor = Color.green;
                GUILayout.Label(item.name + " (Selected)");
                GUI.contentColor = _oldColor;
            }
            else
                GUILayout.Label(item.name);

            guiContent = new GUIContent(item.PreviewImage);
            if (GUILayout.Button(guiContent)) selectedMaterial = item;

            GUILayout.Space(5f);
        }

        GUILayout.EndScrollView();
    }

}