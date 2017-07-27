using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineController : MonoBehaviour {

    public MineShaft[] mineShafts;          //Generates units.
    public CloudController cloud;           //Tranferes units.
    public ManagerController manager;       //Banks units.
    private int lastActiveShaftIndex = 0;
    public Transform ShaftBuyCanvasRoot;

	// Use this for initialization
	void Start () {
        SetupPipeline();
    }

    private void SetupPipeline()
    {
        cloud.consumptionRepos.Add(mineShafts[lastActiveShaftIndex]);
        cloud.productionRepo = manager;
    }

    public void OnBuyNewShaft()
    {
        ActivateNewShaft();
        if (lastActiveShaftIndex == mineShafts.Length-1)
        {
            ShaftBuyCanvasRoot.gameObject.SetActive(false);
        }
        else
        {
            MoveShaftBuyButtonToNextLocation();
        }
    }
    private void ActivateNewShaft()
    {
        lastActiveShaftIndex++;
        mineShafts[lastActiveShaftIndex].gameObject.SetActive(true);
        mineShafts[lastActiveShaftIndex].OnUpgrade();
        cloud.consumptionRepos.Add(mineShafts[lastActiveShaftIndex]);
    }
    private void MoveShaftBuyButtonToNextLocation()
    {
        ShaftBuyCanvasRoot. position = mineShafts[lastActiveShaftIndex + 1].transform.position;
    }
}
