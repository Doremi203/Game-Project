using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{

    public Action StateChanged;
    public IState CurrentState { get; private set; }

    private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> globalTransitions = new List<Transition>();

    public Trigger CreateTrigger() => new Trigger(this);

    public void Tick()
    {
        if (FindTransition(out var transition))
            SetState(transition.To);
        (CurrentState as IStateTickCallbackReciver)?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == CurrentState) return;
        (CurrentState as IStateExitCallbackReciver)?.OnExit();
        CurrentState = state;
        (CurrentState as IStateEnterCallbackReciver)?.OnEnter();
        StateChanged?.Invoke();
    }

    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        if (!transitions.TryGetValue(from.GetType(), out var _transitions))
        {
            _transitions = new List<Transition>();
            transitions[from.GetType()] = _transitions;
        }
        _transitions.Add(new Transition(to, condition));
    }

    public void AddGlobalTransition(IState to, Func<bool> condition)
    {
        globalTransitions.Add(new Transition(to, condition));
    }

    private bool FindTransition(out Transition transition)
    {
        foreach (var item in globalTransitions)
        {
            if (item.ShouldTransist())
            {
                transition = item;
                return true;
            }
        }

        if (transitions.ContainsKey(CurrentState.GetType()))
        {
            foreach (var item in transitions[CurrentState.GetType()])
            {
                if (item.ShouldTransist())
                {
                    transition = item;
                    return true;
                }
            }
        }

        transition = default;
        return false;
    }

}