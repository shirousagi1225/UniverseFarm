using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    public GameObject timeBar;

    private Dictionary<FarmlandName, TimeSpan> growTimeDict = new Dictionary<FarmlandName, TimeSpan>();
    private FarmlandName currentFarmland;
    private bool canShowGrowTime;
    private Text currentGrowTimeText;

    private void OnEnable()
    {
        EventHandler.SetGrowTimeEvent += OnSetGrowTimeEvent;
        EventHandler.UpdateGrowTimeEvent+= OnUpdateGrowTimeEvent;
        StartCoroutine(GrowCountDown());
        StartCoroutine(CurrentTime());
    }

    private void OnDisable()
    {
        EventHandler.SetGrowTimeEvent -= OnSetGrowTimeEvent;
        EventHandler.UpdateGrowTimeEvent -= OnUpdateGrowTimeEvent;
    }

    private void OnSetGrowTimeEvent(FarmlandName farmlandName,CropStateDetails cropStateDetails)
    {
        growTimeDict.Add(farmlandName,new TimeSpan(cropStateDetails.growTimeHr, cropStateDetails.growTimeMin,0));
    }

    private void OnUpdateGrowTimeEvent(FarmlandName farmlandName, bool isInfoBarOpen, Text growTime)
    {
        currentFarmland = farmlandName;
        canShowGrowTime = isInfoBarOpen;
        currentGrowTimeText = growTime;
    }

    //��ܷ�e�ɶ�
    private IEnumerator CurrentTime()
    {
        while (true)
        {
            timeBar.transform.GetChild(0).GetComponent<Text>().text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            timeBar.transform.GetChild(1).GetComponent<Text>().text = DateTime.Now.ToString("HH�Gmm");
            yield return new WaitForSeconds(1);
        }
    }

    //�@���ͪ��ɶ��˼�(������)
    private IEnumerator GrowCountDown()
    {
        while (true)
        {
            foreach (var farmland in FindObjectsOfType<Farmland>())
            {
                if (growTimeDict.ContainsKey(farmland.farmlandName))
                {
                    //���[�J�P�_�O�_�˼Ƨ���
                    growTimeDict[farmland.farmlandName] -= new TimeSpan(0, 0, 1);
                    Debug.Log(growTimeDict[farmland.farmlandName].ToString());
                }
            }

            //���ե�,����if�P�_�u�ݭncanShowGrowTime
            if (canShowGrowTime&& growTimeDict.ContainsKey(currentFarmland))
                currentGrowTimeText.text = growTimeDict[currentFarmland].ToString();

            yield return new WaitForSeconds(1);
        }
    }
}
