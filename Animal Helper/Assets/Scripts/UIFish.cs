using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFish : MonoBehaviour
{
    Main main;

    public Image icon;
    public Text nameText;
    public Text locationText;
    public Text timeText;


    public void Init(Main main, ACFishData data)
    {
        this.main = main;
        string name;
        if (main.localization.TryGetValue(data.name, out name) == false)
        {
            name = data.name;
        }

        nameText.text = name;

        locationText.text = data.location;

        timeText.text = data.time;

        Sprite sprite = null;
        if (ImageLoader.Instance.TryGetImage(data.name, out sprite))
        {
            icon.sprite = sprite;
        }
    }
}
