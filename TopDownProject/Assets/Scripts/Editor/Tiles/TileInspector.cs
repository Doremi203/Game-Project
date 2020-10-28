using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class TileInspector
{

    public static void DrawInspector(Tile tile)
    {
        GUILayout.Label("Add Variation");

        GameObject newVariation = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), false, GUILayout.Height(30));

        if (newVariation != null)
        {
            tile.Variations.Add(newVariation);
            EditorUtility.SetDirty(tile);
        }

        EditorGUILayout.Space();

        for (int i = 0; i < tile.Variations.Count; i++)
        {
            GameObject gameObject = (GameObject)EditorGUILayout.ObjectField(tile.Variations[i], typeof(GameObject), false);

            if (gameObject == null)
            {
                tile.Variations.RemoveAt(i);
                EditorUtility.SetDirty(tile);
            }

            if (gameObject != tile.Variations[i])
                tile.Variations[i] = gameObject;
        }
    }

}
