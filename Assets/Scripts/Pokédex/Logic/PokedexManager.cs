using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PokedexManager : Singleton<PokedexManager>
{
    private Stack<string> storyStack;
    private Stack<string> backgroundStack;
    private PokedexData_Ob pokedexData_Ob;
    private string pokedexJsonPath = Application.streamingAssetsPath + "/PokedexData/Json/Pokedex.json";
    private ClientDetails currentCustomer;

    protected override void Awake()
    {
        base.Awake();

        //����ݼg�b�}�l�C���ƥ�
        var pokedexJson = UnityWebRequest.Get(pokedexJsonPath);
        pokedexJson.SendWebRequest();
        StartCoroutine(WaitLoadData(pokedexJson));
        Debug.Log(pokedexJson.downloadHandler.text);
        pokedexData_Ob = JsonMapper.ToObject<PokedexData_Ob>(Encoding.GetEncoding("utf-8").GetString(pokedexJson.downloadHandler.data));
        Debug.Log(pokedexData_Ob);

        storyStack = new Stack<string>();
        backgroundStack = new Stack<string>();
    }

    //Ū����Ų�奻��k(������)
    public void GetPokedexFormFile(ClientDetails clientDetails)
    {
        currentCustomer = clientDetails;
        storyStack.Clear();
        backgroundStack.Clear();

        storyStack.Push(pokedexData_Ob.pokedexData[currentCustomer.clientName.ToString()].Find(i => i.storyName == "���@1").story);
        backgroundStack.Push(pokedexData_Ob.pokedexData[currentCustomer.clientName.ToString()].Find(i => i.storyName == "���@1").background);
    }

    //��ܹ�Ų��k(������)
    public void ShowPokedex()
    {
        if (storyStack.TryPop(out string storyResult) && backgroundStack.TryPop(out string backgroundResult))
        {
            Debug.Log(storyResult);
            Debug.Log(backgroundResult);
        }
    }

    //���ݤU��������k
    private IEnumerator WaitLoadData(UnityWebRequest pokedexJson)
    {
        while (!pokedexJson.isDone)
            yield return 0;
    }
}
