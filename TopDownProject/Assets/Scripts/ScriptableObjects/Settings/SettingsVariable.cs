using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SettingsVariableBase : ScriptableObject
{

    public static Action OnSettingsChanged;

}

public abstract class SettingsVariable<T> : SettingsVariableBase
{

    public string DisplayName => displayName;

    [SerializeField] private string displayName;
    [SerializeField] private T defaultValue;

    private T cashedValue;
    private bool isCashed;

    public void SetValue(T value)
    {
        isCashed = false;
        SaveValue(value);
        OnSettingsChanged?.Invoke();
    }

    public T GetValue()
    {
        if (isCashed)
            return cashedValue;

        if (PlayerPrefs.HasKey(this.name))
        {
            cashedValue = LoadValue();
            isCashed = true;
            return cashedValue;
        }

        return defaultValue;
    }

    public void ResetValue() => SetValue(defaultValue);

    protected abstract T LoadValue();

    protected abstract void SaveValue(T value);

}