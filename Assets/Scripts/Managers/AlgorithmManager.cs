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
    private Dictionary<RarityType,float> rarityTypeDict = new Dictionary<RarityType,float>();//�}���׼ƾ�
    private ArrayList rarityTypeAList = new ArrayList();//�}���ׯ���
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

    //�]�w�U�ȦW��ΥX�{���v�ƥ�G�N�Ҧ��U�Ȩ̵}���װϤ�����,�åH��l���v�����,�ھڨC���ʶR���~���ߦn�{�׽վ���v(������)
    private void OnSetClientProbabilityEvent(ClientDetails clientDetails, int favoriteState)
    {
        if (!clientNameDict.ContainsValue(clientDetails))
        {
            clientNameDict.Add(clientNameDict.Count, clientDetails);

            //�}�����`�ȡG�����p��X���v���`�ȡA�Ψӭp���H���d��
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
            //�վ㷧�v���`�Ȥ�ҡA�ΨӰʺA��s�X�{���v
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

    //����U�Ȥ�k�G�̵}���װϬq�ӧO�U�Ȫ����v�h�M�w�̲רӳX�U��
    public ClientName ChooseClient()
    {
        RarityType rarity= ChooseRarityType();
        float clientNameTotal = 0;
        Dictionary<int, ClientDetails> accordRarityDict = new();
        int accordRarityCount = 0;
        Debug.Log("�}���סG" + rarity.ToString());

        //�M��ŦX�}���ת��U��
        foreach (var client in clientNameDict)
        {
            if (client.Value.rarityType == rarity)
            {
                accordRarityDict.Add(accordRarityCount, client.Value);
                clientNameTotal += client.Value.occurrence;
                accordRarityCount++;
                //Debug.Log("�ŦX�U�ȡG" + client.Value.clientName.ToString());
            }
        }

        float clientNameNob = UnityEngine.Random.Range(0, clientNameTotal);
        //Debug.Log("�ŦX�U�Ⱦ��v�G" + clientNameNob);

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

    //��ܵ}���פ�k�G�M�w�Ӧ��O�n�X�{���ص��ŰϬq���U��
    private RarityType ChooseRarityType()
    {
        RarityType rarity;
        float rarityTypeNob = UnityEngine.Random.Range(0, rarityTypeTotal);
        //Debug.Log("�}���׾��v�G" + rarityTypeNob);

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
