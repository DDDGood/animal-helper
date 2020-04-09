using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{

    public static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("DataManager");
                _instance = go.AddComponent<DataManager>();
                _instance.Init();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }




    public void Init()
    {
        fishText = Resources.Load<TextAsset>("Data/acfish");
        bugText = Resources.Load<TextAsset>("Data/acbug");
        animalText = Resources.Load<TextAsset>("Data/acanimal");
    }

    TextAsset fishText;
    TextAsset bugText;
    TextAsset animalText;

    ACDex dex = new ACDex();
    public ACDex Dex { get { return dex; } }



    public void LoadFish()
    {
        dex.fish = JsonConvert.DeserializeObject<Dictionary<string, ACFishData>>(fishText.text);
    }

    public void LoadBug()
    {
        dex.bug = JsonConvert.DeserializeObject<Dictionary<string, ACBugData>>(bugText.text);
    }

    public void LoadAnimal()
    {
        dex.animal = JsonConvert.DeserializeObject<Dictionary<string, ACAnimalData>>(animalText.text);
    }




}
