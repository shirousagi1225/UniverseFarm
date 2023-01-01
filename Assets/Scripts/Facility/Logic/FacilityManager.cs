using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityManager : Singleton<FacilityManager>
{
    public FacilityDataList_SO facilityData;

    //�ϥγ]�I��k
    public void UseFacility(FacilityName facilityName, Animator facilityAni)
    {
        //�P�_�]�I����,�̥i�B�z���`���A�Ϥ�
        if (facilityData.GetFacilityDetails(facilityName).farmlandState != FarmlandState.None)
        {
            bool isAbnormal = true;

            //�N�������Ҧ��P�������`���A�M��
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
