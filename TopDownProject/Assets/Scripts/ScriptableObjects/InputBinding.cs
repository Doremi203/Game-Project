using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InputBinding", menuName = "InputBinding")]
public class InputBinding : ScriptableObject
{

    private const string InputBindingsPath = "InputBindings";

    public static List<InputBinding> GetInputBindings()
    {
        List<InputBinding> inputBindings = new List<InputBinding>();
        foreach (var item in Resources.LoadAll<InputBinding>(InputBindingsPath))
        {
            inputBindings.Add(item);
        }
        return inputBindings;
    }

    [SerializeField] private KeyCode defaultBind;

    [SerializeField] private bool isCashed;
    [SerializeField] private KeyCode cashedValue;

    public KeyCode GetKeyCode()
    {
        if (isCashed)
        {
            return cashedValue;
        }
        else
        {
            if (PlayerPrefs.HasKey(this.name))
            {
                cashedValue = (KeyCode)PlayerPrefs.GetInt(this.name);
                isCashed = true;
                return (KeyCode)PlayerPrefs.GetInt(this.name);
            }
        }
        return defaultBind;
    }

    public void SetKeyCode(KeyCode keyCode)
    {
        isCashed = false;
        PlayerPrefs.SetInt(this.name, (int)keyCode);
    }

    public void Reset()
    {
        isCashed = false;
        PlayerPrefs.DeleteKey(this.name);
    }

}