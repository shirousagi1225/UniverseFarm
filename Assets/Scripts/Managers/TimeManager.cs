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

    //顯示當前時間
    private IEnumerator CurrentTime()
    {
        while (true)
        {
            timeBar.transform.GetChild(0).GetComponent<Text>().text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            timeBar.transform.GetChild(1).GetComponent<Text>().text = DateTime.Now.ToString("HH：mm");
            yield return new WaitForSeconds(1);
        }
    }

    //作物生長時間倒數(未完成)
    private IEnumerator GrowCountDown()
    {
        while (true)
        {
            foreach (var farmland in FindObjectsOfType<Farmland>())
            {
                if (growTimeDict.ContainsKey(farmland.farmlandName))
                {
                    //須加入判斷是否倒數完畢
                    growTimeDict[farmland.farmlandName] -= new TimeSpan(0, 0, 1);
                    Debug.Log(growTimeDict[farmland.farmlandName].ToString());
                }
            }

            //測試用,正式if判斷只需要canShowGrowTime
            if (canShowGrowTime&& growTimeDict.ContainsKey(currentFarmland))
                currentGrowTimeText.text = growTimeDict[currentFarmland].ToString();

            yield return new WaitForSeconds(1);
        }
    }
}
