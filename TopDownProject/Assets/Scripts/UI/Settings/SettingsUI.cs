using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{

    [SerializeField] private SettingsGroupButtonUI groupButtonPrefab;
    [SerializeField] private Transform groupButtonsParent;
    [SerializeField] private BaseParameterPresenter[] presentersPrefabs;
    [SerializeField] private Transform variablesParent;

    private void Start() => SetupGroups();

    private void SetupGroups()
    {
        ParametersGroup[] _groups = ParametersGroup.GetGroups();
        foreach (var item in _groups)
        {
            SettingsGroupButtonUI _groupButton = Instantiate(groupButtonPrefab, groupButtonsParent);
            _groupButton.Setup(this, item);        
        }
        SelectGroup(_groups[0]);
    }

    public void SelectGroup(ParametersGroup settingsGroup)
    {
        ClearSelectedGroup();
        foreach (var _parameter in settingsGroup.Parameters)
        {
            foreach (var _presenterPrefab in presentersPrefabs)
            {
                if (_presenterPrefab.TargetType() != _parameter.GetType()) continue;
                Instantiate(_presenterPrefab, variablesParent).Setup(_parameter);
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