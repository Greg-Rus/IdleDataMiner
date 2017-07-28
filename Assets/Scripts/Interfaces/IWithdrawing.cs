using UnityEngine;
public interface IWithdrawing
{
    double Withdraw(double amount);
    Vector3 GetPosition();
    bool IsFull();
    bool IsEmpty();
}
