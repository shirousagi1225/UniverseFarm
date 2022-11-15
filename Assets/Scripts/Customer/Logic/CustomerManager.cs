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

    //�ͦ��U�Ȥ�k(������)
    public void CreateCustomer(GameObject spawnPoint)
    {
        //���אּ�P���U�Ȥ��|�P�ɥX�{
        Instantiate(customer, spawnPoint.transform.position,Quaternion.identity, spawnPoint.transform.parent);
        spawnPoint.transform.parent.GetChild(spawnPoint.transform.parent.childCount-1).GetComponent<Customer>().SetCustomer(clientData.GetClientDetails(AlgorithmManager.Instance.ChooseClient()));
    }
}
