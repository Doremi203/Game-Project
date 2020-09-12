using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings Group", menuName = "Settings/Group")]
public class ParametersGroup : ScriptableObject
{

    private const string Path = "Settings/Groups";

    public static ParametersGroup[] GetSettingsGroups() => Resources.LoadAll<ParametersGroup>(Path);

    public string DisplayName => displayName;
    public List<BaseParameter> Parameters => parameters;

    [SerializeField] private string displayName;
    [SerializeField] public List<BaseParameter> parameters;

}