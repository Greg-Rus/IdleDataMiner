using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PipelineController : MonoBehaviour {

    public MineShaft[] mineShafts;          //Generate units.
    public CloudController cloud;           //Tranferes units.
    public ManagerController manager;       //Banks units.
    private int lastActiveShaftIndex = 0;
    public Transform ShaftBuyCanvasRoot;

    void Start() {
        SetupPipeline();
        RestoreState();
    }

    //Connect repos to form a pipeline.
    private void SetupPipeline()
    {
        cloud.consumptionRepos.Add(mineShafts[lastActiveShaftIndex]); //through IWithdrawing
        cloud.productionRepo = manager; //through IDepositing
    }

    private void RestoreState()
    {
        if (GameSaver.instance.saveStateExists && GameSaver.instance.CheckIfSaveExists(gameObject.name))
        {
            while (GameSaver.instance.RestoreInt("lastActiveShaftIndex") != lastActiveShaftIndex)
            {
                OnBuyNewShaft();
            }
            UpdateIdleProfits();
        }
    }

    private void SaveState()
    {
        GameSaver.instance.RegisterSave(gameObject.name);
        GameSaver.instance.StoreInt("lastActiveShaftIndex", lastActiveShaftIndex);
    }

    public void OnBuyNewShaft()
    {
        ActivateNewShaft();
        if (lastActiveShaftIndex == mineShafts.Length - 1)    //No more shafts to buy. Disable the button.
        {
            ShaftBuyCanvasRoot.gameObject.SetActive(false);
        }
        else
        {
            MoveShaftBuyButtonToNextLocation();             //Move button to the location of the next buyable shaft.
        }
    }
    private void ActivateNewShaft()
    {
        lastActiveShaftIndex++;
        mineShafts[lastActiveShaftIndex].gameObject.SetActive(true);
        cloud.consumptionRepos.Add(mineShafts[lastActiveShaftIndex]);
    }
    private void MoveShaftBuyButtonToNextLocation()
    {
        ShaftBuyCanvasRoot.position = mineShafts[lastActiveShaftIndex + 1].transform.position;
    }

    private void UpdateIdleProfits()
    {
        TimeSpan timeSinceLastGame = DateTime.Now - GameSaver.instance.GetTimeSinceLastGame();
        double secondsSinceLastGame = Math.Round(timeSinceLastGame.TotalSeconds);
        if (secondsSinceLastGame > 1)
        {
            for (int i = 0; i <= lastActiveShaftIndex; i++)                     //Tell shafts to update idle production
            {
                mineShafts[i].GenerateUnitsForIdleTime(secondsSinceLastGame);
            }
            cloud.GenerateUnitsForIdleTime(secondsSinceLastGame);               //Tell could to update idle production
        }
        
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }
}
