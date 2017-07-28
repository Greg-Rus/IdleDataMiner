using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureView : MonoBehaviour {
    public Text repoLoadDisplay;
    public Text structureLevelDisplay;

    public void UpdateRepoLoad(double load)
    {
        repoLoadDisplay.text = load.ToString();
    }

    public void UpdateLevel(int level)
    {
        structureLevelDisplay.text = level.ToString();
    }
}
