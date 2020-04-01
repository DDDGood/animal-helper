using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using Newtonsoft.Json;

public class HtmlParser : MonoBehaviour
{
    public string xPath = "//*[@id='article-body']/div[5]";

    // Start is called before the first frame update
    void Start()
    {

        ParseAnimal();





        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17098");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17099");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17100");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17101");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17102");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17103");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17104");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17105");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17106");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17107");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17108");
        // ParseEXPGGICON("https://gamewith.net/animal-crossing-new-horizons/article/show/17109");
    }
    void ParseBug()
    {
        ACDex dex = new ACDex();
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17110", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17111", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17112", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17113", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17114", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17115", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17116", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17117", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17118", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17119", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17120", dex);
        ParseBugMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17121", dex);

        string dexJson = JsonConvert.SerializeObject(dex.bug);
        string path = "C:/Users/user/Downloads";
        File.WriteAllText(path + "/acbug.json", dexJson);
    }

    void ParseAnimal()
    {
        Dictionary<string, ACAnimalData> animalDex = new Dictionary<string, ACAnimalData>();

        WebClient client = new WebClient();
        MemoryStream ms = new MemoryStream(client.DownloadData(
            "https://wiki.biligame.com/dongsen/%E5%B0%8F%E5%8A%A8%E7%89%A9%E5%9B%BE%E9%89%B4"));

        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms);

        HtmlNode tableNode = doc.DocumentNode.SelectSingleNode("//table[@id = 'CardSelectTr']");
        HtmlNodeCollection items = tableNode.SelectNodes(".//tr");

        Debug.Log(items.Count);
        foreach (HtmlNode item in items)
        {
            HtmlNode title = item.SelectSingleNode(".//a[@title]");
            if (title == null)
            {
                Debug.Log("no title");
                continue;
            }

            string name = title.GetAttributeValue("title", "");

            string detailUrl = title.GetAttributeValue("href", "");
            string engName = ParseAnimalDetail("https://wiki.biligame.com" + detailUrl);

            string imgUrl = title.LastChild.GetAttributeValue("src", "");

            if (engName == "")
            {
                Debug.LogError("NO id: " + name);
                continue;
            }

            Debug.Log(name + " : " + engName + " : " + detailUrl);

            // StartCoroutine(SaveImage("i_" + engName, imgUrl));

            HtmlNodeCollection dataNodes = item.SelectNodes(".//td");
            if (dataNodes.Count != 6)
            {
                Debug.LogError(" no 6 data: " + name);
                continue;
            }



            ACAnimalData data = new ACAnimalData();
            data.name = name;

            string sexTag = dataNodes[1].WriteContentTo();
            if (sexTag.Contains("♂"))
                data.sex = "male";
            else if (sexTag.Contains("♀"))
                data.sex = "female";
            else
                Debug.LogError("no Sex: " + name);



            data.personality = dataNodes[2].WriteContentTo();
            data.personality = data.personality.Replace("\n", "");
            data.specie = dataNodes[3].WriteContentTo();
            data.specie = data.specie.Replace("\n", "");
            data.birthday = dataNodes[4].WriteContentTo();
            data.birthday = data.birthday.Replace("\n", "");
            data.catchprase = dataNodes[5].WriteContentTo();
            data.catchprase = data.catchprase.Replace("\n", "");

            animalDex[engName] = data;
        }

        string dexJson = JsonConvert.SerializeObject(animalDex);
        string path = "C:/Users/user/Downloads";
        File.WriteAllText(path + "/acanimal.json", dexJson);

    }

    string ParseAnimalDetail(string url)
    {
        string engName = "";

        WebClient client = new WebClient();
        MemoryStream ms = new MemoryStream(client.DownloadData(url));

        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms);

        HtmlNode nameNode = doc.DocumentNode.SelectSingleNode("//font[.='外文名称']");
        if (nameNode == null || nameNode.NextSibling == null)
        {
            Debug.LogError(url);
            return engName;
        }

        engName = nameNode.NextSibling.WriteContentTo();

        engName = engName.Substring(0, engName.IndexOf('('));
        engName = engName.ToLower();
        engName = engName.Replace(" ", "");
        engName = engName.Replace(".", "");

        HtmlNode imgNode = doc.DocumentNode.SelectSingleNode("//a[@class = 'image']");
        string imgUrl = imgNode.LastChild.GetAttributeValue("src", "");

        // StartCoroutine(SaveImage(engName, imgUrl));

        return engName;
    }




    void ParseEXPGG()
    {
        WebClient client = new WebClient();
        MemoryStream ms = new MemoryStream(client.DownloadData(
            "https://exp.gg/zh_tw/exp%E5%8E%9F%E5%89%B5%E5%B0%88%E5%8D%80-zh_tw/130809/amp"));

        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms);

        HtmlNode tableNode = doc.DocumentNode.SelectSingleNode("//table[@class = 'acssc211e']");
        HtmlNodeCollection items = tableNode.SelectNodes(".//tr");

        foreach (HtmlNode item in items)
        {
            HtmlNodeCollection datas = item.SelectNodes(".//td");
            Debug.Log(datas.Count);

            if (datas.Count < 7)
                continue;

            Debug.Log(datas[0].FirstChild.Name);

            HtmlNodeCollection spans = datas[0].SelectNodes(".//span[@class =  'acss8af2a']");
            if (spans == null)
                continue;

            string name = "";
            foreach (HtmlNode span in spans)
            {
                name += span.WriteContentTo();
            }
            Debug.Log(name);

            string priceText = datas[1].SelectSingleNode(".//div").WriteContentTo();
            Debug.Log(priceText);
        }
    }

    void ParseEXPGGICON(string url)
    {
        WebClient client = new WebClient();
        MemoryStream ms = new MemoryStream(client.DownloadData(
            url));

        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms);

        HtmlNode tableNode = doc.DocumentNode.SelectSingleNode("//div[@class = 'acnh_monthly']");
        HtmlNodeCollection items = tableNode.SelectNodes(".//tr");

        foreach (HtmlNode item in items)
        {

            if (item.SelectNodes(".//td") == null)
                continue;


            HtmlNodeCollection tds = item.SelectNodes(".//td");

            Debug.Log(tds.Count);


            string fishUrl = tds[0].SelectNodes(".//img")[1].GetAttributeValue("src", "");

            string name = tds[0].SelectNodes(".//img")[1].GetAttributeValue("alt", "");
            string id = name.ToLower();
            id = id.Replace(" ", "");

            StartCoroutine(SaveImage(id, fishUrl));
        }
    }


    IEnumerator SaveImage(string name, string url)
    {
        Debug.Log(name + ": " + url);
        Texture2D texture = new Texture2D(100, 100);

        WWW www = new WWW(url);
        yield return www;

        // calling this function with StartCoroutine solves the problem
        Debug.Log("Why on earh is this never called?");

        www.LoadImageIntoTexture(texture);
        www.Dispose();
        www = null;

        // texture = ScaleTexture(texture, 100, 100);

        byte[] bytes = texture.EncodeToPNG();

        string path = "C:/Users/user/Downloads";
        System.IO.File.WriteAllBytes(path + "/" + name + ".png", bytes);


    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }








    void ParseGameWith()
    {
        ACDex dex = new ACDex();


        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17098", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17099", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17100", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17101", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17102", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17103", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17104", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17105", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17106", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17107", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17108", dex);
        ParseMonth("https://gamewith.net/animal-crossing-new-horizons/article/show/17109", dex);


        string dexJson = JsonConvert.SerializeObject(dex.fish);
        string path = "C:/Users/user/Downloads";
        File.WriteAllText(path + "/acfish.json", dexJson);
    }


    void ParseMonth(string url, ACDex dex)
    {
        WebClient client = new WebClient();
        MemoryStream ms = new MemoryStream(client.DownloadData(url));


        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms);

        HtmlNode tableNode = doc.DocumentNode.SelectSingleNode("//div[@class = 'acnh_monthly']");
        Debug.Log(tableNode.Name);

        HtmlNodeCollection itemNodes = tableNode.SelectNodes(".//tr");

        Debug.Log(itemNodes.Count);


        foreach (HtmlNode itemNode in itemNodes)
        {

            if (itemNode.SelectNodes(".//td") == null)
                continue;

            Debug.Log(itemNode.Name + " : " + itemNode.ChildNodes.Count);

            HtmlNodeCollection tds = itemNode.SelectNodes(".//td");
            string fishUrl = tds[0].SelectSingleNode(".//a").GetAttributeValue("href", "");

            ParseFish(fishUrl, dex);
        }
    }

    void ParseBugMonth(string url, ACDex dex)
    {
        WebClient client = new WebClient();
        MemoryStream ms = new MemoryStream(client.DownloadData(url));

        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms);

        HtmlNode tableNode = doc.DocumentNode.SelectSingleNode("//div[@class = 'acnh_monthly']");
        Debug.Log(tableNode.Name);


        HtmlNodeCollection itemNodes = tableNode.SelectNodes(".//tr");

        foreach (HtmlNode itemNode in itemNodes)
        {

            if (itemNode.SelectNodes(".//td") == null)
                continue;

            Debug.Log(itemNode.Name + " : " + itemNode.ChildNodes.Count);

            HtmlNodeCollection tds = itemNode.SelectNodes(".//td");
            string fishUrl = tds[0].SelectSingleNode(".//a").GetAttributeValue("href", "");

            Debug.Log(fishUrl);
            ParseBug(fishUrl, dex);

            // ParseFish(fishUrl, dex);
        }


    }


    void ParseBug(string url, ACDex dex)
    {
        WebClient client = new WebClient();
        MemoryStream ms = new MemoryStream(client.DownloadData(url));

        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms);

        HtmlNode mushiNode = doc.DocumentNode.SelectSingleNode("//div[@class ='acnh_mushi']");
        if (mushiNode == null)
        {
            Debug.LogError("No Mushi: " + url);
            return;
        }

        HtmlNode nameNode = mushiNode.SelectSingleNode(".//th");

        string name = nameNode.WriteContentTo();

        ACBugData data;
        if (dex.bug.TryGetValue(name, out data) == false)
        {
            data = new ACBugData();
        }

        data.name = name;


        string sellString = nameNode.ParentNode.NextSibling.NextSibling.NextSibling.FirstChild.WriteContentTo();
        // Debug.Log(sellString);
        int sellPrice;
        if (int.TryParse(sellString, out sellPrice))
        {
            data.sellPrice = sellPrice;
        }
        else
        {
            Debug.Log("Price parsing error: " + sellString);
            data.sellPrice = -1;
        }


        HtmlNode timeNode = doc.DocumentNode.SelectSingleNode("//th[.='Time Of Day']").ParentNode;
        data.time = timeNode.LastChild.WriteContentTo();
        // Debug.Log(data.time);

        data.location = timeNode.NextSibling.LastChild.WriteContentTo();
        // Debug.Log(data.location);

        HtmlNode spawnN = doc.DocumentNode.SelectSingleNode("//h3[.='Spawn Period - Northern Hemisphere']");
        HtmlNodeCollection months = spawnN.NextSibling.SelectNodes(".//td");

        string monthText = "";
        for (int i = 0; i < months.Count; i++)
        {
            string month = months[i].WriteContentTo();
            monthText += month + "_";
            data.monthsNorth[i] = month == "x" ? 0 : 1;
        }

        // Debug.Log(monthText);

        HtmlNode spawnS = spawnN.NextSibling.NextSibling;
        months = spawnS.NextSibling.SelectNodes(".//td");

        for (int i = 0; i < months.Count; i++)
        {
            string month = months[i].WriteContentTo();
            monthText += month + "_";
            data.monthsSouth[i] = month == "x" ? 0 : 1;
        }

        string key = name.ToLower();
        key = key.Replace(" ", "");


        dex.bug[key] = data;
    }




    void ParseFish(string url, ACDex dex)
    {

        // Debug.Log("ParseFish: " + url);

        WebClient client = new WebClient();
        MemoryStream ms = new MemoryStream(client.DownloadData(url));

        HtmlDocument doc = new HtmlDocument();
        doc.Load(ms);


        HtmlNode nameNode = doc.DocumentNode.SelectSingleNode("//div[@class ='acnh_mushi']").SelectSingleNode(".//th");


        string name = nameNode.WriteContentTo();
        // Debug.Log(name);

        ACFishData data;
        if (dex.fish.TryGetValue(name, out data) == false)
        {
            data = new ACFishData();
        }

        data.name = name;

        string sellString = nameNode.ParentNode.NextSibling.NextSibling.NextSibling.FirstChild.WriteContentTo();
        // Debug.Log(sellString);
        int sellPrice;
        if (int.TryParse(sellString, out sellPrice))
        {
            data.sellPrice = sellPrice;
        }
        else
        {
            Debug.Log("Price parsing error: " + sellString);
            data.sellPrice = -1;
        }


        HtmlNode timeNode = doc.DocumentNode.SelectSingleNode("//th[.='Time Of Day']").ParentNode;
        data.time = timeNode.LastChild.WriteContentTo();
        // Debug.Log(data.time);

        data.location = timeNode.NextSibling.LastChild.WriteContentTo();
        // Debug.Log(data.location);

        HtmlNode spawnN = doc.DocumentNode.SelectSingleNode("//h3[.='Spawn Period - Northern Hemisphere']");
        HtmlNodeCollection months = spawnN.NextSibling.SelectNodes(".//td");

        string monthText = "";
        for (int i = 0; i < months.Count; i++)
        {
            string month = months[i].WriteContentTo();
            monthText += month + "_";
            data.monthsNorth[i] = month == "x" ? 0 : 1;
        }

        // Debug.Log(monthText);

        HtmlNode spawnS = spawnN.NextSibling.NextSibling;
        months = spawnS.NextSibling.SelectNodes(".//td");

        for (int i = 0; i < months.Count; i++)
        {
            string month = months[i].WriteContentTo();
            monthText += month + "_";
            data.monthsSouth[i] = month == "x" ? 0 : 1;
        }


        dex.fish[name] = data;
    }








    // Update is called once per frame
    void Update()
    {

    }
}
