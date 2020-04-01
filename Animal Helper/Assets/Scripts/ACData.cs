using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ACData
{
    public string name;
}


[System.Serializable]
public class ACDexItemData : ACData
{
    public int sellPrice;
    public string time;
    public string location;
    public int[] monthsNorth = new int[12];
    public int[] monthsSouth = new int[12];

}

[System.Serializable]
public class ACFishData : ACDexItemData
{
    public string shadow;

}


[System.Serializable]
public class ACBugData : ACDexItemData
{

}


[System.Serializable]
public class ACDex
{
    public Dictionary<string, ACFishData> fish = new Dictionary<string, ACFishData>();
    public Dictionary<string, ACBugData> bug = new Dictionary<string, ACBugData>();
    public Dictionary<string, ACAnimalData> animal = new Dictionary<string, ACAnimalData>();
}

public class ACAnimalData : ACData
{
    public string sex;
    public string personality;
    public string specie;
    public string birthday;
    public string catchprase;



}

