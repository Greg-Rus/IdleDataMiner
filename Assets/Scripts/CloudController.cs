using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CloudState { Idle, Downloading, Connecting, Saving}

[RequireComponent(typeof(DataConectionController))]
[RequireComponent(typeof(Repo))]
[RequireComponent(typeof(ProgressionModelCloud))]
[RequireComponent(typeof(StructureView))]
public class CloudController : MonoBehaviour {
    //Debug
    public CloudState state;

    private Repo myRepo;
    public List<IWithdrawing> consumptionRepos;
    public IDepositing productionRepo;
    private DataConectionController myDataStream;
    private int currentRepoIndex = 0;
    private ProgressionModelCloud myModel;
    private StructureView myView;

    private float timer = 0f;

    private FSM<CloudState> myFSM;
    // Use this for initialization
    void Awake()
    {
        consumptionRepos = new List<IWithdrawing>();
    }

	void Start () {
        myDataStream = GetComponent<DataConectionController>();
        myRepo = GetComponent<Repo>();
        myModel = GetComponent<ProgressionModelCloud>();
        myView = GetComponent<StructureView>();

        myFSM = new FSM<CloudState>();
        myFSM.AddState(CloudState.Idle, () => { return; });
        myFSM.AddState(CloudState.Downloading, UpdateDownloading);
        myFSM.AddState(CloudState.Saving, UpdateSaving);
        myFSM.AddState(CloudState.Connecting, UpdateConnecting);
        myFSM.AddTransition(CloudState.Idle, CloudState.Connecting, TransitionToConnecting);
        myFSM.AddTransition(CloudState.Downloading, CloudState.Downloading, TransitionToDownloading);
        myFSM.AddTransition(CloudState.Downloading, CloudState.Connecting, TransitionToConnecting);
        myFSM.AddTransition(CloudState.Connecting, CloudState.Downloading, TransitionToDownloading);
        myFSM.AddTransition(CloudState.Downloading, CloudState.Saving, TransitionToSaving);
        myFSM.AddTransition(CloudState.Saving, CloudState.Downloading, TransitionToDownloading);
        myFSM.AddTransition(CloudState.Saving, CloudState.Saving, TransitionToSaving);
        myFSM.SetState(CloudState.Idle);
        myFSM.SetState(CloudState.Connecting);
    }

    void Update()
    {
        myFSM.UpdateFSM();
        state = myFSM.currentState;
    }

    private void TransitionToConnecting()
    {
        timer = myModel.GetConnectingTime();
        SelectNextRepo();
        myDataStream.CloseDataConnection();
    }

    private void TransitionToDownloading()
    {
        timer = myModel.GetDownloadTime();
        double unitsWithdrawn = consumptionRepos[currentRepoIndex].Withdraw(myModel.GetUnitsDownloadedPerCycle());
        double unitsDeposited = myRepo.Deposit(unitsWithdrawn);
        myView.UpdateRepoLoad(myRepo.currentLoad);
        myDataStream.SetupDataConection(consumptionRepos[currentRepoIndex].GetPosition(), DataFlow.Download);
    }

    private void UpdateDownloading()
    {
        if (!IsStateTimerElapsed())
        {
            myDataStream.AnimateDataConnection(timer / myModel.GetDownloadTime());
        }
        else
        {
            if (myRepo.IsFull())    //Cloud is full. Must save data.
            {
                myFSM.SetState(CloudState.Saving);                  
            }
            else if (!consumptionRepos[currentRepoIndex].IsEmpty())     //More data in repo - keep downloading.
            {
                myFSM.SetState(CloudState.Downloading);         
            }
            else if (currentRepoIndex == consumptionRepos.Count -1)    //No more data in repo and this was the lowest repo. Save whateve was uploaded.
            {
                myFSM.SetState(CloudState.Saving);
            }
            else    //Not the lowst repo, so pick the next one
            {
                myFSM.SetState(CloudState.Connecting);              
            }
        }
    }

    private void UpdateConnecting()
    {
        if (IsStateTimerElapsed())
        {
            myFSM.SetState(CloudState.Downloading);
        }
    }

    private void TransitionToSaving()
    {
        timer = myModel.GetSaveTime();
        double unitsWithdrawn = myRepo.Withdraw(myModel.GetUnitsSavedPerCycle());
        double unitsDeposited = productionRepo.Deposit(unitsWithdrawn);
        myView.UpdateRepoLoad(myRepo.currentLoad);
        myDataStream.SetupDataConection(productionRepo.GetPosition(), DataFlow.Upload);
        
    }

    private void UpdateSaving()
    {
        if (!IsStateTimerElapsed())
        {
            myDataStream.AnimateDataConnection(timer / myModel.GetSaveTime());
        }
        else
        {
            if (myRepo.IsEmpty())
            {
                currentRepoIndex = 0; //Saving finished so lets start downloading from the top again
                myFSM.SetState(CloudState.Downloading);
            }
            else
            {
                myFSM.SetState(CloudState.Saving);
            }
        }
    }

    private void SelectNextRepo()
    {
        currentRepoIndex++;
        if (currentRepoIndex > consumptionRepos.Count - 1)
        {
            currentRepoIndex = 0;
            myFSM.SetState(CloudState.Saving);
        }
            
    }

    private bool IsStateTimerElapsed()
    {
        timer -= Time.deltaTime;
        return timer <= 0 ? true : false;
    }

    public void OnUpgrade()
    {
        myModel.ScaleTolevel(myModel.currentLevel + 1);
        myRepo.maxCapacity = myModel.currentCloudMaxCapacity;
        myView.UpdateLevel(myModel.currentLevel);
    }
}
