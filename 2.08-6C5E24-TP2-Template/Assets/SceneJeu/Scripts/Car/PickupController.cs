using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Linq;
public class PickupController : NetworkBehaviour
{
    public const string HORIZONTAL_AXIS = "Horizontal";
    public const string VERTICAL_AXIS = "Vertical";

    [SerializeField]
    private float horizontalInput;

    [SerializeField]
    private float verticalInput;


    private bool isBraking;
    private float currentBrakeForce;
    private float currentSteerAngle;


    [SerializeField]
    private WheelCollider FLWheelCollider;
    [SerializeField]
    private WheelCollider FRWheelCollider;
    [SerializeField]
    private WheelCollider RLWheelCollider;
    [SerializeField]
    private WheelCollider RRWheelCollider;

    [SerializeField]
    private Transform FLWheelTransform;
    [SerializeField]
    private Transform FRWheelTransform;
    [SerializeField]
    private Transform RLWheelTransform;
    [SerializeField]
    private Transform RRWheelTransform;

    [SerializeField]
    private float motorForce;
    [SerializeField]
    private float brakeForce;
    [SerializeField]
    private float maxSteeringAngle;
    [SerializeField]
    private float centreOfGravityOffset = -1f;

    Rigidbody rigidBody;

    bool isDead;

    public NetworkVariable<int> vie = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private readonly List<Color> _colors = new() { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta, Color.white, Color.black, Color.gray, Color.grey, Color.clear };
    private void DecrementVie()
    {
        
        if (vie != null)
        {
            
            vie.Value = vie.Value-10;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        
        isDead = false;
        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;
        Move();
        Color randomColor = _colors[Random.Range(0, _colors.Count)];

        //random color 
        GameObject.FindGameObjectsWithTag("Carosserie")
                  .ToList()
                  .ForEach(carrosserie => carrosserie.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor));
    }


    void FixedUpdate()
    {
        if (vie.Value <= 0)
        {
            isDead = true;
        }

        if (!IsOwner) return;

        if(isDead) return;

        GetInput();
        HandleMove();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL_AXIS);
        verticalInput = Input.GetAxis(VERTICAL_AXIS);
        isBraking = Input.GetKey(KeyCode.Space);
        ApplyBraking();
    }
    private void ApplyBraking()
    {
        FLWheelCollider.brakeTorque = currentBrakeForce;
        FRWheelCollider.brakeTorque = currentBrakeForce;
        RLWheelCollider.brakeTorque = currentBrakeForce;
        RRWheelCollider.brakeTorque = currentBrakeForce;
    }
    private void HandleMove()
    {
        float motorTorque = verticalInput * motorForce;
        FLWheelCollider.motorTorque = motorTorque;
        FRWheelCollider.motorTorque = motorTorque;
        currentBrakeForce = isBraking ? brakeForce : 0f;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        FLWheelCollider.steerAngle = currentSteerAngle;
        FRWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateWheel(FLWheelCollider, FLWheelTransform);
        UpdateWheel(FRWheelCollider, FRWheelTransform);
        UpdateWheel(RLWheelCollider, RLWheelTransform);
        UpdateWheel(RRWheelCollider, RRWheelTransform);
    }

    private void UpdateWheel(WheelCollider wheelCollider, Transform tranform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        tranform.rotation = rot;
        tranform.position = pos;
    }


    private void Move()
    {
         // Only the owner client should move the object
         transform.position = new Vector3(-325, 70, Random.Range(-55,40));   
    }


    private void OnCollisionEnter(Collision collision)
    {
        

        //Debug.Log(collision.gameObject);
        if (!IsOwner || isDead) return;

        DecrementVie();
    }

}