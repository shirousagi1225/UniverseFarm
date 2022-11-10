using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : Singleton<CustomerManager>
{
    public ClientDataList_SO clientData;

    [SerializeField] private List<ClientName> clientList = new List<ClientName>();

    public void AddClient(ClientName clientName)
    {
        if (!clientList.Contains(clientName))
        {
            clientList.Add(clientName);
        }
    }
}
