using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveGame {

    public List<string> attributeNames;
    public List<double> numericValaues;
    public List<string> savedComponents;
    public DateTime saveTime;

    public SaveGame()
    {
        attributeNames = new List<string>();
        numericValaues = new List<double>();
        savedComponents = new List<string>();
        saveTime = DateTime.Now;
    }
}
