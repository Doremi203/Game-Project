using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{

    [SerializeField] private SettingsGroupButtonUI groupButtonPrefab;
    [SerializeField] private Transform groupButtonsParent;

    private void Start() => SetupGroups();

    private void SetupGroups()
    {
        foreach (var item in SettingsGroup.GetSettingsGroups())
        {
            SettingsGroupButtonUI _groupButton = Instantiate(groupButtonPrefab, groupButtonsParent);
            _groupButton.Setup(this, item);        
        }
    }

    internal void SelectGroup(SettingsGroup settingsGroup)
    {
        Debug.Log(settingsGroup.DisplayName);
    }

}