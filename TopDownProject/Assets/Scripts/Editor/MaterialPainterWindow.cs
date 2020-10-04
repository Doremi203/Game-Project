using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MaterialPainterWindow : EditorWindow
{

    [MenuItem("Window/Painter")]
    public static void ShowWindow() => GetWindow<MaterialPainterWindow>("Painter");

    private Material selectedMaterial;
    private bool isPainting;

    private void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
        isPainting = true;
    }

    private void OnDestroy() => SceneView.duringSceneGui -= this.OnSceneGUI;

    private void OnSceneGUI(SceneView sceneView)
    {
        if (selectedMaterial == null) return;
        if (isPainting == false) return;

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

                _meshRenderer.sharedMaterial = selectedMaterial;

                Event.current.Use();
            }
        }
    }

    private void OnGUI()
    {
        isPainting = GUILayout.Toggle(isPainting, "Paint");
        Material _material = (Material)EditorGUILayout.ObjectField(selectedMaterial, typeof(Material), false, GUILayout.Height(50));
        if (_material != selectedMaterial) selectedMaterial = _material;
    }

}