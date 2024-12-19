using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
//*****************************************
//Creator: SamLee 
//Description: 
//***************************************** 
[RequireComponent(typeof(DecisionRequester), typeof(CarController))]
public class CarAgent : Agent
{
    //[SerializeField] private float currentReward;
    CarController carController;
    [SerializeField] private float stopTimer;
    private void Update()
    {
        
        if (stopTimer <= 5)
            stopTimer += Time.deltaTime;
        else
            ModifyReward(-1f * Time.deltaTime);
    }

    public override void OnEpisodeBegin()
    {
        carController.ResetCar();
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 nextCheckForword = carController.GetNextCheckPointTrans().forward;
        float dirDot = Vector3.Dot(transform.forward, nextCheckForword);
        sensor.AddObservation(dirDot);
        sensor.AddObservation(carController.Speed);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmount = 0;
        float turnAmount = 0;

        switch (actions.DiscreteActions[0])
        {
            case 0: forwardAmount = 0; break;
            case 1: forwardAmount = 1; break;
            case 2: forwardAmount = -1; break;
        }

        switch (actions.DiscreteActions[1])
        {
            case 0: turnAmount = 0; break;
            case 1: turnAmount = 1; break;
            case 2: turnAmount = -1; break;
        }
        
        carController.SetInput(forwardAmount, turnAmount);

    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) forwardAction = 0;
        else if (Input.GetKey(KeyCode.W)) forwardAction = 1;
        else if (Input.GetKey(KeyCode.S)) forwardAction = 2;

        int turnAction = 0;
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) turnAction = 0;
        else if (Input.GetKey(KeyCode.D)) turnAction = 1;
        else if (Input.GetKey(KeyCode.A)) turnAction = 2;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ModifyReward(-0.5f);
            EndEpisode();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ModifyReward(-1f * Time.deltaTime);
            //EndEpisode();
        }
    }
    protected override void Awake()
    {
        base.Awake();
        carController = GetComponent<CarController>();
    }

    public void OnCorrectCheckPoint()
    {
        stopTimer = 0;
        ModifyReward(1f);
    }

    public void OnWrongCheckPoint()
    {
        stopTimer = 0;
        ModifyReward(-1f);
    }

    private void ModifyReward(float amount)
    {
        AddReward(amount);
        //Debug.Log(GetCumulativeReward());
        if (GetCumulativeReward() > 300) EndEpisode();
        if (GetCumulativeReward() < -30) EndEpisode();
    }

}
