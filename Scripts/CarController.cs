using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//*****************************************
//Creator: SamLee 
//Description: 
//***************************************** 
public class CarController : MonoBehaviour
{
    private float inputForword;
    private float inputTurn;
    public CheckPointsContainer checkPointsContainer;
    public Transform spawnPoint;
    private CarAgent carAgent;
    public bool IsAI => carAgent != null;

    public float Speed { get { return Vector3.Dot(transform.forward, rb.velocity); } }
    public float maxSpeed = 6;
    public float turningSpeed = 110;
    public float accelaration = 3;

    private Rigidbody rb;
    public int NextCheckPointIndex {get; private set;}


    private void OnEnable()
    {
        EventHandler.CarThroughCheckPointCorrectEvent += OnCarThroughCheckPointCorrectEvent;
        EventHandler.CarThroughCheckPointWrongEvent += OnCarThroughCheckPointWrongEvent;
    }

    private void OnDisable()
    {
        EventHandler.CarThroughCheckPointCorrectEvent -= OnCarThroughCheckPointCorrectEvent;
        EventHandler.CarThroughCheckPointWrongEvent -= OnCarThroughCheckPointWrongEvent;
    }

    
    private void OnCarThroughCheckPointCorrectEvent(CarController controller, CheckPointSingle single)
    {
        if (controller == this && IsAI) carAgent.OnCorrectCheckPoint();
    }

    private void OnCarThroughCheckPointWrongEvent(CarController controller, CheckPointSingle single)
    {
        if (controller == this && IsAI) carAgent.OnWrongCheckPoint();
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        carAgent = GetComponent<CarAgent>();
        ResetCar();
    }

    public void SetCheckPointsContainer(CheckPointsContainer container)
    {
        checkPointsContainer = container;
    }

    public void SetNextCheckPointIndex(int index)
    {
        NextCheckPointIndex = index;
    }

    

    public void SetInput(float accelarate, float turn)
    {
        inputForword = accelarate;
        inputTurn = turn;
    }
    
    private void Move()
    {

        bool speedPositive = Vector3.Dot(transform.forward, rb.velocity) > 0;

        rb.AddForce(transform.forward * inputForword * accelaration * Time.deltaTime, ForceMode.VelocityChange);

        //Fraction Simulation
        if (speedPositive) rb.AddForce(- transform.forward * inputForword * accelaration/10 * Time.deltaTime, ForceMode.VelocityChange);
        else rb.AddForce(transform.forward * inputForword * accelaration / 10 * Time.deltaTime, ForceMode.VelocityChange);

        if (rb.velocity.magnitude > maxSpeed) rb.velocity = speedPositive? maxSpeed * transform.forward : -maxSpeed * transform.forward;
        
        transform.Rotate(Vector3.up * inputTurn * turningSpeed * Time.deltaTime);
        speedPositive = Vector3.Dot(transform.forward, rb.velocity) > 0;
        rb.velocity = speedPositive ? rb.velocity.magnitude * transform.forward : -rb.velocity.magnitude * transform.forward;

    }

    public void ResetCar()
    {
        NextCheckPointIndex = 0;
        transform.position = spawnPoint.position;
        transform.forward = spawnPoint.forward;
        StopCompletely();
    }

    private void StopCompletely()
    {
        SetInput(0, 0);
        rb.velocity = Vector3.zero;
    }

    public Transform GetNextCheckPointTrans()
    {
        return checkPointsContainer.GetCheckPointTrans(NextCheckPointIndex);
    }
    private void FixedUpdate()
    {

        //if (!IsAI) SetInput(//TODO);

        Move();
    }
    void Start()
    {

    }

    void Update()
    {

    } 
}
