using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings Group", menuName = "Settings/Group")]
public class SettingsGroup : ScriptableObject
{

    private const string Path = "Settings/Groups";

    public static SettingsGroup[] GetSettingsGroups() => Resources.LoadAll<SettingsGroup>(Path);

    public string DisplayName => displayName;
    public List<SettingsVariableBase> SettingsVariables => settingsVariables;

    [SerializeField] private string displayName;
    [SerializeField] public List<SettingsVariableBase> settingsVariables;

}