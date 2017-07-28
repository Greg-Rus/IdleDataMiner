using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerState { Idle, Consuming, Working, Producing};

[RequireComponent(typeof(DataConectionController))]
public class Miner : MonoBehaviour {
    public Repo myRepo;
    public IMinerModel myModel;
    public IDepositing myProductionRepo;
    public IWithdrawing myConsumptionRepo;
    public DataConectionController myDataStream;

    private float timer = 0f;

    FSM<WorkerState> myFSM;
	void Start ()
    {
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
        double unitsWithdrawn = myConsumptionRepo.Withdraw(myModel.GetUnitsMinedPerSecond());
        myRepo.Deposit(unitsWithdrawn);
        myDataStream.SetupDataConection(myConsumptionRepo.GetPosition(), DataFlow.Download);
    }
    private void UpdateConsuming()
    {
        if (!IsStateTimerElapsed())
        {
            myDataStream.AnimateDataConnection(timer / myModel.GetConsumptionCycletime());
        }
        else
        {
            if (myRepo.IsFull())                        //Personal repo full. Produce units to destination repo.
            {
                myFSM.SetState(WorkerState.Producing);
            }
            else
            {
                myFSM.SetState(WorkerState.Consuming);  //Personal repo not yet full. Keep mining.
            }
        }
    }

    private void TransitionToProducing()
    {
        timer = myModel.GetProductionCycletime();
        double unitsWithdrawn = myRepo.Withdraw(myModel.GetUnitsUploadedPerSecond());
        myProductionRepo.Deposit(unitsWithdrawn);
        myDataStream.SetupDataConection(myProductionRepo.GetPosition(), DataFlow.Upload);
    }
    private void UpdateProducing()
    {
        if (!IsStateTimerElapsed())
        {
            myDataStream.AnimateDataConnection(timer / myModel.GetProductionCycletime());
        }
        else
        {
            if (myRepo.IsEmpty())                       //Personal repo emtied to destination repo. Restart mining.
            {
                myFSM.SetState(WorkerState.Consuming);
            }
            else
            {
                myFSM.SetState(WorkerState.Producing);  //Personal repo not yet empty. Keep producing.
            }
        }
    }

    private bool IsStateTimerElapsed()
    {
        timer -= Time.deltaTime;
        return timer <= 0 ? true : false;
    }
}
