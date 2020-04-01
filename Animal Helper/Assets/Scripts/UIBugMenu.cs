using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIBugMenu : UIDexMenu
{
    public override void Init()
    {
        InitBugGO();
        ScrollUpdate();
    }



    void InitBugGO()
    {
        List<KeyValuePair<string, ACBugData>> bugList = Dex.bug.ToList();
        var sortedList = from bug in bugList orderby bug.Value.location select bug;

        foreach (KeyValuePair<string, ACBugData> bugData in Dex.bug)
        {
            GameObject bugGO = Instantiate(itemPrefab);
            bugGO.transform.SetParent(rootTransform, false);
            bugGO.SetActive(true);

            UIDexItem item = bugGO.GetComponent<UIBug>();
            item.Init(bugData.Key, bugData.Value);

            items[bugData.Key] = item;
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

        foreach (KeyValuePair<string, ACBugData> fishData in Dex.bug)
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
