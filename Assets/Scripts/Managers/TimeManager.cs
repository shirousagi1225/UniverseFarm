using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
    public GameObject timeBar;

    private Dictionary<FarmlandName, DateTime> startGrowTimeDict = new Dictionary<FarmlandName, DateTime>();
    private Dictionary<FarmlandName, TimeSpan> growTimeDict = new Dictionary<FarmlandName, TimeSpan>();

    private void OnEnable()
    {
        EventHandler.SetGrowTimeEvent += OnSetGrowTimeEvent;
        StartCoroutine(GrowCountDown());
        StartCoroutine(CurrentTime());
    }

    private void OnDisable()
    {
        EventHandler.SetGrowTimeEvent -= OnSetGrowTimeEvent;
    }

    private void OnSetGrowTimeEvent(FarmlandName farmlandName,CropStateDetails cropStateDetails, DateTime startGrowTime)
    {
        startGrowTimeDict.Add(farmlandName, startGrowTime);
        growTimeDict.Add(farmlandName,new TimeSpan(cropStateDetails.growTimeHr, cropStateDetails.growTimeMin,0));
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
                if (startGrowTimeDict.ContainsKey(farmland.farmlandName))
                {
                    //須加入判斷是否倒數完畢
                    var currentGrowTime = DateTime.Now.Subtract(startGrowTimeDict[farmland.farmlandName]).Duration();
                    var lastGrowTime=growTimeDict[farmland.farmlandName] - new TimeSpan((int)currentGrowTime.TotalHours, (int)currentGrowTime.TotalMinutes, (int)currentGrowTime.TotalSeconds);
                    if (GameObject.Find("InfoBar") != null)
                    {
                        GameObject.Find("InfoBar").transform.GetChild(0).GetComponent<Text>().text = lastGrowTime.ToString();
                        Debug.Log(lastGrowTime.ToString());
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
