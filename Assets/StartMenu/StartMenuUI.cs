using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    public void StartGame()
    {
        EventHandler.CallStartGameEvent();
    }
}
