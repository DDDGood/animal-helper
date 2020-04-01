using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIFishMenu : UIDexMenu
{


    public override void Init()
    {
        InitFishGO();
        ScrollUpdate();
    }



    void InitFishGO()
    {
        List<KeyValuePair<string, ACFishData>> fishList = Dex.fish.ToList();
        var sortedList = from fish in fishList orderby fish.Value.location select fish;

        foreach (KeyValuePair<string, ACFishData> fishData in sortedList)
        {
            GameObject fishGO = Instantiate(itemPrefab);
            fishGO.transform.SetParent(rootTransform, false);
            fishGO.SetActive(true);

            UIDexItem item = fishGO.GetComponent<UIFish>();
            item.Init(fishData.Key, fishData.Value);

            items[fishData.Key] = item;
        }
    }


    public override void ShowMonth(int month)
    {
        int index = month - 1;
        if (index < 0)
        {
            ShowAll();
            return;
        }

        foreach (KeyValuePair<string, ACFishData> fishData in Dex.fish)
        {
            int[] monthsData = Main.Instance.GeoMode == GeographyMode.NORTH ? fishData.Value.monthsNorth : fishData.Value.monthsSouth;

            if (monthsData[index] == 1)
            {
                items[fishData.Key].gameObject.SetActive(true);
            }
            else
            {
                items[fishData.Key].gameObject.SetActive(false);
            }
        }

        ScrollUpdate();
    }



}
