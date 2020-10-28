using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TileWizardWindow : EditorWindow
{

    [MenuItem("Window/Tiles Wizzard")]
    public static void ShowWindow() => GetWindow<TileWizardWindow>("Tile Wizard");

    private const string SavePath = "Assets/Resources/Tiles/";

    private List<Tile> tiles = new List<Tile>();
    private Tile selectedTile;
    private MaterialPresenter selectedMaterial;

    private void Awake()
    {
        RefreshTiles();
        if (tiles[0]) selectedTile = tiles[0]; 
    }

    private void OnGUI()
    {
        selectedMaterial = (MaterialPresenter)EditorGUILayout.ObjectField(selectedMaterial, typeof(MaterialPresenter), false, GUILayout.MinHeight(30f));
        DrawTilesSelector();
    }

    private void DrawTilesSelector()
    {
        if (GUILayout.Button("Reinstantiate Tiles", GUILayout.MinHeight(30f))) ReinstantiateTiles();

        if (GUILayout.Button("Create Tile", GUILayout.MinHeight(30f)))
        {
            CreateNewTile();
            RefreshTiles();
        }

        EditorGUILayout.Space();

        if (tiles.Count <= 0)
        {
            GUILayout.Label("There are no tile assets found!");
            return;
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            GUILayout.BeginHorizontal();

            if (selectedTile == tiles[i]) GUI.contentColor = Color.green;

            if (GUILayout.Button(tiles[i].name, GUILayout.MinHeight(30f)))
            {
                selectedTile = tiles[i];
                Selection.activeObject = tiles[i];              
            }

            GUI.contentColor = Color.white;

            if (GUILayout.Button("Delete", GUILayout.MinWidth(50f), GUILayout.MaxWidth(50f), GUILayout.MinHeight(30f)))
                RemoveTile(selectedTile);

            GUILayout.EndHorizontal();
        }
    }

    private void ReinstantiateTiles()
    {
        if (EditorUtility.DisplayDialog("Reinstantiate Tiles", "Are you sure?", "Yes", "No"))
        {
            foreach (var item in FindObjectsOfType<TilePresenter>())
            {
                // if (item.TargetTile != selectedTile) continue;

                MeshRenderer[] meshRenderers = item.GetComponentsInChildren<MeshRenderer>(true);
                Material[] materials = new Material[meshRenderers.Length];

                for (int i = 0; i < meshRenderers.Length; i++)
                    materials[i] = meshRenderers[i].sharedMaterial;

                item.ResetTile();

                MeshRenderer[] newMeshRenderers = item.GetComponentsInChildren<MeshRenderer>(true);

                for (int i = 0; i < newMeshRenderers.Length; i++)
                    newMeshRenderers[i].sharedMaterial = materials[i];
            }
        }
    }

    private void RemoveTile(Tile tile)
    {
        if (EditorUtility.DisplayDialog("Remove Tile", "Are you sure?", "Yes", "No"))
        {
            AssetDatabase.DeleteAsset(SavePath + tile.name + ".asset");
            RefreshTiles();
        }
    }

    private void InstantiateTile(Tile tile)
    {
        GameObject _gameObject = new GameObject();
        _gameObject.name = tile.name;
        _gameObject.AddComponent<TilePresenter>().Setup(tile);
        Selection.activeGameObject = _gameObject;

        if (!selectedMaterial) return;

        foreach (var item in _gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            item.sharedMaterial = selectedMaterial.Material;
        }
    }


    private void CreateNewTile()
    {
        Tile _tile = (Tile)ScriptableObject.CreateInstance(typeof(Tile));
        _tile.name = "new tile";
        AssetDatabase.CreateAsset(_tile, SavePath + "newTile" + ".asset");
        selectedTile = _tile;
    }

    private void RefreshTiles()
    {
        tiles.Clear();
        foreach (var item in Tile.GetTiles()) tiles.Add(item);
    }

    private bool IsReady(Tile tile)
    {
        if (tile.Variations.Count < 1) return false;

        foreach (var item in tile.Variations)
        {
            if (item == null) return false;
        }

        return true;
    }

}