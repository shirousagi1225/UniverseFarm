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
        //����ݼg�b�}�l�C���ƥ�
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

    //��s�U�ȹ�Ų�ƥ�G�ھڶǤJ�U�ȼƾڸ����U�ȹ�Ų
    private void OnUpdateCustomerPokedexEvent(ClientDetails clientDetails, int favoriteState)
    {
        if (clientDetails.pokedexState == 0)
        {
            //���궥�q�G�Ĥ@������
            foreach (var roleItem in roleItem.GetComponentsInChildren<Transform>())
            {
                //Debug.Log(roleItem);
                if (roleItem.name == clientDetails.pokedexNum.ToString())
                    roleItem.GetComponent<PolygonImage>().enabled = true;
            }
        }else if (favoriteState==0)
        {
            //���궥�q�G���w������
            likeFoodDict.Add(clientDetails.clientName,clientDetails.favoriteFoodSprite);
        }
        else if (favoriteState == 2)
        {
            //���궥�q�G�Q��������
            hateFoodDict.Add(clientDetails.clientName, clientDetails.hateFoodSprite);
        }
    }

    //��s�Ӫ���Ų�ƥ�G�ھڶǤJ�Ӫ��ƾڸ���Ӫ���Ų
    private void OnUpdateCropPokedexEvent(ItemDetails itemDetails)
    {
        foreach (var cropItem in cropItem.GetComponentsInChildren<Transform>())
        {
            //Debug.Log(cropItem);
            if (cropItem.name == itemDetails.pokedexNum.ToString())
                cropItem.GetComponent<PolygonImage>().enabled=true;
        }
    }

    //Ū����Ų�奻��k(������)
    public void GetPokedexFormFile(ClientName roleItemName)
    {
        storyStack.Clear();
        backgroundStack.Clear();

        storyStack.Push(pokedexData_Ob.pokedexData[roleItemName.ToString()].Find(i => i.storyName == "���@1").story);
        backgroundStack.Push(pokedexData_Ob.pokedexData[roleItemName.ToString()].Find(i => i.storyName == "���@1").background);
    }

    //��ܹ�Ų��k(������)
    public void ShowCustomerPokedex(ClientName roleItemName)
    {
        firstDialogue.GetComponent<Review>().clientName = roleItemName;

        //�P�_��Ų�O�_�w����߷R����
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

        //�P�_��Ų�O�_�w����Q������
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

        //�P�_��Ų�O�_�w������
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

    //���ݤU��������k
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
