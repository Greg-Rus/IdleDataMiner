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

public interface IWithdrawing //TODO test if good idea for pipeline management.
{
    double Withdraw(double amount);
    Vector3 GetPosition();
    bool IsFull();
    bool IsEmpty();
}

public interface IDepositing
{
    double Deposit(double amount);
    Vector3 GetPosition();
    bool IsFull();
    bool IsEmpty();
}
