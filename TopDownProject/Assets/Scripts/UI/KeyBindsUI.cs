using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindsUI : MonoBehaviour
{

    [SerializeField] private InputBindingUI prefab_bindingUI;
    [SerializeField] private Transform parent;

    private bool isListening;
    private InputBinding targetInputBinding;

    public void Press(InputBinding target)
    {
        isListening = true;
        targetInputBinding = target;
    }

    private void Awake()
    {
        Refresh();
    }

    private void Update()
    {
        if(isListening)
        {
            if(Input.anyKeyDown)
            {
                foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(kcode)) targetInputBinding.SetKeyCode(kcode);
                }
                isListening = false;
                Refresh();
            }
        }
    }

    private void Refresh()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in InputBinding.GetInputBindings())
        {
            Instantiate(prefab_bindingUI, parent).Setup(item, this);
        }
    }

}