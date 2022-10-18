using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //private Dictionary<FarmlandName,bool> farmlandAvailableDict=new Dictionary<FarmlandName,bool>();

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        
    }

    private void OnAfterSceneLoadedEvent()
    {
        /*foreach (var farmland in FindObjectsOfType<Farmland>())
        {
            if (!farmlandAvailableDict.ContainsKey(farmland.farmlandName))
                farmlandAvailableDict.Add(farmland.farmlandName,true);
        }*/
    }
}
