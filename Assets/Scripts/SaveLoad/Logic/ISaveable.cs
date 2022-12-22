public interface ISaveable
{
    //���U�s�ɤ���
    void SaveableRegister()
    {
        SaveLoadManager.Instance.Register(this);
    }

    SaveData GenerateSaveData();

    void RestoreGameData(SaveData saveData);
}
