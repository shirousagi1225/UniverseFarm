using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FarmlandDataList", menuName = "Farmland/FarmlandDataList_SO")]
public class FarmlandDataList_SO : ScriptableObject
{
    public List<FarmlandDetails> farmlandDetailsList;

    public FarmlandDetails GetFarmlandDetails(FarmlandName farmlandName)
    {
        return farmlandDetailsList.Find(i => i.farmlandName == farmlandName);
    }
}

[System.Serializable]
public class FarmlandDetails
{
    public FarmlandName farmlandName;
    public bool isPlant;
    public FarmlandState farmlandState;
    public Sprite farmlandStateSprite;
    public ItemName seedName;
    public CropState cropState;
    public Sprite cropStateSprite;
}
