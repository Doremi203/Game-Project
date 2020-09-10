using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{

    [SerializeField] private SettingsGroupButtonUI groupButtonPrefab;
    [SerializeField] private Transform groupButtonsParent;
    [SerializeField] private SettingsBaseVariablePresenter[] presentersPrefabs;
    [SerializeField] private Transform variablesParent;

    // Может лучше так. Чем какой-то рандомный список который даже ошибку не выдаст.
    [SerializeField] private SettingsBoolVariablePresenter boolPresenter;

    private void Start() => SetupGroups();

    private void SetupGroups()
    {
        SettingsGroup[] _groups = SettingsGroup.GetSettingsGroups();
        foreach (var item in _groups)
        {
            SettingsGroupButtonUI _groupButton = Instantiate(groupButtonPrefab, groupButtonsParent);
            _groupButton.Setup(this, item);        
        }
        SelectGroup(_groups[0]);
    }

    public void SelectGroup(SettingsGroup settingsGroup)
    {
        ClearSelectedGroup();
        foreach (var _variable in settingsGroup.SettingsVariables)
        {
            foreach (var _presenterPrefab in presentersPrefabs)
            {
                if (_presenterPrefab.TargetType() != _variable.GetType()) continue;
                Instantiate(_presenterPrefab, variablesParent).Setup(_variable);
                break;
            }
        }
    }

    private void ClearSelectedGroup()
    {
        foreach (Transform item in variablesParent)
            Destroy(item.gameObject);
    }

}