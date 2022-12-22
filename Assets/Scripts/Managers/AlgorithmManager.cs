using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlgorithmManager : Singleton<AlgorithmManager>
{
    public float startAbnormalProbability;
    public float maxVisitValue;
    public float minVisitValue;
    public float visitChangeValue;

    private Dictionary<int, FarmlandState> abnormalStateDict = new Dictionary<int, FarmlandState>();
    private float[] abnormalProbability;
    private float abnormalTotal = 0;
    private int abnormalTries=1;

    private Dictionary<int, ClientDetails> clientNameDict = new Dictionary<int, ClientDetails>();
    private Dictionary<RarityType,float> rarityTypeDict = new Dictionary<RarityType,float>();//稀有度數據
    private ArrayList rarityTypeAList = new ArrayList();//稀有度索引
    private float rarityTypeTotal = 0;

    private void OnEnable()
    {
        EventHandler.SetClientProbabilityEvent += OnSetClientProbabilityEvent;
        InitAbnormalProbability();
    }

    private void OnDisable()
    {
        EventHandler.SetClientProbabilityEvent -= OnSetClientProbabilityEvent;
    }

    //設定顧客名單及出現機率事件：將所有顧客依稀有度區分等級,並以初始機率為基準,根據每次購買物品的喜好程度調整機率(未完成)
    private void OnSetClientProbabilityEvent(ClientDetails clientDetails, int favoriteState)
    {
        if (!clientNameDict.ContainsValue(clientDetails))
        {
            clientNameDict.Add(clientNameDict.Count, clientDetails);

            //稀有度總值：首先計算出概率的總值，用來計算隨機範圍
            if (!rarityTypeDict.ContainsKey(clientDetails.rarityType))
            {
                switch (clientDetails.rarityType)
                {
                    case RarityType.C:
                        rarityTypeDict.Add(clientDetails.rarityType, 0.4375f);
                        break;
                    case RarityType.B:
                        rarityTypeDict.Add(clientDetails.rarityType, 0.3125f);
                        break;
                    case RarityType.A:
                        rarityTypeDict.Add(clientDetails.rarityType, 0.1875f);
                        break;
                    case RarityType.S:
                        rarityTypeDict.Add(clientDetails.rarityType, 0.0625f);
                        break;
                }
                rarityTypeAList.Add(clientDetails.rarityType);
                rarityTypeTotal += rarityTypeDict[clientDetails.rarityType];
            }
        }
        /*else
        {
            //調整概率的總值比例，用來動態更新出現機率
            if (favoriteState == 0)
            {
                if(clientDetails.occurrence >= maxVisitValue)
                    clientDetails.occurrence = maxVisitValue;
                else
                    clientDetails.occurrence += visitChangeValue;

                foreach (var client in clientNameDict)
                {
                    if (client.Value.clientName!= clientDetails.clientName)
                    {
                        if(client.Value.occurrence <= minVisitValue)
                            client.Value.occurrence = minVisitValue;
                        else
                            client.Value.occurrence -= visitChangeValue / (clientNameDict.Count-1);
                    }
                }
            }
            else if (favoriteState == 2)
            {
                if (clientDetails.occurrence <= minVisitValue)
                    clientDetails.occurrence = minVisitValue;
                else
                    clientDetails.occurrence -= visitChangeValue;

                foreach (var client in clientNameDict)
                {
                    if (client.Value.clientName != clientDetails.clientName)
                    {
                        if (client.Value.occurrence >= maxVisitValue)
                            client.Value.occurrence = maxVisitValue;
                        else
                            client.Value.occurrence += visitChangeValue / (clientNameDict.Count - 1);
                    }
                }
            }
        }*/
    }

    //設定各異常狀態機率及初始化
    private void InitAbnormalProbability()
    {
        abnormalStateDict.Add(0, FarmlandState.Dry);
        abnormalStateDict.Add(1, FarmlandState.Deworming);
        abnormalStateDict.Add(2, FarmlandState.Fertilize);

        float probabilityValue = 1f/ abnormalStateDict.Count;
        abnormalProbability = new float[3] { probabilityValue, probabilityValue, probabilityValue };
        //Debug.Log(probabilityValue);

        //首先計算出概率的總值，用來計算隨機範圍
        for (int i = 0; i < abnormalProbability.Length; i++)
        {
            abnormalTotal += abnormalProbability[i];
        }
    }

    //累加觸發異常狀態機率方法
    public bool TriggerAbnormalState()
    {
        float nob = UnityEngine.Random.Range(0, 1.0f);
        Debug.Log("異常累加機率次數："+abnormalTries);

        if (nob < startAbnormalProbability * abnormalTries)
        {
            abnormalTries = 1;
            return true;
        }
        else
        {
            abnormalTries++;
            return false;
        }
    }

    //選擇異常狀態種類方法
    public FarmlandState ChooseAbnormalState()
    {
        float nob= UnityEngine.Random.Range(0, abnormalTotal);

        for (int i = 0; i < abnormalProbability.Length; i++)
        {
            if (nob < abnormalProbability[i])
            {
                return abnormalStateDict[i];
            }
            else
            {
                nob -= abnormalProbability[i];
            }
        }
        return abnormalStateDict[abnormalProbability.Length - 1];
    }

    //選擇顧客方法：依稀有度區段個別顧客的機率去決定最終來訪顧客
    public ClientName ChooseClient()
    {
        RarityType rarity= ChooseRarityType();
        float clientNameTotal = 0;
        Dictionary<int, ClientDetails> accordRarityDict = new();
        int accordRarityCount = 0;
        Debug.Log("稀有度：" + rarity.ToString());

        //尋找符合稀有度的顧客
        foreach (var client in clientNameDict)
        {
            if (client.Value.rarityType == rarity)
            {
                accordRarityDict.Add(accordRarityCount, client.Value);
                clientNameTotal += client.Value.occurrence;
                accordRarityCount++;
                //Debug.Log("符合顧客：" + client.Value.clientName.ToString());
            }
        }

        float clientNameNob = UnityEngine.Random.Range(0, clientNameTotal);
        //Debug.Log("符合顧客機率：" + clientNameNob);

        for (int i = 0; i < accordRarityDict.Count; i++)
        {
            if (clientNameNob < accordRarityDict[i].occurrence)
            {
                return accordRarityDict[i].clientName;
            }
            else
            {
                clientNameNob -= accordRarityDict[i].occurrence;
            }
        }
        return accordRarityDict[accordRarityDict.Count - 1].clientName;
    }

    //選擇稀有度方法：決定該次是要出現哪種等級區段的顧客
    private RarityType ChooseRarityType()
    {
        RarityType rarity;
        float rarityTypeNob = UnityEngine.Random.Range(0, rarityTypeTotal);
        //Debug.Log("稀有度機率：" + rarityTypeNob);

        for (int i = 0; i < rarityTypeDict.Count; i++)
        {
            rarity = (RarityType)rarityTypeAList[i];
            if (rarityTypeNob < rarityTypeDict[rarity])
            {
                return rarity;
            }
            else
            {
                rarityTypeNob -= rarityTypeDict[rarity];
            }
        }
        return (RarityType)rarityTypeAList[rarityTypeDict.Count-1];
    }
}
