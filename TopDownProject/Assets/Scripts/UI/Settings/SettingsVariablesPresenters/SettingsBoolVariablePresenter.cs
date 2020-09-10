using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsBoolVariablePresenter : SettingsVariablePresenter<SettingsBoolVariable>
{

    [SerializeField] private Dropdown dropdown;

    protected override void Present(SettingsBoolVariable settingsVariable)
    {
        dropdown.value = settingsVariable.GetValue() ? 1 : 0;
        dropdown.gameObject.SetActive(true);
    }

    public void OnValueChanged(int value) => settingsVariable.SetValue(value == 1);

}