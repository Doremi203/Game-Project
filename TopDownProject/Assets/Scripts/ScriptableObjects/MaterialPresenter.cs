using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MaterialPresenter : ScriptableObject
{

    const string SavePath = "Assets/Resources/MaterialPresenters/";

    public static void CreateMaterialPresenter(string displayName, Texture2D previewImage, Material material)
    {
        MaterialPresenter _materialPresenter = (MaterialPresenter)ScriptableObject.CreateInstance(typeof(MaterialPresenter));
        _materialPresenter.name = displayName;
        _materialPresenter.PreviewImage = previewImage;
        _materialPresenter.Material = material;

        if(!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        string _path = AssetDatabase.GenerateUniqueAssetPath(SavePath + displayName + ".asset");
        AssetDatabase.CreateAsset(_materialPresenter, _path);        
    }

    public static MaterialPresenter[] GetMaterialPresenters() => Resources.LoadAll<MaterialPresenter>("MaterialPresenters");

    public Texture2D PreviewImage;
    public Material Material;

}