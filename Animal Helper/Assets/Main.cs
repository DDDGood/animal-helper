using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Main : MonoBehaviour
{
    public TextAsset dexText;
    public TextAsset localizationText;

    public ACDex dex;
    public Dictionary<string, string> localization;


    public Transform fishRoot;
    public GameObject fishPrefab;

    public Dictionary<string, GameObject> fishGOs = new Dictionary<string, GameObject>();

    bool onSelect = false;


    void Start()
    {
        ImageLoader.Instance.Init();


        dex.fish = JsonConvert.DeserializeObject<Dictionary<string, ACFishData>>(dexText.text);
        localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(localizationText.text);


        foreach (KeyValuePair<string, ACFishData> fishData in dex.fish)
        {
            GameObject fishGO = Instantiate(fishPrefab);
            fishGO.transform.SetParent(fishRoot, false);
            fishGO.SetActive(true);
            fishGO.GetComponent<UIFish>().Init(this, fishData.Value);

            fishGOs[fishData.Key] = fishGO;
        }

        ScrollUpdate();
    }

    public void ScrollUpdate()
    {
        int count = 0;
        foreach (GameObject fishGo in fishGOs.Values)
        {
            if (fishGo.activeSelf)
                count += 1;
        }
        RectTransform rt = (RectTransform)fishRoot.transform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, count * 100);
    }

    public void SelectMonth(int month)
    {
        if (month > 12)
            return;

        if (onSelect)
        {
            CancelSelect();
            return;
        }
        else
        {
            int index = month - 1;

            foreach (KeyValuePair<string, ACFishData> fishData in dex.fish)
            {
                if (fishData.Value.monthsNorth[index] == 1)
                {
                    fishGOs[fishData.Key].SetActive(true);
                }
                else
                {
                    fishGOs[fishData.Key].SetActive(false);
                }
            }
            onSelect = true;
        }

        ScrollUpdate();
    }

    public void CancelSelect()
    {
        foreach (GameObject fishGo in fishGOs.Values)
        {
            fishGo.SetActive(true);
        }
        onSelect = false;
    }

}
