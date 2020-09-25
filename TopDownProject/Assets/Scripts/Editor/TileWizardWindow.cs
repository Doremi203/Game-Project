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

    private void Awake()
    {
        RefreshTiles();
        if (tiles[0]) selectedTile = tiles[0]; 
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.MinWidth(250f));

        DrawTilesSelector();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        DrawTileInspector();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawTileInspector()
    {
        if (selectedTile == false) return;

        GUI.contentColor = Color.white;

        string _name = EditorGUILayout.DelayedTextField("Tile Name", selectedTile.name);

        if (_name != selectedTile.name)
        {
            AssetDatabase.RenameAsset(SavePath + selectedTile.name + ".asset", _name);
        }

        EditorGUILayout.Space();

        GUILayout.Label("Create new variation:");

        GameObject _newVariation = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), false, GUILayout.Height(50));
        if (_newVariation != null) selectedTile.Variations.Add(_newVariation);

        EditorGUILayout.Space();

        for (int i = 0; i < selectedTile.Variations.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            GameObject _gameObject = (GameObject)EditorGUILayout.ObjectField(selectedTile.Variations[i], typeof(GameObject), false);
            if(_gameObject != selectedTile.Variations[i])
            {
                selectedTile.Variations[i] = _gameObject;
                EditorUtility.SetDirty(selectedTile);
            }
            if (GUILayout.Button("Remove")) selectedTile.Variations.Remove(selectedTile.Variations[i]);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        if (IsReady(selectedTile))
        {
            if (GUILayout.Button("Instantiate tile", GUILayout.MinHeight(30f))) InstantiateTile(selectedTile);
        }
        else
        {
            GUI.contentColor = Color.yellow;
            GUILayout.Label("Every tile should have at least 1 variation.");
            GUI.contentColor = Color.white;
        }
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

            if (GUILayout.Button(tiles[i].name, GUILayout.MinHeight(30f))) selectedTile = tiles[i];

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
                if (item.TargetTile == selectedTile)
                    item.ResetTile();
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