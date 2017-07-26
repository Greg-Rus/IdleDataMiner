using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineShaftView : MonoBehaviour, IMineShaftUI
{
    public Text repoLoadDisplay;

    public void UpdateRepoLoad(double load)
    {
        repoLoadDisplay.text = load.ToString();
    }

}
