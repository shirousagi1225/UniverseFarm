using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    //�]�I�ʵe��k
    public void FacilityUseing(Animator facilityAni)
    {
        if(facilityAni.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            facilityAni.SetTrigger("Use");
    }

    //���@�ʵe��k
    public void Maintain(Transform maintain)
    {
        Animator maintainAni= maintain.GetComponent<Animator>();
        maintainAni.Play(maintainAni.GetCurrentAnimatorStateInfo(0).shortNameHash,0,0);
    }
}
