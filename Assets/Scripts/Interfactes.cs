using UnityEngine;

public interface IMinerModel
{
    double GetUnitsMinedPerSecond();
    double GetUnitsUploadedPerSecond();
    float GetConsumptionCycletime();
    float GetProductionCycletime();
}

public interface IMineShaftUI
{
    void UpdateRepoLoad(double load);
}

public interface IRepoUseage
{
    double Consume(double amount);
    double Produce(double amount);
    Vector3 GetConsumptionRepoLocation();
    Vector3 GetProductionRepoLocation();
}
