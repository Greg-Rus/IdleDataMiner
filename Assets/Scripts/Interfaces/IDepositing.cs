using UnityEngine;

public interface IDepositing
{
    double Deposit(double amount);
    Vector3 GetPosition();
    bool IsFull();
    bool IsEmpty();
}