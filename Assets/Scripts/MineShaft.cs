using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProgressionModelMineShaft))]
[RequireComponent(typeof(StructureView))]
public class MineShaft : MonoBehaviour, IRepoUseage {
    public Repo myConsumptionRepo;
    public Repo myProductionRepo;
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
    }

    private void ConfigureRepos()
    {
        myConsumptionRepo.maxCapacity = Mathf.Infinity;
        myConsumptionRepo.currentLoad = Mathf.Infinity;
        myProductionRepo.maxCapacity = Mathf.Infinity;
    }
    private void ConfigureMiners()
    {
        for (int i = 0; i < myWorkers.Length; i++)
        {
            myWorkers[i].myModel = myModel as IMinerModel;
            myWorkers[i].shaftRepos = this as IRepoUseage;
            myWorkers[i].myRepo.maxCapacity = myModel.currentWorkerRepoMaxCapacity;
        }
    }

//  Upgrade Mechanic methods
    public void OnUpgrade()
    {
        myModel.ScaleToLevel(myModel.currentLevel + 1);
        if (ActiveWorkerCount() < myModel.currentNumberOfWorkers) ActivateNewWorker();
        UpdateWorkers();
        myView.UpdateLevel(myModel.currentLevel);
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

//  IRepoUseage methods
    public double Consume(double amount)
    {
        return myConsumptionRepo.Withdraw(amount);
    }
    public double Produce(double amount)
    {
        double depositedAmount = myProductionRepo.Deposit(amount);
        myView.UpdateRepoLoad(myProductionRepo.currentLoad);
        return depositedAmount; //TODO check if needed.
    }
    public Vector3 GetConsumptionRepoLocation()
    {
        return myConsumptionRepo.transform.position;
    }
    public Vector3 GetProductionRepoLocation()
    {
        return myProductionRepo.transform.position;
    }


}
