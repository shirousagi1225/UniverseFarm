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

    //�]�w�U���`���A���v�Ϊ�l��
    private void InitAbnormalProbability()
    {
        abnormalStateDict.Add(0, FarmlandState.Dry);
        abnormalStateDict.Add(1, FarmlandState.Deworming);
        abnormalStateDict.Add(2, FarmlandState.Fertilize);

        float probabilityValue = 1f/ abnormalStateDict.Count;
        abnormalProbability = new float[3] { probabilityValue, probabilityValue, probabilityValue };
        //Debug.Log(probabilityValue);

        //�����p��X���v���`�ȡA�Ψӭp���H���d��
        for (int i = 0; i < abnormalProbability.Length; i++)
        {
            abnormalTotal += abnormalProbability[i];
        }
    }

    //�֥[Ĳ�o���`���A���v��k
    public bool TriggerAbnormalState()
    {
        float nob = UnityEngine.Random.Range(0, 1.0f);
        Debug.Log("���`�֥[���v���ơG"+abnormalTries);

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

    //��ܲ��`���A������k
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

    //�]�w�U�ȥX�{���v�Ϊ�l��(�Ҽ{�g���ƥ�,�åB�ݭq�n�U�U�Ȫ���l�X�{���v,���A�Υ������t���v,������)
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

        //�����p��X���v���`�ȡA�Ψӭp���H���d��
        for (int i = 0; i < clientNameProbability.Length; i++)
        {
            clientNameTotal += clientNameProbability[i];
        }
    }

    //����U�Ȥ�k
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
