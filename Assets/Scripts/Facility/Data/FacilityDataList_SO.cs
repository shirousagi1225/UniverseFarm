using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "FacilityDataList_SO", menuName = "Facility/FacilityDataList_SO")]
public class FacilityDataList_SO : ScriptableObject
{
    public List<FacilityDetails> FacilityDetailsList;

    public FacilityDetails GetFacilityDetails(FacilityName facilityName)
    {
        return FacilityDetailsList.Find(i => i.facilityName == facilityName);
    }
}

[System.Serializable]
public class FacilityDetails
{
    public FacilityName facilityName;
    public FarmlandState farmlandState;
    public AnimatorController maintainAnimatorCO;
}
