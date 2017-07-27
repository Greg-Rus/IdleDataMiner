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

        myFSM = new FSM<WorkerState>();
        myFSM.AddState(WorkerState.Idle, () => { return; });
        myFSM.AddState(WorkerState.Consuming, UpdateConsuming);
        myFSM.AddState(WorkerState.Producing, UpdateProducing);
        myFSM.AddTransition(WorkerState.Idle, WorkerState.Consuming, TransitionToConsuming);
        myFSM.AddTransition(WorkerState.Consuming, WorkerState.Producing, TransitionToProducing);
        myFSM.AddTransition(WorkerState.Consuming, WorkerState.Consuming, TransitionToConsuming);
        myFSM.AddTransition(WorkerState.Producing, WorkerState.Consuming, TransitionToConsuming);
        myFSM.AddTransition(WorkerState.Producing, WorkerState.Producing, TransitionToProducing);
        myFSM.SetState(WorkerState.Idle);
        myFSM.SetState(WorkerState.Consuming);
    }

	void Update () {
        myFSM.UpdateFSM();
	}

    private void TransitionToConsuming()
    {
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
                myFSM.SetState(WorkerState.Producing);
            }
            else
            {
                myFSM.SetState(WorkerState.Consuming);
            }
        }
    }

    private void TransitionToProducing()
    {
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
                myFSM.SetState(WorkerState.Consuming);
            }
            else
            {
                myFSM.SetState(WorkerState.Producing);
            }
        }
    }

    private bool IsStateTimerElapsed()
    {
        timer -= Time.deltaTime;
        return timer <= 0 ? true : false;
    }
}
