using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlgorithmManager : Singleton<AlgorithmManager>
{
    public float startAbnormalProbability;

    private Dictionary<int, FarmlandState> abnormalStateDict = new Dictionary<int, FarmlandState>();
    private float[] abnormalProbability;
    private float abnormalTotal = 0;
    private int abnormalTries=1;

    private Dictionary<int, ClientName> clientNameDict = new Dictionary<int, ClientName>();
    private float[] clientNameProbability;
    private float clientNameTotal = 0;

    void Start()
    {
        InitAbnormalProbability();
        InitClientProbability();
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

    //設定顧客出現機率及初始化(考慮寫成事件,並且需訂好各顧客的初始出現機率,不適用平均分配機率,未完成)
    private void InitClientProbability()
    {
        foreach (int clientName in Enum.GetValues(typeof(ClientName)))
        {
            if(clientName!=0)
                clientNameDict.Add(clientName-1, (ClientName)clientName);
        }

        float probabilityValue = 1f / clientNameDict.Count;
        clientNameProbability = new float[2] { probabilityValue, probabilityValue };
        //Debug.Log(probabilityValue);

        //首先計算出概率的總值，用來計算隨機範圍
        for (int i = 0; i < clientNameProbability.Length; i++)
        {
            clientNameTotal += clientNameProbability[i];
        }
    }

    //選擇顧客方法
    public ClientName ChooseClient()
    {
        float nob = UnityEngine.Random.Range(0, clientNameTotal);

        for (int i = 0; i < clientNameProbability.Length; i++)
        {
            if (nob < clientNameProbability[i])
            {
                return clientNameDict[i];
            }
            else
            {
                nob -= clientNameProbability[i];
            }
        }
        return clientNameDict[clientNameProbability.Length - 1];
    }
}
