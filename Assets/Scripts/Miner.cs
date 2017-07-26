using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinerState { Idle, Consuming, Working, Producing};

public class Miner : MonoBehaviour {
    //public Repo myConsumptionRepo;
    //public Repo myProductionRepo;
    public Repo myRepo;
    public IMinerModel myModel;
    public IRepoUseage shaftRepos;

    //public Transform consumptionTarget;
    //public Transform productionTarget;

    public LineRenderer myLineRenderer;
    public float minWidth = 0.2f;
    public float maxWidth = 0.5f;

    //public double unitsMinedPerSecond = 10d;
    //public double unitsUploadedPerSecond = 50d;
    //public float consumptionTime = 1f;
    //public float productionTime = 1f;
    public MinerState state;

    private float timer = 0f;
	// Use this for initialization
	void Start ()
    {
        state = MinerState.Idle;
        SetupLineRenderer(transform.position, Color.green);
        ConfigureRepo();
    }

    private void ConfigureRepo()
    {

    }	
	// Update is called once per frame
	void Update () {
        switch (state)
        {
            case MinerState.Idle: UpdateIdle();break;
            case MinerState.Consuming: UpdateConsuming();break;
            case MinerState.Working: UpdateWorking();break;
            case MinerState.Producing: UpdateProducing();break;
        }
	}

    private void UpdateIdle()
    {
        state = MinerState.Consuming;
    }

    private void TransitionToConsuming()
    {
        state = MinerState.Consuming;
        timer = myModel.GetConsumptionCycletime();
        double unitsWithdrawn = shaftRepos.Consume(myModel.GetUnitsMinedPerSecond()); //myConsumptionRepo.Withdraw(myModel.GetUnitsMinedPerSecond());
        double unitsDeposited = myRepo.Deposit(unitsWithdrawn);
        SetupLineRenderer(shaftRepos.GetConsumptionRepoLocation(), Color.green);
    }
    private void UpdateConsuming()
    {
        if (!IsStateTimerElapsed())
        {
            AnimateLineRendererCycle(timer / myModel.GetConsumptionCycletime());
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
        state = MinerState.Working;
    }
    private void UpdateWorking()
    {
        TransitionToProducing();
    }
    private void TransitionToProducing()
    {
        state = MinerState.Producing;
        timer = myModel.GetProductionCycletime();
        double unitsWithdrawn = myRepo.Withdraw(myModel.GetUnitsUploadedPerSecond());
        double unitsDeposited = shaftRepos.Produce(unitsWithdrawn);
        SetupLineRenderer(shaftRepos.GetProductionRepoLocation(), Color.red);
    }
    private void UpdateProducing()
    {
        if (!IsStateTimerElapsed())
        {
            AnimateLineRendererCycle(timer / myModel.GetProductionCycletime());
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
        timer -= Time.unscaledDeltaTime;
        return timer <= 0 ? true : false;
    }

    private void SetupLineRenderer(Vector3 target, Color color)
    {
        myLineRenderer.SetPosition(0, transform.position);
        myLineRenderer.SetPosition(1, new Vector3(transform.position.x, target.y, transform.position.z));
        myLineRenderer.SetPosition(2, target);
        myLineRenderer.startColor = color;
        myLineRenderer.endColor = color;
    }

    private void AnimateLineRendererCycle(float t)
    {

        if (t < 0.5f)
        {
            float newSize = Mathf.Lerp(minWidth, maxWidth, t * 2);
            myLineRenderer.startWidth = newSize;
            myLineRenderer.endWidth = newSize;
        }
        else
        {
            float newSize = Mathf.Lerp(maxWidth, minWidth, t - 0.5f);
            myLineRenderer.startWidth = newSize;
            myLineRenderer.endWidth = newSize;
        }
            
    }
}
