using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsGroupButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI groupNameText;

    private SettingsGroup settingsGroup;
    private SettingsUI settingsUI;

    public void Setup(SettingsUI settingsUI, SettingsGroup settingsGroup)
    {
        this.settingsUI = settingsUI;
        this.settingsGroup = settingsGroup;
        groupNameText.text = settingsGroup.DisplayName;
    }

    public void ButtonClicked() => settingsUI.SelectGroup(settingsGroup);

}