using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ParameterPresenter<T> : BaseParameterPresenter where T : BaseParameter
{

    [SerializeField] private TextMeshProUGUI variableNameText;

    protected T settingsVariable;

    public override Type TargetType() => typeof(T);

    public override void Setup(BaseParameter settingsVariable)
    {
        this.settingsVariable = settingsVariable as T;
        variableNameText.text = settingsVariable.DisplayName;
        Present(settingsVariable as T);
    }

    protected abstract void Present(T settingsVariable);

}