using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputBindingUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textValue;

    private SettingsKeyCodeVariable targetBinding;
    private KeyBindsUI parent;

    public void Setup(SettingsKeyCodeVariable targetBinding, KeyBindsUI parent)
    {
        this.targetBinding = targetBinding;
        this.parent = parent;
        textName.text = targetBinding.DisplayName;
        textValue.text = targetBinding.GetValue().ToString();
    }

    public void Press()
    {
        parent.Press(targetBinding);
    }

}
