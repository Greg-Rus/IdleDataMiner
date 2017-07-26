using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineController : MonoBehaviour {

    public GameObject ShaftPrefab;
    public MineShaft[] mineShafts;
    private int lastActiveShaftIndex = 0;
    public Transform ShaftBuyCanvasRoot;

	// Use this for initialization
	void Start () {

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
    }
    private void MoveShaftBuyButtonToNextLocation()
    {
        ShaftBuyCanvasRoot. position = mineShafts[lastActiveShaftIndex + 1].transform.position;
    }
}
