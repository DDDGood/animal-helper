using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimal : MonoBehaviour
{

    public Image icon;
    public Text nameText;
    public Text sexText;
    public Text specieText;
    public Text personalityText;
    public Text catchpraseText;
    public Text birthdayText;

    protected ACAnimalData data;
    public ACAnimalData Data { get { return data; } }


    public void Init(string key, ACAnimalData data)
    {
        this.data = data;

        nameText.text = data.name;
        sexText.text = data.sex == "male" ? "男" : "女";
        personalityText.text = data.personality;
        specieText.text = data.specie;
        birthdayText.text = data.birthday;
        catchpraseText.text = data.catchprase;

        Sprite sprite = null;
        if (ImageLoader.Instance.TryGetImage("i_" + key, out sprite))
        {
            icon.sprite = sprite;
        }
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
