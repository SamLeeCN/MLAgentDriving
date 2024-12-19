using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//*****************************************
//Creator: SamLee 
//Description: 
//***************************************** 
public class CheckPointSingle : MonoBehaviour
{
    private CheckPointsContainer container;
    private int index;


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            CarController car = other.GetComponent<CarController>();
            bool isCarForward = Vector3.Dot(transform.forward, car.transform.position - transform.position) > 0;
            int carNextIndex = isCarForward ? index + 1 : index;
            carNextIndex = (carNextIndex + container.CheckPointsNum) % container.CheckPointsNum;
            container.CarThroughCheckPoint(this, car, carNextIndex);
        }
    }

    public void SetBasicInfo(CheckPointsContainer container, int index)
    {
        this.container = container;
        this.index = index;
    }


    
    void Start()
    {

    }

    void Update()
    {

    } 
}
