using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>,ISaveable
{
    //private Dictionary<FarmlandName,bool> farmlandAvailableDict=new Dictionary<FarmlandName,bool>();

    //�x�s�Ӫ���Ų���ꪬ�A�ƾ�
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
        //���U�O�s�ƾ�
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
        //�x�s�Ӫ���Ų���ꪬ�A
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

        //�٭�Ӫ���Ų���ꪬ�A
        foreach (var cropItem in cropItem.GetComponentsInChildren<Transform>())
        {
            if (cropPokedexAvailableDict[cropItem.name] && cropItem.name != "CropLabel")
                cropItem.GetComponent<PolygonImage>().enabled = cropPokedexAvailableDict[cropItem.name];
        }
    }
}
