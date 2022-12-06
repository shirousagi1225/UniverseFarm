using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UniversalUIList_SO", menuName = "Universal/UniversalUIList_SO")]
public class UniversalUIList_SO : ScriptableObject
{
    public List<UniversalUIDetails> universalUIDetailsList;

    public UniversalUIDetails GetUniversalUIDetails(UniversalUIType UIType)
    {
        return universalUIDetailsList.Find(i => i.UIType == UIType);
    }
}

[System.Serializable]
public class UniversalUIDetails
{
    public UniversalUIType UIType;
    public string title;
    public string buttonText01;
    public string buttonText02;
}
