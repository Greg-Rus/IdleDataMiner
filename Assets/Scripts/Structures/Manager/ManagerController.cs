using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Repo))]
[RequireComponent(typeof(StructureView))]
public class ManagerController : MonoBehaviour , IDepositing{
    private Repo myRepo;
    private StructureView myView;

    void Start()
    {
        myRepo = GetComponent<Repo>();
        myRepo.maxCapacity = Mathf.Infinity;
        myView = GetComponent<StructureView>();
        RestoreState();
    }

    //Expose IDepositing
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
    public bool IsEmpty()
    {
        return myRepo.IsEmpty();
    }
    public bool IsFull()
    {
        return myRepo.IsFull();
    }

    private void RestoreState()
    {
        if (GameSaver.instance.saveStateExists)
        {
            myRepo.Deposit(GameSaver.instance.RestoreDouble(gameObject.name + "repo"));
            myView.UpdateRepoLoad(myRepo.currentLoad);
        }
    }
    private void SaveState()
    {
        GameSaver.instance.StoreDouble(gameObject.name + "repo", myRepo.currentLoad);
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }



}
