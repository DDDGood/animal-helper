using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIDexItem : MonoBehaviour
{

    public Image icon;
    public Text nameText;
    public Text locationText;
    public Text timeText;
    public List<Image> months;

    protected ACDexItemData data;

    public abstract void Init(string key, ACDexItemData data);
    public Color monthHighlightColor;
    public Color monthNormalColor;


    public void ChangeGeoMode(GeographyMode mode)
    {
        switch (mode)
        {
            case GeographyMode.NORTH:
                UpdateMonthColor(data.monthsNorth);
                break;
            case GeographyMode.SOUTH:
                UpdateMonthColor(data.monthsSouth);
                break;
        }

    }

    protected void UpdateMonthColor(int[] monthDatas)
    {

        for (int i = 0; i < monthDatas.Length; i++)
        {
            int canFind = monthDatas[i];
            if (canFind == 1)
                months[i].color = monthHighlightColor;
            else
                months[i].color = monthNormalColor;
        }
    }

}
