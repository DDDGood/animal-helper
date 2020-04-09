using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFish : UIDexItem
{
    public Text shadowText;

    public override void Init(string id, ACDexItemData oriData)
    {
        this.data = oriData;

        ACFishData data = (ACFishData)oriData;
        nameText.text = data.name;

        locationText.text = data.location;

        timeText.text = data.time;

        shadowText.text = data.shadow;


        Sprite sprite = null;
        if (ImageLoader.Instance.TryGetImage(id, out sprite))
        {
            icon.sprite = sprite;
        }

        ChangeGeoMode(MainCollect.Instance.GeoMode);
    }



}
