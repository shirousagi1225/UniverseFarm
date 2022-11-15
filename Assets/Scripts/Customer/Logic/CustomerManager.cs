using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static ClientDataList_SO;

public class CustomerManager : Singleton<CustomerManager>
{
    public ClientDataList_SO clientData;
    public GameObject customer;

    [SerializeField] private List<ClientName> clientList = new List<ClientName>();

    public void AddClient(ClientName clientName)
    {
        if (!clientList.Contains(clientName))
        {
            clientList.Add(clientName);
        }
    }

    //生成顧客方法(未完成)
    public void CreateCustomer(GameObject spawnPoint)
    {
        //須改為同個顧客不會同時出現
        Instantiate(customer, spawnPoint.transform.position,Quaternion.identity, spawnPoint.transform.parent);
        spawnPoint.transform.parent.GetChild(spawnPoint.transform.parent.childCount-1).GetComponent<Customer>().SetCustomer(clientData.GetClientDetails(AlgorithmManager.Instance.ChooseClient()));
    }
}
