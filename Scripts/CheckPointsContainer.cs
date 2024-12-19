using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//*****************************************
//Creator: SamLee 
//Description: 
//***************************************** 
public class CheckPointsContainer : MonoBehaviour
{
    
    private List<CheckPointSingle> checkPointList = new List<CheckPointSingle>();
    [SerializeField] private List<CarController> carList = new List<CarController>();
    public int CheckPointsNum => checkPointList.Count;
    private void Awake()
    {
        checkPointList.Clear();
        

        for (int i=0; i<transform.childCount; i++)
        {
            Transform checkPointTrans = transform.GetChild(i);
            CheckPointSingle checkPoint = checkPointTrans.GetComponent<CheckPointSingle>();
            checkPoint.SetBasicInfo(this, i);
            checkPointList.Add(checkPoint);
        }


        foreach (CarController car in carList)
        {
            car.SetCheckPointsContainer(this);
        }
    }

    public void CarThroughCheckPoint(CheckPointSingle checkPointSingle, CarController car, int carNextIndex)
    {
        int index = checkPointList.IndexOf(checkPointSingle);
        
        if (index == car.NextCheckPointIndex)
        {
            EventHandler.CallCarThroughCheckPointCorrectEvent(car, checkPointSingle);
        }
        else
        {
            EventHandler.CallCarThroughCheckPointWrongEvent(car, checkPointSingle);
        }
        car.SetNextCheckPointIndex(carNextIndex);

    }

    public Transform GetCheckPointTrans(int index)
    {
        return checkPointList[index].transform;
    }
    void Start()
    {

    }

    void Update()
    {

    } 
}
