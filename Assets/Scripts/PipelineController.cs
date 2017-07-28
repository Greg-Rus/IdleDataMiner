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
        cloud.consumptionRepos.Add(mineShafts[lastActiveShaftIndex]);
        cloud.productionRepo = manager;
    }

    private void RestoreState()
    {
        if (GameSaver.instance.saveStateExists)
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
        if (timeSinceLastGame.Seconds > 1)
        {
            for (int i = 0; i <= lastActiveShaftIndex; i++)
            {
                mineShafts[i].GenerateUnitsForIdleTime(timeSinceLastGame.Seconds);
            }
        }
        cloud.GenerateUnitsForIdleTime(timeSinceLastGame.Seconds);
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }
}
