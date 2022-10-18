using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropDataList", menuName = "Farmland/CropDataList_SO")]
public class CropDataList_SO : ScriptableObject
{
    public List<CropStateDetails> CropStateDetailsList;

    public CropStateDetails GetCropStateDetails(ItemName seedName)
    {
        return CropStateDetailsList.Find(i => i.seedName == seedName);
    }

    public CropDetails GetCropDetails(ItemName seedName, CropState cropState)
    {
        return GetCropStateDetails(seedName).CropDetailsList.Find(i => i.cropState == cropState);
    }
}

[System.Serializable]
public class CropStateDetails
{
    public ItemName seedName;
    public int produce;
    public List<CropDetails> CropDetailsList;
}

[System.Serializable]
public class CropDetails
{
    public CropState cropState;
    public Sprite cropStateSprite;
}
