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
        //���ե�,�����ȩwTimeSpan(0, 9, 59)
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

    //��ܷ�e�ɶ�
    private IEnumerator CurrentTime()
    {
        while (true)
        {
            timeBar.transform.GetChild(0).GetComponent<Text>().text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            timeBar.transform.GetChild(1).GetComponent<Text>().text = DateTime.Now.ToString("HH:mm");
            yield return new WaitForSeconds(1);
        }
    }

    //�@���ͪ��ɶ��˼�(������)
    private IEnumerator GrowCountDown()
    {
        while (true)
        {
            bool isAbnormalCountDown=false;

            foreach (var farmland in FindObjectsOfType<Farmland>())
            {
                if (growTimeDict.ContainsKey(farmland.farmlandName))
                {
                    //�P�_�O�_�˼Ƨ���(Ĳ�o���`���A����ͪ�)
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

                    //�ȩw(�i�ק�)�G�C�j�@�q�ɶ�����Ĳ�o���`���A�P�_(�ثeĲ�o�O�Ҧ��شӤ��@���i��@���P�_,�D�ӧO�P�_)
                    //���ե�,�����ȩwTimeSpan(0, 4, 59)
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

                //���ե�,����if�P�_�u�ݭncanShowGrowTime
                if (canShowGrowTime && growTimeDict.ContainsKey(currentFarmland))
                    currentGrowTimeText.text = growTimeDict[currentFarmland].ToString();
            }

            yield return new WaitForSeconds(1);
        }
    }

    //�U�ȥ��U�ɶ��˼�(������)
    private IEnumerator CustomerComingCountDown()
    {
        while (true)
        {
            //�ݧP�_�H�شӤ����A�мƶq�}��i���H��
            if(FindObjectsOfType<Customer>().Length < FindObjectsOfType<Farmland>().Length)
            {
                if (comingTime <= new TimeSpan(0, 0, 0))
                {
                    yield return StartCoroutine(CustomerManager.Instance.CreateCustomer(GameObject.Find("SpawnPoint")));
                    //���ե�,�����ȩwTimeSpan(0, 9, 59)
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
