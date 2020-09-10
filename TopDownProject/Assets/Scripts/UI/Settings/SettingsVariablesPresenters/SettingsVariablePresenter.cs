using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SettingsVariablePresenter<T> : SettingsBaseVariablePresenter where T : SettingsVariableBase
{

    [SerializeField] private TextMeshProUGUI variableNameText;

    protected T settingsVariable;

    public override Type TargetType() => typeof(T);

    public override void Setup(SettingsVariableBase settingsVariable)
    {
        this.settingsVariable = settingsVariable as T;
        variableNameText.text = settingsVariable.DisplayName;
        Present(settingsVariable as T);
    }

    protected abstract void Present(T settingsVariable);

}