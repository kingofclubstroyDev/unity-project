﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    bool changedStateThisUpdate = false;
    [SerializeField] public AIVariables AIVariables;
    [HideInInspector] float stateTimeElapsed;
    [SerializeField] State currentState;
    [SerializeField] List<Transition> generalTransitions;

    
    [SerializeField] NavMeshAgent _agent;
    public NavMeshAgent agent {
        get {return _agent;}
    }
    private GameObject _target;
    public GameObject target
    {
        get { return _target; }
    }

    private float timeTargetChosen;

    // Start is called before the first frame update
    void Start()
    {
        AIVariables = GetComponent<AIVariables>();
        _agent.speed = AIVariables.moveSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        changedStateThisUpdate = false;
        foreach(Transition transition in generalTransitions) {

            if(transition.CheckTransition(this)) {

                TransitionToState(transition.getState());
                break;

            }
        }

        currentState?.UpdateState(this);
        
    }

    private void OnDrawGizmos()
    {
        // if(currentState != null)
        // {
        //     //todo: Draw gizmos if we want them
        // }
    }

    public void TransitionToState(State nextState)
    {
        currentState = nextState;
        if (changedStateThisUpdate == false)
        {
            OnExitState();
            changedStateThisUpdate = true;
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    //This will be for some manager to add some state transitions
    public void addTransition(Transition transition, int index) {
        generalTransitions.Insert(index, transition);
    }

    public void removeTransition(Transition transition) {

        generalTransitions.Remove(transition);

    }

    public void setTarget(GameObject newTarget) {
        _target = newTarget;
        timeTargetChosen = Time.time;
    }

    public bool checkNewTarget() {
        return (Time.time - timeTargetChosen >= AIVariables.timeToCheckNewEnemy);
    }

}