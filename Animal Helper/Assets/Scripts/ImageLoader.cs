using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    static ImageLoader _instance;
    public static ImageLoader Instance
    {
        get
        {
            return _instance;
        }
    }

    Dictionary<string, Sprite> images;
    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    public void Init()
    {
        images = new Dictionary<string, Sprite>();

        Sprite[] texResources = Resources.LoadAll<Sprite>("Icon");
        foreach (Sprite tex in texResources)
        {
            Debug.Log(tex.name);
            images[tex.name] = tex;
        }
    }

    public bool TryGetImage(string key, out Sprite sprite)
    {
        Sprite result = null;
        if (images.TryGetValue(key, out result) == false)
        {
            Debug.LogError("NO Image: " + key);
            sprite = result;
            return false;
        }
        sprite = result;
        return true;
    }

}
