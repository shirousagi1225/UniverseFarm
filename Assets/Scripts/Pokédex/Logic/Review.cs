using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Review : MonoBehaviour
{
    public ClientName clientName;
    public PokedexState pokedexState;

    public void ReviewClicked()
    {
        if(clientName!=ClientName.None)
            CustomerManager.Instance.EnterDialogue(clientName, pokedexState);
    }
}
