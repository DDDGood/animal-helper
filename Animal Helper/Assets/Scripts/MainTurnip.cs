using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainTurnip : MonoBehaviour
{

    public TurnipPredictor predictor;
    public GameObject possibilityPrefab;
    public Transform rootTransform;

    public InputField inputBuyPrice;
    public InputField[] inputPrices;

    public Text resultPatternDescription;

    Dictionary<int, string> resultPatterns = new Dictionary<int, string>();


    // Start is called before the first frame update
    void Start()
    {
        inputBuyPrice.onEndEdit.AddListener((text) => { RefreshResult(); });
        foreach (InputField input in inputPrices)
        {
            input.onEndEdit.AddListener((text) => { RefreshResult(); });
        }
        inputBuyPrice.text = PlayerPrefs.GetString("buyPrice");

        for (int i = 0; i < 12; i++)
        {
            inputPrices[i].text = PlayerPrefs.GetString(i.ToString());
        }
        RefreshResult();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshResult()
    {
        int buyPrice = 0;
        if (int.TryParse(inputBuyPrice.text, out buyPrice) == false)
        {
            return;
        }
        if (buyPrice < 90 || buyPrice > 110)
            return;

        int[] prices = new int[14];
        prices[0] = buyPrice;
        for (int i = 0; i < inputPrices.Length; i++)
        {
            int price;
            if (int.TryParse(inputPrices[i].text, out price) == false)
            {
                price = 0;
            }
            prices[i + 2] = price;
        }


        PlayerPrefs.SetString("buyPrice", inputBuyPrice.text);
        for (int i = 0; i < 12; i++)
        {
            PlayerPrefs.SetString(i.ToString(), inputPrices[i].text);
        }


        // while (rootTransform.childCount > 0)
        //     Destroy(rootTransform.GetChild(0).gameObject);
        for (int i = 0; i < rootTransform.childCount; i++)
        {
            GameObject.Destroy(rootTransform.GetChild(i).gameObject);
        }

        List<Possibility> results = predictor.AnalyzePossibilities(prices);

        resultPatterns.Clear();

        foreach (Possibility result in results)
        {
            GameObject go = Instantiate(possibilityPrefab);
            go.transform.SetParent(rootTransform, false);
            go.SetActive(true);
            go.GetComponent<UIPossibility>().Init(result);

            resultPatterns[result.pattern] = result.description;
        }

        resultPatternDescription.text = string.Join(", ", resultPatterns.Values);

        ScrollUpdate(results.Count);
    }


    public void ScrollUpdate(int count)
    {
        // foreach (UIAnimal item in animalItems.Values)
        // {
        //     if (item.gameObject.activeSelf)
        //         count += 1;
        // }
        RectTransform rt = (RectTransform)rootTransform.transform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, count * 120);
    }


    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }


    public void ClearAll()
    {
        inputBuyPrice.text = "";
        foreach (InputField priceText in inputPrices)
        {
            priceText.text = "";
        }

        PlayerPrefs.SetString("buyPrice", "");
        for (int i = 0; i < 12; i++)
        {
            PlayerPrefs.SetString(i.ToString(), "");
        }

        for (int i = 0; i < rootTransform.childCount; i++)
        {
            GameObject.Destroy(rootTransform.GetChild(i).gameObject);
        }

        resultPatternDescription.text = "";
    }



}
