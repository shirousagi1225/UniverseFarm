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

        //之後需寫在開始遊戲事件
        var pokedexJson = UnityWebRequest.Get(pokedexJsonPath);
        pokedexJson.SendWebRequest();
        StartCoroutine(WaitLoadData(pokedexJson));
        Debug.Log(pokedexJson.downloadHandler.text);
        pokedexData_Ob = JsonMapper.ToObject<PokedexData_Ob>(Encoding.GetEncoding("utf-8").GetString(pokedexJson.downloadHandler.data));
        Debug.Log(pokedexData_Ob);

        storyStack = new Stack<string>();
        backgroundStack = new Stack<string>();
    }

    //讀取圖鑑文本方法(未完成)
    public void GetPokedexFormFile(ClientDetails clientDetails)
    {
        currentCustomer = clientDetails;
        storyStack.Clear();
        backgroundStack.Clear();

        storyStack.Push(pokedexData_Ob.pokedexData[currentCustomer.clientName.ToString()].Find(i => i.storyName == "身世1").story);
        backgroundStack.Push(pokedexData_Ob.pokedexData[currentCustomer.clientName.ToString()].Find(i => i.storyName == "身世1").background);
    }

    //顯示圖鑑方法(未完成)
    public void ShowPokedex()
    {
        if (storyStack.TryPop(out string storyResult) && backgroundStack.TryPop(out string backgroundResult))
        {
            Debug.Log(storyResult);
            Debug.Log(backgroundResult);
        }
    }

    //等待下載完成方法
    private IEnumerator WaitLoadData(UnityWebRequest pokedexJson)
    {
        while (!pokedexJson.isDone)
            yield return 0;
    }
}
