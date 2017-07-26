using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {
    public WorkerState state;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case WorkerState.Idle: UpdateIdle(); break;
            case WorkerState.Consuming: UpdateConsuming(); break;
            case WorkerState.Working: UpdateWorking(); break;
            case WorkerState.Producing: UpdateProducing(); break;
        }
    }

    private void UpdateIdle()
    { }

    private void UpdateConsuming()
    { }

    private void UpdateWorking()
    { }

    private void UpdateProducing()
    { }
}
