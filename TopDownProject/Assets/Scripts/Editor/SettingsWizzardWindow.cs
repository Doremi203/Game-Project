﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SettingsWizzardWindow : EditorWindow
{

    [MenuItem("Window/Settings Wizzard")]
    public static void ShowWindow() => GetWindow<SettingsWizzardWindow>("Settings Wizzard");

    private const string SavePath = "Assets/Resources/Settings/Groups/";

    private ParametersGroup[] groups;
    private ParametersGroup selectedGroup;

    private void OnFocus() => RefreshGroups();

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.MinWidth(250f));

        DrawGroupSelector();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        DrawGroupInspector();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawGroupSelector()
    {
        if (GUILayout.Button("Create Group", GUILayout.MinHeight(30f)))
        {
            CreateGroup();
            RefreshGroups();
        }

        EditorGUILayout.Space();

        foreach (var item in groups)
        {
            GUILayout.BeginHorizontal();

            if (selectedGroup == item) GUI.contentColor = Color.green;

            if (GUILayout.Button(item.name, GUILayout.MinHeight(30f))) selectedGroup = item;

            GUI.contentColor = Color.white;

            if (GUILayout.Button("Delete", GUILayout.MinWidth(50f), GUILayout.MaxWidth(50f), GUILayout.MinHeight(30f)))
                DeleteGroup(selectedGroup);

            GUILayout.EndHorizontal();
        }
    }

    private void DrawGroupInspector()
    {
        if (!selectedGroup)
        {
            GUILayout.Label("Please select or create Settings Group");
            return;
        }

        GUILayout.Label(selectedGroup.DisplayName);

        GUILayout.Space(10f);

        string newName = GUILayout.TextArea(selectedGroup.DisplayName);
        if (newName != selectedGroup.DisplayName)
        {
            selectedGroup.DisplayName = newName;
            EditorUtility.SetDirty(selectedGroup);
        }

        GUILayout.Space(10f);

        foreach (var item in selectedGroup.parameters)
        {
            Color color = GUI.contentColor;
            if (item == Selection.activeObject) GUI.contentColor = Color.green;

            if (GUILayout.Button(item.DisplayName, GUILayout.MinHeight(30f))) Selection.activeObject = item;

            GUI.contentColor = color;
        }
    }

    private void RefreshGroups() => groups = ParametersGroup.GetGroups();

    private void CreateGroup()
    {
        ParametersGroup group = (ParametersGroup)ScriptableObject.CreateInstance(typeof(ParametersGroup));
        group.DisplayName = "newGroup";
        AssetDatabase.CreateAsset(group, SavePath + "newGroup" + ".asset");
        selectedGroup = group;
    }

    private void DeleteGroup(ParametersGroup selectedGroup)
    {
        if (EditorUtility.DisplayDialog("Delete Group", "Are you sure? The parameters won't be deleted", "Yes", "No"))
        {
            AssetDatabase.DeleteAsset(SavePath + selectedGroup.name + ".asset");
            RefreshGroups();
        }
    }

}