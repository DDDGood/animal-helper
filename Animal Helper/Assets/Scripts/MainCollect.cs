using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GeographyMode
{
    NORTH, SOUTH
}
public class MainCollect : MonoBehaviour
{
    static MainCollect _instance;
    public static MainCollect Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }


    public TextAsset fishText;
    public TextAsset bugText;

    // public ACDex dex;


    public List<UIDexMenu> dexMenus;
    int currentMenuID = 0;


    GeographyMode geoMode;
    public GeographyMode GeoMode { get { return geoMode; } }
    int currentMonth = 0;

    public List<Button> btnDexMenus;
    public List<Button> btnGeoModes;
    public List<Button> btnMonths;
    public Color monthHightlightColor;
    public Color monthNormalColor;


    void Start()
    {
        // dex.fish = JsonConvert.DeserializeObject<Dictionary<string, ACFishData>>(fishText.text);
        // dex.bug = JsonConvert.DeserializeObject<Dictionary<string, ACBugData>>(bugText.text);

        // ImageLoader.Instance.Init();

        DataManager.Instance.LoadFish();
        DataManager.Instance.LoadBug();

        foreach (UIDexMenu menu in dexMenus)
        {
            menu.Init();
        }

        ChangeMode(0);
        ChangeGeoMode(0);
    }


    public void ChangeMode(int nextMenuID)
    {
        btnDexMenus[currentMenuID].GetComponent<Image>().color = monthNormalColor;
        btnDexMenus[nextMenuID].GetComponent<Image>().color = monthHightlightColor;

        if (nextMenuID == currentMenuID)
            return;


        dexMenus[currentMenuID].Close();
        dexMenus[nextMenuID].Open();
        dexMenus[nextMenuID].ShowMonth(currentMonth);

        currentMenuID = nextMenuID;

    }

    public void ChangeGeoMode(int modeID)
    {
        GeographyMode nextMode = (GeographyMode)modeID;

        btnGeoModes[(int)geoMode].GetComponent<Image>().color = monthNormalColor;
        btnGeoModes[modeID].GetComponent<Image>().color = monthHightlightColor;

        if (geoMode == nextMode)
            return;



        foreach (UIDexMenu menu in dexMenus)
        {
            menu.ChangeGeoMode(nextMode);
        }
        geoMode = nextMode;


        dexMenus[currentMenuID].ShowMonth(currentMonth);
    }

    public void SelectMonth(int month)
    {
        if (month > 12)
            return;

        int currentIndex = currentMonth - 1;
        int index = month - 1;

        if (currentMonth == month)
        {
            btnMonths[index].GetComponent<Image>().color = monthNormalColor;
            CancelSelect();
        }
        else
        {
            Debug.Log("refresh month");


            dexMenus[currentMenuID].ShowMonth(month);

            if (currentMonth > 0)
                btnMonths[currentIndex].GetComponent<Image>().color = monthNormalColor;

            currentMonth = month;
            btnMonths[index].GetComponent<Image>().color = monthHightlightColor;
        }
    }


    void CancelSelect()
    {
        Debug.Log("cancel select");

        dexMenus[currentMenuID].ShowAll();

        currentMonth = 0;
    }


    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
