using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientDataList_SO", menuName = "Customer/ClientDataList_SO")]
public class ClientDataList_SO : ScriptableObject
{
    public List<ClientDetails> clientDetailsList;

    public ClientDetails GetClientDetails(ClientName clientName)
    {
        return clientDetailsList.Find(i => i.clientName == clientName);
    }
}

[System.Serializable]
public class ClientDetails
{
    public ClientName clientName;
    public Sprite clientSprite;
    public float walkSpeed;
    public int stateTime;
    public float occurrence;
    public List<ItemName> favoriteFoodList;
    public List<ItemName> hateFoodList;
}
