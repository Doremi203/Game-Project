using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SettingsKeyCodeVariablePresenter : SettingsVariablePresenter<SettingsKeyCodeVariable>
{

    [SerializeField] private TextMeshProUGUI keyCodeText;

    private bool isListening;

    protected override void Present(SettingsKeyCodeVariable settingsVariable)
    {
        keyCodeText.text = settingsVariable.GetValue().ToString();
    }

    public void StartListeningForInput()
    {
        isListening = true;
        keyCodeText.text = "Press any key";
    }

    private void Update()
    {
        if (!isListening) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndListeningForInput();
            return;
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode item in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(item))
                {
                    settingsVariable.SetValue(item);
                    EndListeningForInput();
                    isListening = false;
                    return;
                }
            }
        }
    }

    public void EndListeningForInput()
    {
        keyCodeText.text = settingsVariable.GetValue().ToString();
        isListening = false;
    }

}