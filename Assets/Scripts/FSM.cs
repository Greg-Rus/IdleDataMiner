using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FSM<T> {
    public T currentState;
    Dictionary<T, Action> states;
    Dictionary<T, List<TransitionActionTouple<T>>> transitions;

    public FSM()
    {
        states = new Dictionary<T, Action>();
        transitions = new Dictionary<T, List<TransitionActionTouple<T>>>();
    }

    public void AddState(T state, Action stateUpdateCallback)
    {
        states.Add(state, stateUpdateCallback);
        transitions.Add(state, new List<TransitionActionTouple<T>>());
    }
    public void SetState(T toState)
    {
        List<TransitionActionTouple<T>> possibleTransitions = transitions[currentState];
        foreach (var transition in possibleTransitions)
        {
            if (transition.toTransitionState.Equals(toState))
            {
                transition.toTransitionAction();
            }
        }
        currentState = toState;
    }
    public void UpdateFSM()
    {
        Action stateAction = states[currentState];
        stateAction();
    }
    public void AddTransition(T fromState, T toState, Action callback)
    {
        transitions[fromState].Add(new TransitionActionTouple<T>(toState, callback));
    }
}
public struct TransitionActionTouple<T>
{
    public T toTransitionState;
    public Action toTransitionAction;

    public TransitionActionTouple(T toState, Action action)
    {
        toTransitionState = toState;
        toTransitionAction = action;
    }
}
