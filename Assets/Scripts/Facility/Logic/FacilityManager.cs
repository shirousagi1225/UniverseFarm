using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityManager : Singleton<FacilityManager>
{
    public FacilityDataList_SO facilityData;

    //使用設施方法
    public void UseFacility(FacilityName facilityName, Animator facilityAni)
    {
        //判斷設施種類,依可處理異常狀態區分
        if (facilityData.GetFacilityDetails(facilityName).farmlandState != FarmlandState.None)
        {
            bool isAbnormal = true;

            //將場景中所有同類型異常狀態清除
            foreach (var farmland in FindObjectsOfType<Farmland>())
            {
                if (farmland.currentState == facilityData.GetFacilityDetails(facilityName).farmlandState)
                {
                    Transform maintain = farmland.transform.GetChild(2);
                    farmland.currentState = FarmlandState.None;

                    if (isAbnormal)
                        AnimationManager.Instance.FacilityUseing(facilityAni);

                    maintain.GetComponent<Animator>().runtimeAnimatorController = facilityData.GetFacilityDetails(facilityName).maintainAnimatorCO;
                    AnimationManager.Instance.Maintain(maintain);

                    farmland.transform.GetChild(0).gameObject.SetActive(false);
                    isAbnormal = false;
                }
            }
        }
        else if (facilityName.ToString() == "FarmHouse")
            AnimationManager.Instance.FacilityUseing(facilityAni);
    }
}
