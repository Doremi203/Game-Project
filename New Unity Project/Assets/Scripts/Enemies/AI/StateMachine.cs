using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public event Action<BaseState> OnStateChanged;

    private Dictionary<Type, BaseState> states;
    private BaseState currentState;

    public void SetStates(Dictionary<Type, BaseState> states)
    {
        this.states = states;
    }

    private void Update()
    {
        if (currentState == null) currentState = states.Values.First();
        var nextState = currentState?.Tick();
        if(nextState != null && nextState != currentState?.GetType())
        {
            SwitchState(nextState);
        }
    }

    private void SwitchState(Type newState)
    {
        currentState = states[newState];
        OnStateChanged?.Invoke(currentState);
    }

}