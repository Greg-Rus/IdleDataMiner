using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionModelMineShaft : MonoBehaviour, IMinerModel
{
    public int currentLevel = 1;
    public int maxLevel = 100;

    public int depth = 1;

    public double baseUnitsMinedPerCycle = 10d;
    public double currentUnitsMinedPerCycle = 10d;

    public double baseUnitsUploadedPerCycle = 50d;
    public double currentUnitsUploadedPerCycle = 50d;

    public float baseConsumptionCycleTime = 1f;
    public float baseProductionCycleTime = 1f;

    public double baseWorkerRepoMaxCapacity = 35d;
    public double currentWorkerRepoMaxCapacity = 35d;

    public int currentNumberOfWorkers = 1;
    public int maxnumberOfWorkers = 3;

    public float consumptionCycleTime = 1;
    public float productionCycleTime = 1;

    public double productionRepoMaxCapacity = Mathf.Infinity;
    public double consumptionRepoMaxCapacity = Mathf.Infinity;

    public double GetUnitsMinedPerCycleAtLevel(int newLevel)
    {
        return baseUnitsMinedPerCycle * 10d * newLevel * depth;
    }
    public double GetUnitsUploadedPerCycleAtLevel(int newLevel)
    {
        return baseUnitsUploadedPerCycle * 10d * newLevel * depth;
    }
    public double GetWorkerRepoCapacityAtLevel(int newLevel)
    {
        return baseWorkerRepoMaxCapacity * 10d * newLevel * depth;
    }
    public int GetNumberOfWorkersAtLevel(int newLevel)
    {
        return (newLevel / 50) + 1;
    }

    public void ScaleToLevel(int newLevel)
    {
        currentLevel = newLevel;
        currentUnitsMinedPerCycle = GetUnitsMinedPerCycleAtLevel(newLevel);
        currentUnitsUploadedPerCycle = GetUnitsUploadedPerCycleAtLevel(newLevel);
        currentNumberOfWorkers = GetNumberOfWorkersAtLevel(newLevel);
        currentWorkerRepoMaxCapacity = GetWorkerRepoCapacityAtLevel(newLevel);
    }

    public double GetUnitsMinedPerSecond()
    {
        return currentUnitsMinedPerCycle;
    }

    public double GetUnitsUploadedPerSecond()
    {
        return currentUnitsUploadedPerCycle;
    }

    public float GetConsumptionCycletime()
    {
        return consumptionCycleTime;
    }

    public float GetProductionCycletime()
    {
        return productionCycleTime;
    }

    public double GetCapacity()
    {
        return currentWorkerRepoMaxCapacity;
    }

    public double GetProductionPerSecond()
    {
        return currentUnitsMinedPerCycle / (consumptionCycleTime + productionCycleTime);    //Simplified calculation.
    }

}
