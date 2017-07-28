using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionModelCloud : MonoBehaviour, ICloudModel {

    public int currentLevel;
    public int maxlevel;

    public double baseUnitsDownloadedPerCycle = 100d;
    public double currentUnitsDownloadedPerCycle = 100d;

    public double baseUnitsSavedPerCycle = 500;
    public double currentUnitsSavedPerCycle = 500;

    public double baseCloudMaxCapacity = 1000d;
    public double currentCloudMaxCapacity = 1000d;

    public float connectingTime = 0.5f;
    public float downloadTime = 1f;
    public float saveTime = 1f;

    public double GetUnitsDownloadedPerCycle()
    {
        return currentUnitsDownloadedPerCycle;
    }
    public double GetUnitsSavedPerCycle()
    {
        return currentUnitsSavedPerCycle;
    }
    public double GetCloudCapacity()
    {
        return currentCloudMaxCapacity;
    }
    public float GetConnectingTime()
    {
        return connectingTime;
    }
    public float GetDownloadTime()
    {
        return downloadTime;
    }
    public float GetSaveTime()
    {
        return saveTime;
    }

    public double GetUnitsDownloadedPerCycleAtLevel(int newLevel)
    {
        return baseUnitsDownloadedPerCycle * 5d * newLevel;
    }
    public double GetUnitsSavedPerCycleAtLevel(int newLevel)
    {
        return baseUnitsSavedPerCycle * 5d * newLevel;
    }
    public double GetCloudCapacityAtLevel(int newLevel)
    {
        return baseCloudMaxCapacity * 5d * newLevel;
    }

    public void ScaleTolevel(int newLevel)
    {
        currentLevel = newLevel;
        currentCloudMaxCapacity = GetCloudCapacityAtLevel(newLevel);
        currentUnitsDownloadedPerCycle = GetUnitsDownloadedPerCycleAtLevel(newLevel);
        currentUnitsSavedPerCycle = GetUnitsSavedPerCycleAtLevel(newLevel);
    }

    public double GetProductionPerSecond()
    {
        return currentUnitsSavedPerCycle / (downloadTime + saveTime);   //Simplified calculation. 
                                                                        //Should take into account connectiond delays and bottlenecks.
    }
}
