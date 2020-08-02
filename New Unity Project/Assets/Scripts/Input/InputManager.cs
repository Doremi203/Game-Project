using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Dictionary<IAction, string> keyMap;

    private void Awake()
    {
        keyMap = new Dictionary<IAction, string>();
        AddAction(null,"Space");
    }

    private void AddAction(IAction action, string key)
    {
        var duplicateExists = keyMap.Any(kvp => kvp.Value.Equals(key));
        if (duplicateExists)
        {
            throw new ArgumentException("Дубликат");
        }
        else
        {
            keyMap.Add(action, key); //Возможна ошибка словаря при дублировании action
        }
    }
}
