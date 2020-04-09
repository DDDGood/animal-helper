using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBug : UIDexItem
{
    public override void Init(string id, ACDexItemData oriData)
    {
        this.data = oriData;

        ACBugData data = (ACBugData)oriData;
        nameText.text = data.name;

        locationText.text = data.location;

        timeText.text = data.time;

        Sprite sprite = null;
        if (ImageLoader.Instance.TryGetImage(id, out sprite))
        {
            icon.sprite = sprite;
        }

        ChangeGeoMode(MainCollect.Instance.GeoMode);
    }


}
