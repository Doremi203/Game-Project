using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BoolParameterPresenter : ParameterPresenter<BoolParameter>
{

    [SerializeField] private Dropdown dropdown;

    protected override void Present(BoolParameter settingsVariable)
    {
        dropdown.value = settingsVariable.GetValue() ? 1 : 0;
        dropdown.gameObject.SetActive(true);
    }

    private void OnEnable() => dropdown.onValueChanged.AddListener(OnValueChanged);

    private void OnDisable() => dropdown.onValueChanged.RemoveListener(OnValueChanged);

    public void OnValueChanged(int value) => settingsVariable.SetValue(value == 1);

}