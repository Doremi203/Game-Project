using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputBindingUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textValue;

    private InputBinding targetBinding;
    private KeyBindsUI parent;

    public void Setup(InputBinding targetBinding, KeyBindsUI parent)
    {
        this.targetBinding = targetBinding;
        this.parent = parent;
        textName.text = targetBinding.name;
        textValue.text = targetBinding.GetKeyCode().ToString();
    }

    public void Press()
    {
        parent.Press(targetBinding);
    }

}
