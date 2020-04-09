using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPossibility : MonoBehaviour
{
    public Text description;
    public Text[] priceTexts;

    Possibility possibility;

    public void Init(Possibility possibility)
    {
        this.possibility = possibility;
        description.text = possibility.description;
        for (int i = 0; i < priceTexts.Length; i++)
        {
            string txt;
            PossiblePrice posPrice = possibility.prices[i + 2];
            if (posPrice.min == posPrice.max)
                txt = posPrice.min.ToString();
            else
                txt = posPrice.min + "~" + posPrice.max;
            priceTexts[i].text = txt;
        }
    }
}
