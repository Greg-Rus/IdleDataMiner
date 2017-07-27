using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repo : MonoBehaviour, IWithdrawing, IDepositing {
    public double maxCapacity = 100d;
    public double currentLoad = 0d;

    public double Deposit(double amount)
    {
        double newLoad = currentLoad + amount;
        if (currentLoad == maxCapacity)
        {
            return 0d;
        }
        else if (newLoad >= maxCapacity)
        {
            currentLoad = maxCapacity;
            return newLoad - maxCapacity;
        }
        else
        {
            currentLoad = newLoad;
            return amount;
        }
    }

    public double Withdraw(double amount)
    {
        double newLoad = currentLoad - amount;
        if (currentLoad == 0d)
        {
            return 0d;
        }
        else if (newLoad <= 0d)
        {
            currentLoad = 0d;
            return amount + newLoad;
        }
        else
        {
            currentLoad = newLoad;
            return amount;
        }
    }

    public bool IsFull()
    {
        return currentLoad == maxCapacity ? true : false;
    }
    public bool IsEmpty()
    {
        return currentLoad == 0d ? true : false;
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
