public interface ISaveable
{
    //µù¥U¦sÀÉ¤¶­±
    void SaveableRegister()
    {
        SaveLoadManager.Instance.Register(this);
    }

    SaveData GenerateSaveData();

    void RestoreGameData(SaveData saveData);
}
