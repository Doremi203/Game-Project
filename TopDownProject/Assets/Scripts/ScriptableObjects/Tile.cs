using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : ScriptableObject
{

    public const string TilesPath = "Tiles";

    public static Tile[] GetTiles() => Resources.LoadAll<Tile>(TilesPath);

    public List<GameObject> Variations = new List<GameObject>();

}