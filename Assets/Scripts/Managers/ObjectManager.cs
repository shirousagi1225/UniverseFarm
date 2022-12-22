using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>,ISaveable
{
    //private Dictionary<FarmlandName,bool> farmlandAvailableDict=new Dictionary<FarmlandName,bool>();

    //儲存植物圖鑑解鎖狀態數據
    public GameObject cropItem;
    private Dictionary<string, bool> cropPokedexAvailableDict = new Dictionary<string, bool>();

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.UpdateCropPokedexEvent += OnUpdateCropPokedexEvent;
    }

    private void Start()
    {
        //註冊保存數據
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.UpdateCropPokedexEvent -= OnUpdateCropPokedexEvent;
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

    private void OnUpdateCropPokedexEvent(ItemDetails itemDetails)
    {
        if(itemDetails!=null)
            cropPokedexAvailableDict[itemDetails.pokedexNum.ToString()]=true;
    }

    public SaveData GenerateSaveData()
    {
        //儲存植物圖鑑解鎖狀態
        foreach (var cropItem in cropItem.GetComponentsInChildren<Transform>())
        {
            if (!cropPokedexAvailableDict.ContainsKey(cropItem.name))
                cropPokedexAvailableDict.Add(cropItem.name, false);
        }

        SaveData saveData = new SaveData();
        saveData.cropPokedexAvailableDict= cropPokedexAvailableDict;

        return saveData;
    }

    public void RestoreGameData(SaveData saveData)
    {
        cropPokedexAvailableDict = saveData.cropPokedexAvailableDict;

        //還原植物圖鑑解鎖狀態
        foreach (var cropItem in cropItem.GetComponentsInChildren<Transform>())
        {
            if (cropPokedexAvailableDict[cropItem.name] && cropItem.name != "CropLabel")
                cropItem.GetComponent<PolygonImage>().enabled = cropPokedexAvailableDict[cropItem.name];
        }
    }
}
