using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    private string saveDataFolder;
    private List<ISaveable> saveableList=new List<ISaveable>();
    private Dictionary<string,SaveData> saveDataDict = new Dictionary<string, SaveData>();

    private bool isStartGame;
    private bool isRemove;

    protected override void Awake()
    {
        base.Awake();
        saveDataFolder =Application.persistentDataPath+"/SAVE/";
        isStartGame = true;
        isRemove = false;
        Debug.Log(saveDataFolder);
    }

    private void OnEnable()
    {
        EventHandler.StartGameEvent += OnStartGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.StartGameEvent -= OnStartGameEvent;
    }

    private void OnStartGameEvent()
    {
        StartCoroutine(Load());
        isStartGame = false;
    }

    public void Register(ISaveable saveable)
    {
        saveableList.Add(saveable);
    }

    public IEnumerator Save()
    {
        saveDataDict.Clear();

        foreach(var saveable in saveableList)
            saveDataDict.Add(saveable.GetType().Name,saveable.GenerateSaveData());

        var resultPath = saveDataFolder + "data.sav";

        StringBuilder saveDataJson = new StringBuilder();
        JsonWriter jw = new JsonWriter(saveDataJson);
        //設置為格式化模式
        jw.PrettyPrint = true;
        //縮排空格數
        jw.IndentValue = 4;
        JsonMapper.ToJson(saveDataDict, jw);

        if (!File.Exists(resultPath))
            Directory.CreateDirectory(saveDataFolder);
        File.WriteAllText(resultPath, saveDataJson.ToString());

        yield return 0; 
    }

    public IEnumerator Load()
    {
        var resultPath = saveDataFolder + "data.sav";
        if (File.Exists(resultPath)) 
        {
            var saveData = JsonMapper.ToObject<Dictionary<string, SaveData>>(File.ReadAllText(resultPath));
            //Debug.Log(saveData);
            //Debug.Log(saveableList);

            foreach (var saveable in saveableList)
                saveable.RestoreGameData(saveData[saveable.GetType().Name]);
        }

        yield return 0;
    }

    public IEnumerator Remove()
    {
        var resultPath = saveDataFolder + "data.sav";
        if (File.Exists(resultPath))
            File.Delete(resultPath);
        isRemove=true;

        yield return 0;
    }

    private void OnApplicationQuit()
    {
        if(!isRemove&& !isStartGame)
            StartCoroutine(Save());
    }

    private void OnApplicationPause(bool pause)
    {
        if (!isStartGame)
        {
            if (pause == true)
                StartCoroutine(Save());
            else if (pause == false)
                StartCoroutine(Load());
        }
    }
}
