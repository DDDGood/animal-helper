using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIDexMenu : MonoBehaviour
{

    protected ACDex Dex { get { return Main.Instance.dex; } }
    public Transform rootTransform;
    public GameObject itemPrefab;
    public Dictionary<string, UIDexItem> items = new Dictionary<string, UIDexItem>();


    public abstract void Init();
    public abstract void ShowMonth(int month);

    public void ScrollUpdate()
    {
        int count = 0;
        foreach (UIDexItem item in items.Values)
        {
            if (item.gameObject.activeSelf)
                count += 1;
        }
        RectTransform rt = (RectTransform)rootTransform.transform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, count * 100);
    }



    public void ShowAll()
    {
        foreach (UIDexItem item in items.Values)
        {
            item.gameObject.SetActive(true);
        }

        ScrollUpdate();
    }

    public void ChangeGeoMode(GeographyMode mode)
    {
        Debug.Log("c mode: " + mode);
        foreach (UIDexItem item in items.Values)
        {
            item.ChangeGeoMode(mode);
        }
    }


    public void Open()
    {
        rootTransform.gameObject.SetActive(true);
    }
    public void Close()
    {
        rootTransform.gameObject.SetActive(false);
    }



}
