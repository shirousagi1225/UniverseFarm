using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Facility : MonoBehaviour
{
    public FacilityName facilityName;

    public void FacilityClicked(Animator facilityAni)
    {
        FacilityManager.Instance.UseFacility(facilityName, facilityAni);
    }
}
