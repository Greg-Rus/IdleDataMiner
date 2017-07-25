using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repo : MonoBehaviour {
    public double maxCapacity = 100d;
    public double currentLoad = 0d;
	// Use this for initialization
	void Start () {
		
	}

    public double Deposit(double amount)
    {
        if (gameObject.name == "ShaftRepo")
        {
            Debug.Log("Shaft got: " + amount);
        }
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
}
