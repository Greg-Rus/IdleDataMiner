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

public interface IConsumable //TODO test if good idea for pipeline management.
{
    double Consume(double amount);
    Vector3 GetRepoLocation();
    bool IsFull();
    bool IsEmpty();
}

public interface IProducable
{
    double Produce(double amount);
    Vector3 GetRepoLocation();
    bool IsFull();
    bool IsEmpty();
}
