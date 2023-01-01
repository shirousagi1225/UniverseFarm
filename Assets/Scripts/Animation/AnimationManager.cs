using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    //設施動畫方法
    public void FacilityUseing(Animator facilityAni)
    {
        if(facilityAni.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            facilityAni.SetTrigger("Use");
    }

    //維護動畫方法
    public void Maintain(Transform maintain)
    {
        Animator maintainAni= maintain.GetComponent<Animator>();
        maintainAni.Play(maintainAni.GetCurrentAnimatorStateInfo(0).shortNameHash,0,0);
    }
}
