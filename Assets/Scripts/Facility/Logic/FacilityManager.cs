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
                    farmland.currentState = FarmlandState.Good; ;

                    if (isAbnormal)
                        AnimationManager.Instance.FacilityUseing(facilityAni);

                    AnimatorOverrideController aniCO = new AnimatorOverrideController(maintain.GetComponent<Animator>().runtimeAnimatorController);
                    maintain.GetComponent<Animator>().runtimeAnimatorController = aniCO;
                    aniCO[aniCO.animationClips[0]] = facilityData.GetFacilityDetails(facilityName).maintainAniClip;
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
