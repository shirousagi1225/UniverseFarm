using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FarmlandStateDataList", menuName = "Farmland/FarmlandStateDataList_SO")]
public class FarmlandStateDataList_SO : ScriptableObject
{
    public List<FarmlandStateDetails> farmlandStateDetailsList;

    public FarmlandStateDetails GetFarmlandStateDetails(FarmlandState farmlandState)
    {
        return farmlandStateDetailsList.Find(i => i.farmlandState == farmlandState);
    }
}

[System.Serializable]
public class FarmlandStateDetails
{
    public FarmlandState farmlandState;
    public Sprite farmlandStateSprite;
}
