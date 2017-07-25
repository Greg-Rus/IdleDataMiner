using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineShaft : MonoBehaviour {
    public Repo myConsumptionRepo;
    public Repo myProductionRepo;
    public Miner[] myWorkers; //TODO list
    public int level = 1;
    public ProgressionModelMineShaft myModel;
	// Use this for initialization
	void Start () {
        myConsumptionRepo.maxCapacity = Mathf.Infinity;
        myConsumptionRepo.currentLoad = Mathf.Infinity;
        for (int i = 0; i < myWorkers.Length; i++)
        {
            myWorkers[i].myConsumptionRepo = myConsumptionRepo;
            myWorkers[i].myProductionRepo = myProductionRepo;
            myWorkers[i].myModel = myModel as IMinerModel;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnUpdate()
    {

    }
}
