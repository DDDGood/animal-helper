using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageLoader
{
    static ImageLoader _instance;
    public static ImageLoader Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ImageLoader();

            return _instance;
        }
    }

    Dictionary<string, Sprite> images;
    public ImageLoader()
    {
        Init();
    }

    // Start is called before the first frame update
    public void Init()
    {
        images = new Dictionary<string, Sprite>();

        LoadAllImages();
    }

    public void LoadAllImages()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Icon");
        foreach (Sprite sprite in sprites)
        {
            Debug.Log(sprite.name);
            images[sprite.name] = sprite;
        }
    }

    public void LoadImagesFromPath(string path)
    {

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
