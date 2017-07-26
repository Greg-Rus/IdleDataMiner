using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerState { Idle, Consuming, Working, Producing};

[RequireComponent(typeof(DataConectionController))]
public class Miner : MonoBehaviour {
    //public Repo myConsumptionRepo;
    //public Repo myProductionRepo;
    public Repo myRepo;
    public IMinerModel myModel;
    public IRepoUseage shaftRepos;
    public DataConectionController myDataStream;

    //public Transform consumptionTarget;
    //public Transform productionTarget;

    //public LineRenderer myLineRenderer;
    //public float minWidth = 0.2f;
    //public float maxWidth = 0.5f;

    //public double unitsMinedPerSecond = 10d;
    //public double unitsUploadedPerSecond = 50d;
    //public float consumptionTime = 1f;
    //public float productionTime = 1f;
    public WorkerState state;

    private float timer = 0f;
    // Use this for initialization

    FSM<WorkerState> myFSM;
	void Start ()
    {
        state = WorkerState.Idle;
        myDataStream = GetComponent<DataConectionController>();
        //SetupLineRenderer(transform.position, Color.green);
        ConfigureRepo();

        myFSM = new FSM<WorkerState>();
        myFSM.AddState(WorkerState.Idle);
        myFSM.AddState(WorkerState.Working);
        myFSM.AddState(WorkerState.Consuming);
        myFSM.AddState(WorkerState.Producing);
        myFSM.AddTransition(WorkerState.Idle, WorkerState.Working, () => Print("From Idle to Working!"));
        myFSM.AddTransition(WorkerState.Working, WorkerState.Consuming, () => Print("From Working to consuming!"));
        myFSM.SetState(WorkerState.Idle);
        myFSM.SetState(WorkerState.Working);
        myFSM.SetState(WorkerState.Consuming);
    }

    public void Print(string text)
    {
        Debug.Log(text + " in state: " + myFSM.currentState);

    }

    private void ConfigureRepo()
    {

    }	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case WorkerState.Idle: UpdateIdle();break;
            case WorkerState.Consuming: UpdateConsuming();break;
            case WorkerState.Working: UpdateWorking();break;
            case WorkerState.Producing: UpdateProducing();break;
        }
	}

    private void UpdateIdle()
    {
        state = WorkerState.Consuming;
    }

    private void TransitionToConsuming()
    {
        state = WorkerState.Consuming;
        timer = myModel.GetConsumptionCycletime();
        double unitsWithdrawn = shaftRepos.Consume(myModel.GetUnitsMinedPerSecond());
        double unitsDeposited = myRepo.Deposit(unitsWithdrawn);
        myDataStream.SetupDataConection(shaftRepos.GetConsumptionRepoLocation(), DataFlow.Download);
    }
    private void UpdateConsuming()
    {
        if (!IsStateTimerElapsed())
        {
            myDataStream.AnimateDataConnection(timer / myModel.GetConsumptionCycletime());
        }
        else
        {
            if (myRepo.IsFull())
            {
                TransitionToWorking();
            }
            else
            {
                TransitionToConsuming();
            }
        }
    }

    private void TransitionToWorking()
    {
        state = WorkerState.Working;
    }
    private void UpdateWorking()
    {
        TransitionToProducing();
    }
    private void TransitionToProducing()
    {
        state = WorkerState.Producing;
        timer = myModel.GetProductionCycletime();
        double unitsWithdrawn = myRepo.Withdraw(myModel.GetUnitsUploadedPerSecond());
        double unitsDeposited = shaftRepos.Produce(unitsWithdrawn);
        myDataStream.SetupDataConection(shaftRepos.GetProductionRepoLocation(), DataFlow.Upload);
    }
    private void UpdateProducing()
    {
        if (!IsStateTimerElapsed())
        {
            myDataStream.AnimateDataConnection(timer / myModel.GetProductionCycletime());
        }
        else
        {
            if (myRepo.IsEmpty())
            {
                TransitionToConsuming();
            }
            else
            {
                TransitionToProducing();
            }
        }
    }

    private bool IsStateTimerElapsed()
    {
        timer -= Time.deltaTime;
        return timer <= 0 ? true : false;
    }

    //private void SetupLineRenderer(Vector3 target, Color color)
    //{
    //    myLineRenderer.SetPosition(0, transform.position);
    //    myLineRenderer.SetPosition(1, new Vector3(transform.position.x, target.y, transform.position.z));
    //    myLineRenderer.SetPosition(2, target);
    //    myLineRenderer.startColor = color;
    //    myLineRenderer.endColor = color;
    //}

    //private void AnimateLineRendererCycle(float t)
    //{

    //    if (t < 0.5f)
    //    {
    //        float newSize = Mathf.Lerp(minWidth, maxWidth, t * 2);
    //        myLineRenderer.startWidth = newSize;
    //        myLineRenderer.endWidth = newSize;
    //    }
    //    else
    //    {
    //        float newSize = Mathf.Lerp(maxWidth, minWidth, t - 0.5f);
    //        myLineRenderer.startWidth = newSize;
    //        myLineRenderer.endWidth = newSize;
    //    }
            
    //}
}
