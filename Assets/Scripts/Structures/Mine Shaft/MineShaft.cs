using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProgressionModelMineShaft))]
[RequireComponent(typeof(StructureView))]
public class MineShaft : MonoBehaviour, IWithdrawing , IDepositing{
    public Repo myConsumptionRepo;      //Separate GameObject with a tranform used for visual effects. Set in inspector.
    public Repo myRepo;                 //Separate GameObject with a tranform used for visual effects. Set in inspector.
    public Miner[] myWorkers;           //Separate GameObject with a tranform used for visual effects. Set in inspector.
    private int lastActiveWorkerIndex = 0;
    private ProgressionModelMineShaft myModel;
    private StructureView myView;

	void Awake () {
        myModel = GetComponent<ProgressionModelMineShaft>();
        myView = GetComponent<StructureView>();
        ConfigureRepos();
        ConfigureMiners();
        UpgradeToLevel(1);
        RestoreState();
    }

    //Generator strucutre. Has infinite resources mined over time.
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

    //Structure upgrade mechanic
    public void OnUpgrade()
    {
        UpgradeToLevel(myModel.currentLevel + 1);
    }
    private void UpgradeToLevel(int newLevel)
    {
        myModel.ScaleToLevel(newLevel);
        if (ActiveWorkerCount() < myModel.currentNumberOfWorkers) ActivateNewWorker();
        UpgradesWorkers();
        myView.UpdateLevel(myModel.currentLevel);
    }

    public void GenerateUnitsForIdleTime(double seconds)
    {
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

    //Propagates upgraded state to workers: bigger repo capacity
    private void UpgradesWorkers()
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

    //Upgrade the structure to the saved level. Restore repo load.
    private void RestoreState()
    {
        if (GameSaver.instance.saveStateExists && GameSaver.instance.CheckIfSaveExists(gameObject.name))
        {
            int oldLevel = GameSaver.instance.RestoreInt(gameObject.name + "level");
            UpgradeToLevel(oldLevel);
            Deposit(GameSaver.instance.RestoreDouble(gameObject.name + "repo"));
        }
    }

    private void SaveState()
    {
        GameSaver.instance.RegisterSave(gameObject.name);
        GameSaver.instance.StoreInt(gameObject.name + "level", myModel.currentLevel);
        GameSaver.instance.StoreDouble(gameObject.name + "repo", myRepo.currentLoad);
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }
}
