using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProgressionModelMineShaft))]
[RequireComponent(typeof(StructureView))]
public class MineShaft : MonoBehaviour, IWithdrawing , IDepositing{
    public Repo myConsumptionRepo;
    public Repo myRepo;
    public Miner[] myWorkers; //TODO list
    //public Repo[] workerRepos;
    private int lastActiveWorkerIndex = 0;
    private ProgressionModelMineShaft myModel;
    private StructureView myView;
	// Use this for initialization
	void Awake () {
        myModel = GetComponent<ProgressionModelMineShaft>();
        myView = GetComponent<StructureView>();
        ConfigureRepos();
        ConfigureMiners();
        UpgradeToLevel(1);
        RestoreState();
    }

    private void ConfigureRepos()
    {
        myConsumptionRepo.maxCapacity = Mathf.Infinity;
        myConsumptionRepo.currentLoad = Mathf.Infinity;
        myRepo.maxCapacity = Mathf.Infinity;
    }
    private void ConfigureMiners()
    {
        for (int i = 0; i < myWorkers.Length; i++)
        {
            myWorkers[i].myModel = myModel as IMinerModel;
            myWorkers[i].myConsumptionRepo = myConsumptionRepo as IWithdrawing;
            myWorkers[i].myProductionRepo = this as IDepositing;
            myWorkers[i].myRepo.maxCapacity = myModel.currentWorkerRepoMaxCapacity;
        }
    }

//  Upgrade Mechanic methods
    public void OnUpgrade()
    {
        UpgradeToLevel(myModel.currentLevel + 1);
    }

    private void UpgradeToLevel(int newLevel)
    {
        myModel.ScaleToLevel(newLevel);
        if (ActiveWorkerCount() < myModel.currentNumberOfWorkers) ActivateNewWorker();
        UpdateWorkers();
        myView.UpdateLevel(myModel.currentLevel);
    }
    public void GenerateUnitsForIdleTime(float seconds)
    {
        Debug.Log("Idle profit for: " + gameObject.name + " " + myModel.GetProductionPerSecond() * seconds);
        Deposit(myModel.GetProductionPerSecond() * seconds);
    }

    private void ActivateNewWorker()
    {
        lastActiveWorkerIndex++;
        myWorkers[lastActiveWorkerIndex].gameObject.SetActive(true);
    }
    private int ActiveWorkerCount()
    {
        return lastActiveWorkerIndex + 1;
    }
    private void UpdateWorkers()
    {
        for (int i = 0; i < myWorkers.Length; i++)
        {
            myWorkers[i].myRepo.maxCapacity = myModel.currentWorkerRepoMaxCapacity;
        }
    }

    //Expose myRepo inteface so that other structurse can use this structures as a Repo.
    public double Withdraw(double amount)
    {
        double withdrawnAmount = myRepo.Withdraw(amount);
        myView.UpdateRepoLoad(myRepo.currentLoad);
        return withdrawnAmount;
    }
    public double Deposit(double amount)
    {
        double depositedAmount = myRepo.Deposit(amount);
        myView.UpdateRepoLoad(myRepo.currentLoad);
        return depositedAmount;
    }
    public Vector3 GetPosition()
    {
        return myRepo.GetPosition();
    }
    public bool IsFull()
    {
        return myRepo.IsFull();
    }
    public bool IsEmpty()
    {
        return myRepo.IsEmpty();
    }

    private void RestoreState()
    {
        if (GameSaver.instance.saveStateExists)
        {
            int oldLevel = GameSaver.instance.RestoreInt(gameObject.name + "level");
            UpgradeToLevel(oldLevel);
            Deposit(GameSaver.instance.RestoreDouble(gameObject.name + "repo"));
        }
    }
    private void SaveState()
    {
        GameSaver.instance.StoreInt(gameObject.name + "level", myModel.currentLevel);
        GameSaver.instance.StoreDouble(gameObject.name + "repo", myRepo.currentLoad);
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }
}
