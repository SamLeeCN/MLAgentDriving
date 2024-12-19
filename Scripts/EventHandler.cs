using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//*****************************************
//Creator: SamLee 
//Description: 
//***************************************** 
public static class EventHandler
{
    public static event Action<CarController, CheckPointSingle> CarThroughCheckPointCorrectEvent;

    public static void CallCarThroughCheckPointCorrectEvent(CarController car, CheckPointSingle checkPoint)
    {
        CarThroughCheckPointCorrectEvent?.Invoke(car, checkPoint);
    }

    public static event Action<CarController, CheckPointSingle> CarThroughCheckPointWrongEvent;

    public static void CallCarThroughCheckPointWrongEvent(CarController car, CheckPointSingle checkPoint)
    {
        CarThroughCheckPointWrongEvent?.Invoke(car, checkPoint);
    }


    
}
