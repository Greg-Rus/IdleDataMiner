﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineShaftView : MonoBehaviour, IMineShaftUI
{
    public Text repoLoadDisplay;
    public Text shaftLevelDisplay; 

    public void UpdateRepoLoad(double load)
    {
        repoLoadDisplay.text = load.ToString();
    }

    public void UpdateLevel(int level)
    {
        shaftLevelDisplay.text = level.ToString();
    }
    

}
