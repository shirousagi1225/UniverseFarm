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
    private TimeSpan judgeAbnormalTime;
    private TimeSpan comingTime;

    private FarmlandName currentFarmland;
    private bool canShowGrowTime;
    private Text currentGrowTimeText;

    private void OnEnable()
    {
        judgeAbnormalTime=new TimeSpan(0,0,0);
        //測試用,正式暫定TimeSpan(0, 9, 59)
        comingTime = new TimeSpan(0,0,19);
        EventHandler.SetGrowTimeEvent += OnSetGrowTimeEvent;
        EventHandler.UpdateGrowTimeEvent += OnUpdateGrowTimeEvent;
        StartCoroutine(GrowCountDown());
        StartCoroutine(CurrentTime());
        StartCoroutine(CustomerComingCountDown());
    }

    private void OnDisable()
    {
        EventHandler.SetGrowTimeEvent -= OnSetGrowTimeEvent;
        EventHandler.UpdateGrowTimeEvent -= OnUpdateGrowTimeEvent;
    }

    private void OnSetGrowTimeEvent(FarmlandName farmlandName, CropStateDetails cropStateDetails)
    {
        growTimeDict.Add(farmlandName, new TimeSpan(0,cropStateDetails.growTimeMin, cropStateDetails.growTimeSec));
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
            timeBar.transform.GetChild(1).GetComponent<Text>().text = DateTime.Now.ToString("HH:mm");
            yield return new WaitForSeconds(1);
        }
    }

    //作物生長時間倒數(未完成)
    private IEnumerator GrowCountDown()
    {
        while (true)
        {
            bool isAbnormalCountDown=false;

            foreach (var farmland in FindObjectsOfType<Farmland>())
            {
                if (growTimeDict.ContainsKey(farmland.farmlandName))
                {
                    //判斷是否倒數完畢(觸發異常狀態停止生長)
                    if (growTimeDict[farmland.farmlandName] == new TimeSpan(0, 0, 0))
                    {
                        growTimeDict.Remove(farmland.farmlandName);
                        EventHandler.CallUpdateFarmlandStateEvent(farmland);
                    }
                    else if(!farmland.transform.GetChild(0).gameObject.activeInHierarchy)
                    {
                        growTimeDict[farmland.farmlandName] -= new TimeSpan(0, 0, 1);
                        FarmlandManager.Instance.SetCropState(farmland.transform.GetChild(3).GetComponent<SpriteRenderer>(),
                            farmland.transform.GetChild(3).GetComponent<Crop>().seedName, growTimeDict[farmland.farmlandName]);
                        Debug.Log(growTimeDict[farmland.farmlandName].ToString());
                    }

                    //暫定(可修改)：每隔一段時間執行觸發異常狀態判斷(目前觸發是所有種植中作物進行一次判斷,非個別判斷)
                    //測試用,正式暫定TimeSpan(0, 4, 59)
                    if (!isAbnormalCountDown && !farmland.transform.GetChild(0).gameObject.activeInHierarchy)
                    {
                        if (judgeAbnormalTime >= new TimeSpan(0, 0, 9))
                        {
                            EventHandler.CallUpdateFarmlandStateEvent(farmland);
                            judgeAbnormalTime = new TimeSpan(0, 0, 0);
                        }
                        else
                            judgeAbnormalTime += new TimeSpan(0, 0, 1);
                        isAbnormalCountDown=true;
                        Debug.Log(judgeAbnormalTime);
                    }
                }

                //測試用,正式if判斷只需要canShowGrowTime
                if (canShowGrowTime && growTimeDict.ContainsKey(currentFarmland))
                    currentGrowTimeText.text = growTimeDict[currentFarmland].ToString();
            }

            yield return new WaitForSeconds(1);
        }
    }

    //顧客光顧時間倒數(未完成)
    private IEnumerator CustomerComingCountDown()
    {
        while (true)
        {
            //需判斷以種植中的農田數量開放進場人數
            if(FindObjectsOfType<Customer>().Length < FindObjectsOfType<Farmland>().Length)
            {
                if (comingTime <= new TimeSpan(0, 0, 0))
                {
                    yield return StartCoroutine(CustomerManager.Instance.CreateCustomer(GameObject.Find("SpawnPoint")));
                    //測試用,正式暫定TimeSpan(0, 9, 59)
                    comingTime = new TimeSpan(0, 0, 9);
                }
                else
                    comingTime -= new TimeSpan(0, 0, 1);
                //Debug.Log(comingTime);
            }
                
            yield return new WaitForSeconds(1);
        }
    }
}
