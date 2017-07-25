using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionModelMineShaft : ScriptableObject {

    public int currentLevel = 1;
    public int maxLevel = 100;

    public int depth = 1;

    public double baseUnitsMinedPerCycle = 10d;
    public double currentUnitsMinedPerCycle = 10d;

    public double baseUnitsUploadedPerCycle = 50d;
    public double currnetUnitsUploadedPerCycle = 50d;

    public float baseConsumptionCycleTime = 1f;
    public float baseProductionCycleTime = 1f;

    public double baseProductionRepoMaxCapacity = 230d;
    public double currentProductionRepoMaxCapacity = 230d;

    public int currentNumberOfWorkers = 1;
    public int maxnumberOfWorkers = 3;

    public double GetUnitsMinedPerCycleAtLevel(int newLevel)
    {
        return baseUnitsMinedPerCycle * 10d * newLevel * depth;
    }
    public double GetUnitsUploadedPerCycleAtLevel(int newLevel)
    {
        return baseUnitsUploadedPerCycle * 10d * newLevel * depth;
    }
    public double GetProductionRepoCapacityAtLevel(int newLevel)
    {
        return baseProductionRepoMaxCapacity * 10d * newLevel * depth;
    }
    public int GetNumberOfWorkersAtLevel(int newLevel)
    {
        return (newLevel / 50) + 1;
    }

    public void ScaleToLevel(int newLevel)
    {
        currentLevel = newLevel;
        currentUnitsMinedPerCycle = GetUnitsMinedPerCycleAtLevel(newLevel);
        currentProductionRepoMaxCapacity = GetProductionRepoCapacityAtLevel(newLevel);
        currnetUnitsUploadedPerCycle = GetUnitsUploadedPerCycleAtLevel(newLevel);
        currentNumberOfWorkers = GetNumberOfWorkersAtLevel(newLevel);
    }

}
