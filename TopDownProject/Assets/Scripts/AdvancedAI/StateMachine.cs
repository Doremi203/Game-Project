using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdvancedAI
{

    public class StateMachine
    {

        public IState CurrentState => currentState;

        private IState currentState;

        // Values
        private Dictionary<string, float> floatParameters = new Dictionary<string, float>();
        private Dictionary<string, int> intParameters = new Dictionary<string, int>();
        private Dictionary<string, bool> boolParameters = new Dictionary<string, bool>();
        private Dictionary<string, bool> triggerParameters = new Dictionary<string, bool>();

        // Transitions
        private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> globalTransitions = new List<Transition>();

        public void Tick()
        {
            if (FindTransition(out var transition))
                SetState(transition.To);
            currentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (state == currentState) return;
            currentState?.OnExit();
            currentState = state;
            currentState.OnEnter();
            // Reset Triggers
            foreach (var key in triggerParameters.Keys.ToList())
                triggerParameters[key] = false;
        }

        public void AddFloat(string key, float defaultValue) => floatParameters.Add(key, defaultValue);

        public void AddInt(string key, int defaultValue) => intParameters.Add(key, defaultValue);

        public void AddBool(string key, bool defaultValue) => boolParameters.Add(key, defaultValue);

        public void AddTrigger(string key) => triggerParameters.Add(key, false);

        public float ReadFloat(string key)
        {
            float value;
            if (floatParameters.TryGetValue(key, out value)) return value;
            Debug.LogError("Float parameter with key" + key + " is not found!");
            return value;
        }

        public int ReadInt(string key)
        {
            int value;
            if (intParameters.TryGetValue(key, out value)) return value;
            Debug.LogError("Int parameter with key" + key + " is not found!");
            return value;
        }

        public bool ReadBool(string key)
        {
            bool value;
            if (boolParameters.TryGetValue(key, out value)) return value;
            Debug.LogError("Bool parameter with key" + key + " is not found!");
            return value;
        }

        public bool IsTriggerActive(string key)
        {
            bool value;
            if (triggerParameters.TryGetValue(key, out value)) return value;
            Debug.LogError("Trigger with key" + key + " is not found!");
            return value;
        }

        public void SetFloat(string key, float value)
        {
            if (floatParameters.ContainsKey(key))
                floatParameters[key] = value;
            else
                Debug.LogError("Float parameter with key" + key + " is not found!");
        }

        public void SetInt(string key, int value)
        {
            if (intParameters.ContainsKey(key))
                intParameters[key] = value;
            else
                Debug.LogError("Int parameter with key" + key + " is not found!");
        }

        public void SetBool(string key, bool value)
        {
            if (boolParameters.ContainsKey(key))
                boolParameters[key] = value;
            else
                Debug.LogError("Bool parameter with key" + key + " is not found!");
        }

        public void SetTrigger(string key)
        {
            if (triggerParameters.ContainsKey(key))
                triggerParameters[key] = true;
            else
                Debug.LogError("Trigger with key" + key + " is not found!");
        }

        public void AddTransition(IState from, IState to, Condition[] conditions)
        {
            if (!transitions.TryGetValue(from.GetType(), out var _transitions))
            {
                _transitions = new List<Transition>();
                transitions[from.GetType()] = _transitions;
            }
            _transitions.Add(new Transition(to, conditions));
        }

        public void AddGlobalTransition(IState to, Condition[] conditions)
        {
            globalTransitions.Add(new Transition(to, conditions));
        }

        private bool FindTransition(out Transition transition)
        {
            foreach (var item in globalTransitions)
            {
                if (item.ShouldTransist(this))
                {
                    transition = item;
                    return true;
                }
            }

            if (transitions.ContainsKey(currentState.GetType()))
            {
                foreach (var item in transitions[currentState.GetType()])
                {
                    if (item.ShouldTransist(this))
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

}