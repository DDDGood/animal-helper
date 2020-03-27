using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ACData
{
    public string name;
    public string image;
}

[System.Serializable]
public class ACFishData : ACData
{
    public int sellPrice;
    public string time;
    public string location;
    public int[] monthsNorth = new int[12];
    public int[] monthsSouth = new int[12];

}


[System.Serializable]
public class ACDex
{
    public Dictionary<string, ACFishData> fish = new Dictionary<string, ACFishData>();
}

