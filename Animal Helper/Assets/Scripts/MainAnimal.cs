using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainAnimal : MonoBehaviour
{

    static MainAnimal _instance;
    public static MainAnimal Instance { get { return _instance; } }

    void Awake()
    {
        _instance = this;
    }

    protected ACDex Dex { get { return DataManager.Instance.Dex; } }
    public Transform rootTransform;
    public GameObject itemPrefab;

    public UIAnimalFilter uIAnimalFilter;

    Dictionary<string, UIAnimal> animalItems = new Dictionary<string, UIAnimal>();


    string currentAnimal;

    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.LoadAnimal();
        InitGO();
        ScrollUpdate();

        uIAnimalFilter.InitAnimal();
    }

    void InitGO()
    {
        foreach (KeyValuePair<string, ACAnimalData> animalData in Dex.animal)
        {
            GameObject animalGO = Instantiate(itemPrefab);
            animalGO.transform.SetParent(rootTransform, false);
            animalGO.SetActive(true);

            UIAnimal item = animalGO.GetComponent<UIAnimal>();
            item.Init(animalData.Key, animalData.Value);

            animalItems[animalData.Key] = item;
        }
    }

    public void ScrollUpdate()
    {
        int count = 0;
        foreach (UIAnimal item in animalItems.Values)
        {
            if (item.gameObject.activeSelf)
                count += 1;
        }
        RectTransform rt = (RectTransform)rootTransform.transform;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, count * 200);
    }

    public void SelectAnimal(string key)
    {
        if (currentAnimal == key)
        {
            CancelSelect();
        }
        else
        {
            foreach (UIAnimal animal in animalItems.Values)
            {
                if (string.Equals(animal.Data.specie, key))
                    animal.gameObject.SetActive(true);
                else
                    animal.gameObject.SetActive(false);
            }
            currentAnimal = key;
            ScrollUpdate();
        }
    }

    void CancelSelect()
    {
        currentAnimal = "";
        foreach (UIAnimal animal in animalItems.Values)
        {
            animal.gameObject.SetActive(true);
        }
        ScrollUpdate();
    }


    public void OpenFilter()
    {
        uIAnimalFilter.gameObject.SetActive(true);
    }
    public void CloseFilter()
    {
        uIAnimalFilter.gameObject.SetActive(false);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
