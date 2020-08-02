using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StateMachine 
{

    private IState currentState;

    private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
    private List<Transition> currentTransitions = new List<Transition>();
    private List<Transition> anyTransitions = new List<Transition>();

    private static List<Transition> emptyTransitions = new List<Transition>();

    public IState GetCurrentState()
    {
        return currentState;
    }

    public void Tick()
    {
        var _transition = GetTransition();
        if (_transition != null) SetState(_transition.To);
        currentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == currentState) return;

        currentState?.OnExit();
        currentState = state;

        transitions.TryGetValue(currentState.GetType(), out currentTransitions);
        if (currentTransitions == null)
            currentTransitions = emptyTransitions;

        currentState.OnEnter();
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if (transitions.TryGetValue(from.GetType(), out var _transitions) == false)
        {
            _transitions = new List<Transition>();
            transitions[from.GetType()] = _transitions;
        }
        _transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(state, predicate));
    }

    private class Transition
    {

        public Func<bool> Condition { get; }
        public IState To { get; }

        public Transition (IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }

    }

    private Transition GetTransition()
    {
        foreach (var _transition in anyTransitions)
        {
            if (_transition.Condition()) return _transition;
        }

        foreach (var _transition in currentTransitions)
        {
            if (_transition.Condition()) return _transition;
        }

        return null;
    }

}

public interface IState
{

    void Tick();
    void OnEnter();
    void OnExit();

}