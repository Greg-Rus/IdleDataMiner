using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FSM<T> {
    public T currentState;
    //Dictionary<T, TransitionActionTouple<T>> transitions;
    Dictionary<T, List<TransitionActionTouple<T>>> states;

    public FSM()
    {
        states = new Dictionary<T, List<TransitionActionTouple<T>>>();
    }

    public void AddState(T state)
    {
        states.Add(state, new List<TransitionActionTouple<T>>());
    }
    public void SetState(T toState)
    {
        List<TransitionActionTouple<T>> possibleTransitions = states[currentState];
        foreach (var transition in possibleTransitions)
        {
            if (transition.toTransitionState.Equals(toState))
            {
                transition.toTransitionAction();
            }
        }
        currentState = toState;
    }
    public void AddTransition(T fromState, T toState, Action callback)
    {
        states[fromState].Add(new TransitionActionTouple<T>(toState, callback));
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
