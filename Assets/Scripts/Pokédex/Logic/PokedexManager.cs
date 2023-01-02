using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PokedexManager : Singleton<PokedexManager>
{
    public GameObject cropItem;
    public GameObject roleItem;
    public GameObject likeFood;
    public GameObject hateFood;
    public Transform firstDialogue;
    public Image likeDialogue;
    public Image hateDialogue;
    public Sprite likeUnlockSprite;
    public Sprite hateUnlockSprite;
    public Sprite likeDialogueSprite;
    public Sprite hateDialogueSprite;
    public GameObject storyBar;
    public Text story;
    public GameObject backgroundBar;
    public Text background;

    private Dictionary<ClientName,Sprite> likeFoodDict = new Dictionary<ClientName, Sprite>();
    private Dictionary<ClientName, Sprite> hateFoodDict = new Dictionary<ClientName, Sprite>();

    private Stack<string> storyStack;
    private Stack<string> backgroundStack;
    private PokedexData_Ob pokedexData_Ob;
    private string pokedexJsonPath = Application.streamingAssetsPath + "/PokedexData/Json/Pokedex.json";

    private void OnEnable()
    {
        //之後需寫在開始遊戲事件
        var pokedexJson = UnityWebRequest.Get(pokedexJsonPath);
        pokedexJson.SendWebRequest();
        StartCoroutine(WaitLoadData(pokedexJson));
        Debug.Log(pokedexJson.downloadHandler.text);
        //pokedexData_Ob = JsonMapper.ToObject<PokedexData_Ob>(Encoding.GetEncoding("utf-8").GetString(pokedexJson.downloadHandler.data));
        Debug.Log(pokedexData_Ob);

        storyStack = new Stack<string>();
        backgroundStack = new Stack<string>();

        EventHandler.UpdateCustomerPokedexEvent += OnUpdateCustomerPokedexEvent;
        EventHandler.UpdateCropPokedexEvent += OnUpdateCropPokedexEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateCustomerPokedexEvent -= OnUpdateCustomerPokedexEvent;
        EventHandler.UpdateCropPokedexEvent -= OnUpdateCropPokedexEvent;
    }

    //更新顧客圖鑑事件：根據傳入顧客數據解鎖顧客圖鑑
    private void OnUpdateCustomerPokedexEvent(ClientDetails clientDetails, int favoriteState)
    {
        if (clientDetails.pokedexState == 0)
        {
            //解鎖階段：第一次見面
            foreach (var roleItem in roleItem.GetComponentsInChildren<Transform>())
            {
                //Debug.Log(roleItem);
                if (roleItem.name == clientDetails.pokedexNum.ToString())
                    roleItem.GetComponent<PolygonImage>().enabled = true;
            }
        }else if (favoriteState==0)
        {
            //解鎖階段：喜歡的食物
            likeFoodDict.Add(clientDetails.clientName,clientDetails.favoriteFoodSprite);
        }
        else if (favoriteState == 2)
        {
            //解鎖階段：討厭的食物
            hateFoodDict.Add(clientDetails.clientName, clientDetails.hateFoodSprite);
        }
    }

    //更新植物圖鑑事件：根據傳入植物數據解鎖植物圖鑑
    private void OnUpdateCropPokedexEvent(ItemDetails itemDetails)
    {
        foreach (var cropItem in cropItem.GetComponentsInChildren<Transform>())
        {
            //Debug.Log(cropItem);
            if (cropItem.name == itemDetails.pokedexNum.ToString())
                cropItem.GetComponent<PolygonImage>().enabled=true;
        }
    }

    //讀取圖鑑文本方法(未完成)
    public void GetPokedexFormFile(ClientName roleItemName)
    {
        storyStack.Clear();
        backgroundStack.Clear();

        storyStack.Push(pokedexData_Ob.pokedexData[roleItemName.ToString()].Find(i => i.storyName == "身世1").story);
        backgroundStack.Push(pokedexData_Ob.pokedexData[roleItemName.ToString()].Find(i => i.storyName == "身世1").background);
    }

    //顯示圖鑑方法(未完成)
    public void ShowCustomerPokedex(ClientName roleItemName)
    {
        firstDialogue.GetComponent<Review>().clientName = roleItemName;

        //判斷圖鑑是否已解鎖喜愛食物
        if (likeFoodDict.ContainsKey(roleItemName))
        {
            likeDialogue.GetComponent<Review>().clientName = roleItemName;
            likeFood.transform.GetChild(0).gameObject.SetActive(false);
            likeFood.transform.GetChild(1).gameObject.SetActive(true);
            likeFood.transform.GetChild(1).GetComponent<Image>().sprite= likeFoodDict[roleItemName];
            likeDialogue.sprite = likeDialogueSprite;
        }
        else
        {
            likeDialogue.GetComponent<Review>().clientName = ClientName.None;
            likeFood.transform.GetChild(0).gameObject.SetActive(true);
            likeFood.transform.GetChild(1).gameObject.SetActive(false);
            likeDialogue.sprite = likeUnlockSprite;
        }

        //判斷圖鑑是否已解鎖討厭食物
        if (hateFoodDict.ContainsKey(roleItemName))
        {
            hateDialogue.GetComponent<Review>().clientName = roleItemName;
            hateFood.transform.GetChild(0).gameObject.SetActive(false);
            hateFood.transform.GetChild(1).gameObject.SetActive(true);
            hateFood.transform.GetChild(1).GetComponent<Image>().sprite = hateFoodDict[roleItemName];
            hateDialogue.sprite = hateDialogueSprite;
        }
        else
        {
            hateDialogue.GetComponent<Review>().clientName = ClientName.None;
            hateFood.transform.GetChild(0).gameObject.SetActive(true);
            hateFood.transform.GetChild(1).gameObject.SetActive(false);
            hateDialogue.sprite = hateUnlockSprite;
        }

        //判斷圖鑑是否已全解鎖
        if (likeFoodDict.ContainsKey(roleItemName)&& hateFoodDict.ContainsKey(roleItemName))
        {
            GetPokedexFormFile(roleItemName);

            if (storyStack.TryPop(out string storyResult) && backgroundStack.TryPop(out string backgroundResult))
            {
                story.text=storyResult;
                background.text=backgroundResult;
            }

            storyBar.transform.GetChild(0).gameObject.SetActive(false);
            storyBar.transform.GetChild(1).gameObject.SetActive(true);
            backgroundBar.transform.GetChild(0).gameObject.SetActive(false);
            backgroundBar.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            storyBar.transform.GetChild(0).gameObject.SetActive(true);
            storyBar.transform.GetChild(1).gameObject.SetActive(false);
            backgroundBar.transform.GetChild(0).gameObject.SetActive(true);
            backgroundBar.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    //等待下載完成方法
    private IEnumerator WaitLoadData(UnityWebRequest pokedexJson)
    {
        while (!pokedexJson.isDone)
            yield return 0;
        while (pokedexData_Ob == null)
        {
            pokedexData_Ob = JsonMapper.ToObject<PokedexData_Ob>(Encoding.GetEncoding("utf-8").GetString(pokedexJson.downloadHandler.data));
            yield return 0;
        }
    }
}
