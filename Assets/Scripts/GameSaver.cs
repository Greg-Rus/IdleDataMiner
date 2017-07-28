using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

public class GameSaver : MonoBehaviour {
    public static GameSaver instance;
    private SaveGame saveState;
    public bool saveStateExists;

	void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        LoadGameState();
    }

    public DateTime GetTimeSinceLastGame()
    {
        return saveState.saveTime;
    }

    public void StoreDouble(string name, double value)
    {
        int index = FindValueIndexForAttribute(name);
        if (index == -1)
        {
            saveState.attributeNames.Add(name);
            saveState.numericValaues.Add(value);
        }
        else
        {
            saveState.numericValaues[index] = value;
        }
    }
    public void StoreInt(string name, int value)
    {
        int index = FindValueIndexForAttribute(name);
        if (index == -1)
        {
            saveState.attributeNames.Add(name);
            saveState.numericValaues.Add((int)value);
        }
        else
        {
            saveState.numericValaues[index] = (int)value;
        }
    }
    public double RestoreDouble(string name)
    {
        int index = FindValueIndexForAttribute(name);
        if (index != -1)
        {
            return saveState.numericValaues[index];
        }
        else
        {
            return 0;
        }
    }
    public int RestoreInt(string name)
    {
        int index = FindValueIndexForAttribute(name);
        if (index != -1)
        {
            return (int)saveState.numericValaues[index];
        }
        else
        {
            LogMissingAttribute(name);
            return 0;
        }
    }

    private int FindValueIndexForAttribute(string name)
    {
        int index = saveState.attributeNames.IndexOf(name);

        return index;
    }

    private void LogMissingAttribute(string name)
    {
        Debug.LogError("Atribut with name: " + name + "not found in save file! Returning '0'");
    }

    public void LoadGameState()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            saveState = (SaveGame)bf.Deserialize(file);
            file.Close();
            saveStateExists = true;
        }
        else
        {
            saveState = new SaveGame();
            saveStateExists = false;
        }
    }

    private void OnDestroy()
    {
        SaveGameState();
    }

    public void SaveGameState()
    {
        saveState.saveTime = DateTime.Now;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, saveState);
        file.Close();
    }

}
