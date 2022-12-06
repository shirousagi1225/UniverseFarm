using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokedexUI : MonoBehaviour
{
    private bool isPokedexOpen;

    private void OnEnable()
    {
        EventHandler.ShowPokedexEvent += OnShowPokedexEvent;
        isPokedexOpen=false;
    }

    private void OnDisable()
    {
        EventHandler.ShowPokedexEvent += OnShowPokedexEvent;
    }

    private void OnShowPokedexEvent(ClientDetails clientDetails)
    {
        
    }

    public void PokedexButton()
    {
        //isPokedexOpen = !isPokedexOpen;
        PokedexManager.Instance.ShowPokedex();
    }
}
