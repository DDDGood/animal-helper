using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimalFilter : MonoBehaviour
{

    public GameObject buttonPrefab;
    public Transform sexRoot;
    public Transform specieRoot;
    public Transform personalityRoot;

    Button currentSpecieBtn;
    Button currentPersonalityBtn;

    public Color btnHightlightColor;
    public Color btnNormalColor;



    string[] sexTags = new string[2] { "male", "female" };
    string[] specieTags = new string[] { "狗","貓","青蛙","食蟻獸","猩猩","狼","松鼠","雞","老鷹","豬","馬","章魚",
                                    "鹿","獅子","老虎","鳥","老鼠","牛","小熊","大熊","鱷魚","奶牛","綿羊","山羊",
                                    "鴨","猴子","袋鼠","象","犀牛","無尾熊","鴕鳥","兔子","企鵝","河馬","倉鼠"};
    string[] personalityTags = new string[] { "元氣", "普通", "成熟", "大姐姐", "自戀", "運動", "悠閑", "暴躁" };


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitAnimal()
    {
        foreach (string specie in specieTags)
        {
            GameObject item = Instantiate(buttonPrefab);
            item.transform.SetParent(specieRoot, false);
            item.SetActive(true);
            Button btn = item.GetComponent<Button>();
            btn.GetComponent<Image>().color = btnNormalColor;
            btn.GetComponentInChildren<Text>().text = specie;
            btn.onClick.AddListener(() =>
            {
                MainAnimal.Instance.SelectAnimal(specie);


                if (currentSpecieBtn == btn)
                {
                    currentSpecieBtn.GetComponent<Image>().color = btnNormalColor;
                    currentSpecieBtn = null;
                }
                else
                {
                    if (currentSpecieBtn != null)
                    {
                        currentSpecieBtn.GetComponent<Image>().color = btnNormalColor;
                    }

                    currentSpecieBtn = btn;
                    currentSpecieBtn.GetComponent<Image>().color = btnHightlightColor;
                }

                gameObject.SetActive(false);
            });
        }
    }

    public void InitPersonalities()
    {
        foreach (string personality in personalityTags)
        {
            GameObject item = Instantiate(buttonPrefab);
            item.transform.SetParent(personalityRoot, false);
            item.SetActive(true);
            Button btn = item.GetComponent<Button>();
            btn.GetComponent<Image>().color = btnNormalColor;
            btn.GetComponentInChildren<Text>().text = personality;
            btn.onClick.AddListener(() =>
                      {
                          //   MainAnimal.Instance.SelectAnimal(personality);


                          if (currentPersonalityBtn == btn)
                          {
                              currentPersonalityBtn.GetComponent<Image>().color = btnNormalColor;
                              currentPersonalityBtn = null;
                          }
                          else
                          {
                              if (currentPersonalityBtn != null)
                              {
                                  currentPersonalityBtn.GetComponent<Image>().color = btnNormalColor;
                              }

                              currentPersonalityBtn = btn;
                              currentPersonalityBtn.GetComponent<Image>().color = btnHightlightColor;
                          }

                          gameObject.SetActive(false);
                      });

        }
    }




}
