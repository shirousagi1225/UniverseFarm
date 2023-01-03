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
    public PokedexNum pokedexNum;
    public ItemName seedName;
    public Sprite clientSprite;
    public AnimationClip clientAniClip;
    public float walkSpeed;
    public int stateTime;
    public RarityType rarityType;
    public float occurrence;
    public int pokedexState;
    public List<ItemName> favoriteFoodList;
    public Sprite favoriteFoodSprite;
    public List<ItemName> hateFoodList;
    public Sprite hateFoodSprite;
}
