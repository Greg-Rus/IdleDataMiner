using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveGame {

    public List<string> attributeNames;
    public List<double> numericValaues;
    public DateTime saveTime;

    public SaveGame()
    {
        attributeNames = new List<string>();
        numericValaues = new List<double>();
        saveTime = DateTime.Now;
    }
}
