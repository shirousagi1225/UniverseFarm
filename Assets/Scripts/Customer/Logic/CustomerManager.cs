using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class CustomerManager : Singleton<CustomerManager>
{
    public ClientDataList_SO clientData;
    public GameObject customer;

    private Dictionary<ClientName, List<PokedexState>> clientDict = new();

    private void OnEnable()
    {
        EventHandler.GetOtherCustomerEvent += OnOtherCustomerEvent;
        //測試用,正式根據初始角色數量而定
        AddClient(ClientName.艾絲琪);
        AddClient(ClientName.亞伯特);
        AddClient(ClientName.雅蘭娜);
        AddClient(ClientName.漢吉);
        AddClient(ClientName.倫納德);
        AddClient(ClientName.多莉絲);
        AddClient(ClientName.尼古拉斯);
        AddClient(ClientName.魔法阿罵);
    }

    private void OnDisable()
    {
        EventHandler.GetOtherCustomerEvent -= OnOtherCustomerEvent;
    }

    //切換圖層事件：當角色間靠近時根據位置遠近切換顯示前後順序,避免重疊(未完成)
    private void OnOtherCustomerEvent(Transform customer, Collider2D[] collider2Ds)
    {
        if (collider2Ds.Length > 2)
        {
            if (collider2Ds[2].transform.position.y > customer.position.y)
            {
                customer.GetComponent<SpriteRenderer>().sortingOrder = 2;
                collider2Ds[2].transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            else
            {
                customer.GetComponent<SpriteRenderer>().sortingOrder = 1;
                collider2Ds[2].transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
        else if (collider2Ds.Length == 2)
            customer.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void AddClient(ClientName clientName)
    {
        if (!clientDict.ContainsKey(clientName))
        {
            List<PokedexState> pokedexStateList = new();
            clientDict.Add(clientName, pokedexStateList);
            clientDict[clientName].Add(PokedexState.初次見面);
            EventHandler.CallSetClientProbabilityEvent(clientData.GetClientDetails(clientName),1);
            //Debug.Log(pokedexStateList.Count);
            //Debug.Log(clientDict[clientName].Count);
        }
    }

    //生成顧客方法(未完成)
    public void CreateCustomer(GameObject spawnPoint)
    {
        Instantiate(customer, spawnPoint.transform.position,Quaternion.identity, spawnPoint.transform.parent);
        spawnPoint.transform.parent.GetChild(spawnPoint.transform.parent.childCount-1).GetComponent<Customer>().SetCustomer(clientData.GetClientDetails(AlgorithmManager.Instance.ChooseClient()));
    }

    //進入對話方法(未完成)
    public void EnterDialogue(ClientName clientName)
    {
        //Debug.Log(clientDict[clientName].Count);

        //需判斷角色圖鑑解鎖階段,顯示相對應的對話
        if (clientDict[clientName].Count-1== clientData.GetClientDetails(clientName).pokedexState)
        {
            //啟動子UI
            EventHandler.CallShowSecUIEvent("SecCanvas", true,true);
            //啟動對話方法
            DialogueManager.Instance.GetDialogueFormFile(clientData.GetClientDetails(clientName), clientDict[clientName][clientDict[clientName].Count-1].ToString());
            DialogueManager.Instance.ShowDialogue();

            //測試用,正式要刪除
            //PokedexManager.Instance.GetPokedexFormFile(clientData.GetClientDetails(clientName));

            clientData.GetClientDetails(clientName).pokedexState++;
        }
    }

    //購買作物選擇方法(未完成)
    public int BuyCrops(ClientName clientName)
    {
        int favoriteState=0;
        Dictionary<int, Crop> matureCropDict=new();
        int matureCropCount = 0;

        //尋找已成熟的作物放入字典中,沒有成熟作物回傳0
        foreach (var crop in FindObjectsOfType<Crop>())
        {
            if (crop.growthStage == 3)
            {
                matureCropDict.Add(matureCropCount, crop);
                matureCropCount++;
            }   
        }

        //依照喜好程度依序判斷顧客是否進行購買,喜好程度高優先購買
        while (favoriteState<3&& matureCropCount != 0)
        {
            Debug.Log("喜好程度："+favoriteState);
            if (favoriteState == 0)
            {
                //喜愛(購買比例：100%～80%)
                for (int i = 0; i < matureCropDict.Count; i++)
                {
                    if (clientData.GetClientDetails(clientName).favoriteFoodList.Contains(matureCropDict[i].cropName))
                    {
                        int produce = FarmlandManager.Instance.Produce(matureCropDict[i].seedName);
                        int soldCount = (int)(produce * UnityEngine.Random.Range(0.8f, 1.0f));
                        if (soldCount != produce)
                            InventoryManager.Instance.AddItem(matureCropDict[i].cropName, soldCount, produce - soldCount);
                        matureCropDict[i].growthStage=4;
                        StartCoroutine(matureCropDict[i].Harvest());
                        EventHandler.CallSetClientProbabilityEvent(clientData.GetClientDetails(clientName), 0);

                        //判斷是否購買到喜歡的作物,是的話新增特殊對話
                        if (!clientDict[clientName].Contains(PokedexState.喜歡)&& clientData.GetClientDetails(clientName).pokedexState!=0)
                            clientDict[clientName].Add(PokedexState.喜歡);

                        return soldCount;
                    }
                }
                favoriteState++;
            }
            else if (favoriteState == 1)
            {
                //能吃(購買比例：70%～40%)
                for (int i = 0; i < matureCropDict.Count; i++)
                {
                    if (!clientData.GetClientDetails(clientName).hateFoodList.Contains(matureCropDict[i].cropName))
                    {
                        int produce = FarmlandManager.Instance.Produce(matureCropDict[i].seedName);
                        int soldCount = (int)(produce * UnityEngine.Random.Range(0.4f, 0.7f));
                        InventoryManager.Instance.AddItem(matureCropDict[i].cropName, soldCount, produce - soldCount);
                        matureCropDict[i].growthStage = 4;
                        StartCoroutine(matureCropDict[i].Harvest());
                        return soldCount;
                    }
                }
                favoriteState++;
            }
            else if(favoriteState == 2)
            {
                //討厭(購買比例：30%～10%)
                if (clientData.GetClientDetails(clientName).hateFoodList.Contains(matureCropDict[0].cropName))
                {
                    int produce = FarmlandManager.Instance.Produce(matureCropDict[0].seedName);
                    int soldCount = (int)(produce * UnityEngine.Random.Range(0.1f, 0.3f));
                    InventoryManager.Instance.AddItem(matureCropDict[0].cropName, soldCount, produce - soldCount);
                    matureCropDict[0].growthStage = 4;
                    StartCoroutine(matureCropDict[0].Harvest());
                    EventHandler.CallSetClientProbabilityEvent(clientData.GetClientDetails(clientName), 2);

                    //判斷是否購買到討厭的作物,是的話新增特殊對話
                    if (!clientDict[clientName].Contains(PokedexState.討厭) && clientData.GetClientDetails(clientName).pokedexState != 0)
                        clientDict[clientName].Add(PokedexState.討厭);

                    return soldCount;
                }
                favoriteState++;
            }
        }
        return 0;
    }
}
