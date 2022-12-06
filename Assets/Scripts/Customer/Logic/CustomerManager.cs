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
    }

    private void OnDisable()
    {
        EventHandler.GetOtherCustomerEvent -= OnOtherCustomerEvent;
    }

    //�����ϼh�ƥ�G�����ⶡ�a��ɮھڦ�m���������ܫe�ᶶ��,�קK���|(������)
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
            clientDict[clientName].Add(PokedexState.�즸����);
            //Debug.Log(pokedexStateList.Count);
            //Debug.Log(clientDict[clientName].Count);
        }
    }

    //�ͦ��U�Ȥ�k(������)
    public void CreateCustomer(GameObject spawnPoint)
    {
        Instantiate(customer, spawnPoint.transform.position,Quaternion.identity, spawnPoint.transform.parent);
        spawnPoint.transform.parent.GetChild(spawnPoint.transform.parent.childCount-1).GetComponent<Customer>().SetCustomer(clientData.GetClientDetails(AlgorithmManager.Instance.ChooseClient()));
    }

    //�i�J��ܤ�k(������)
    public void EnterDialogue(ClientName clientName)
    {
        //Debug.Log(clientDict[clientName].Count);

        //�ݧP�_�����Ų���궥�q,��ܬ۹��������
        if (clientDict[clientName].Count-1== clientData.GetClientDetails(clientName).pokedexState)
        {
            //�ҰʤlUI
            EventHandler.CallShowSecUIEvent(true,true);
            //�Ұʹ�ܤ�k
            DialogueManager.Instance.GetDialogueFormFile(clientData.GetClientDetails(clientName), clientDict[clientName][clientDict[clientName].Count-1].ToString());
            DialogueManager.Instance.ShowDialogue();

            //���ե�,�����n�R��
            PokedexManager.Instance.GetPokedexFormFile(clientData.GetClientDetails(clientName));

            clientData.GetClientDetails(clientName).pokedexState++;
        }
    }

    //�ʶR�@����ܤ�k(������)
    public int BuyCrops(ClientName clientName)
    {
        int favoriteState=0;
        Dictionary<int, Crop> matureCropDict=new();
        int matureCropCount = 0;

        //�M��w�������@����J�r�夤,�S�������@���^��0
        foreach (var crop in FindObjectsOfType<Crop>())
        {
            if (crop.growthStage == 3)
            {
                matureCropDict.Add(matureCropCount, crop);
                matureCropCount++;
            }   
        }

        //�̷ӳߦn�{�ר̧ǧP�_�U�ȬO�_�i���ʶR,�ߦn�{�װ��u���ʶR
        while (favoriteState<3&& matureCropCount != 0)
        {
            Debug.Log("�ߦn�{�סG"+favoriteState);
            if (favoriteState == 0)
            {
                //�߷R(�ʶR��ҡG100%��80%)
                for (int i = 0; i < matureCropDict.Count; i++)
                {
                    if (clientData.GetClientDetails(clientName).favoriteFoodList.Contains(matureCropDict[i].cropName))
                    {
                        int produce = FarmlandManager.Instance.Produce(matureCropDict[i].seedName);
                        int soldCount = (int)(produce * UnityEngine.Random.Range(0.8f, 1.0f));
                        if (soldCount != produce)
                            InventoryManager.Instance.AddItem(matureCropDict[i].cropName, produce - soldCount);
                        matureCropDict[i].growthStage=4;
                        StartCoroutine(matureCropDict[i].Harvest());

                        //�P�_�O�_�ʶR����w���@��,�O���ܷs�W�S�����
                        if (!clientDict[clientName].Contains(PokedexState.���w)&& clientData.GetClientDetails(clientName).pokedexState!=0)
                            clientDict[clientName].Add(PokedexState.���w);

                        return soldCount;
                    }
                }
                favoriteState++;
            }
            else if (favoriteState == 1)
            {
                //��Y(�ʶR��ҡG70%��40%)
                for (int i = 0; i < matureCropDict.Count; i++)
                {
                    if (!clientData.GetClientDetails(clientName).hateFoodList.Contains(matureCropDict[i].cropName))
                    {
                        int produce = FarmlandManager.Instance.Produce(matureCropDict[i].seedName);
                        int soldCount = (int)(produce * UnityEngine.Random.Range(0.4f, 0.7f));
                        InventoryManager.Instance.AddItem(matureCropDict[i].cropName, produce - soldCount);
                        matureCropDict[i].growthStage = 4;
                        StartCoroutine(matureCropDict[i].Harvest());
                        return soldCount;
                    }
                }
                favoriteState++;
            }
            else if(favoriteState == 2)
            {
                //�Q��(�ʶR��ҡG30%��10%)
                if (clientData.GetClientDetails(clientName).hateFoodList.Contains(matureCropDict[0].cropName))
                {
                    int produce = FarmlandManager.Instance.Produce(matureCropDict[0].seedName);
                    int soldCount = (int)(produce * UnityEngine.Random.Range(0.1f, 0.3f));
                    InventoryManager.Instance.AddItem(matureCropDict[0].cropName, produce - soldCount);
                    matureCropDict[0].growthStage = 4;
                    StartCoroutine(matureCropDict[0].Harvest());

                    //�P�_�O�_�ʶR��Q�����@��,�O���ܷs�W�S�����
                    if (!clientDict[clientName].Contains(PokedexState.�Q��) && clientData.GetClientDetails(clientName).pokedexState != 0)
                        clientDict[clientName].Add(PokedexState.�Q��);

                    return soldCount;
                }
                favoriteState++;
            }
        }
        return 0;
    }
}