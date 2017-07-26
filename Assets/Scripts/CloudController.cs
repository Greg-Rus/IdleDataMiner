using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CloudState { Idle, Downloading, Connecting, Saving}

[RequireComponent(typeof(DataConectionController))]
[RequireComponent(typeof(Repo))]
public class CloudController : MonoBehaviour {
    public CloudState state;
    private Repo myRepo;
    public List<Repo> consumptionRepos;
    public Repo productionRepo;
    private DataConectionController myDataStream;
    private int currentRepoIndex = 0;


    public float connectingTime = 0.5f;
    public float downloadTime = 1f;
    public double unitsDownloadedPerCycle = 100d;
    public double unitsSavedPerCycle = 1000d;
    public float saveTime = 1f;

    private float timer = 0f;
    // Use this for initialization
    void Awake()
    {
        consumptionRepos = new List<Repo>();
    }

	void Start () {
        myDataStream = GetComponent<DataConectionController>();
        myRepo = GetComponent<Repo>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case CloudState.Idle: UpdateIdle(); break;
            case CloudState.Downloading: UpdateDownloading(); break;
            case CloudState.Connecting: UpdateConnecting(); break;
            case CloudState.Saving: UpdateSaving(); break;
        }
    }

    private void UpdateIdle()
    {
        TransitionToConnecting();
    }
    private void TransitionToConnecting()
    {
        state = CloudState.Connecting;
        timer = connectingTime;
        SelectNextRepo();
    }

    private void TransitionToDownloading()
    {
        state = CloudState.Downloading;
        timer = downloadTime;
        double unitsWithdrawn = consumptionRepos[currentRepoIndex].Withdraw(unitsDownloadedPerCycle);
        double unitsDeposited = myRepo.Deposit(unitsWithdrawn);
        myDataStream.SetupDataConection(consumptionRepos[currentRepoIndex].GetPosition(), DataFlow.Download);
    }

    private void UpdateDownloading()
    {
        if (!IsStateTimerElapsed())
        {
            myDataStream.AnimateDataConnection(timer / downloadTime);
        }
        else
        {
            if (myRepo.IsFull())
            {
                TransitionToSaving();
            }
            else if (!consumptionRepos[currentRepoIndex].IsEmpty())
            {
                TransitionToConnecting();
            }
            else
            {
                TransitionToDownloading();
            }
        }
    }

    private void UpdateConnecting()
    {
        if (IsStateTimerElapsed())
        {
            TransitionToDownloading();
        }
    }

    private void TransitionToSaving()
    {
        state = CloudState.Saving;
        timer = saveTime;
        double unitsWithdrawn = myRepo.Withdraw(unitsSavedPerCycle);
        double unitsDeposited = productionRepo.Deposit(unitsWithdrawn);
        myDataStream.SetupDataConection(productionRepo.GetPosition(), DataFlow.Upload);
        currentRepoIndex = 0;
    }

    private void UpdateSaving()
    {
        if (!IsStateTimerElapsed())
        {
            myDataStream.AnimateDataConnection(timer / saveTime);
        }
        else
        {
            if (myRepo.IsEmpty())
            {
                TransitionToConnecting();
            }
            else
            {
                TransitionToSaving();
            }
        }
    }

    private void SelectNextRepo()
    {
        currentRepoIndex++;
        if (currentRepoIndex > consumptionRepos.Count - 1) currentRepoIndex = 0;
    }

    private bool IsStateTimerElapsed()
    {
        timer -= Time.deltaTime;
        return timer <= 0 ? true : false;
    }
}
