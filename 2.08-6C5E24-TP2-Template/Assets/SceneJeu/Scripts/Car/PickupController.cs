using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System.Collections;

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

    public GameObject carrosserie;
    public List<Vector3> posVector3;
    bool isDead;

    public NetworkVariable<int> vie = new();
    private NetworkVariable<Vector3> posNetwork = new();
    private NetworkVariable<Color> colorNetwork = new();
    
    public void DecrementVie()
    {
        if (!IsOwner) return;

        if (vie != null)
        {
            vie.Value = vie.Value-10;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();;
        if (!colorNetwork.Value.IsUnityNull())
        {
            foreach (var c in carrosserie.GetComponentsInChildren<Renderer>())
            {
                c.material.color = colorNetwork.Value;
            }
        }
        isDead = false;
        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;
        colorNetwork.OnValueChanged += OnChangeColor;
        posNetwork.OnValueChanged += OnCurrentSpawn;
        if (IsOwner)
        {
            ChangeColorRPC();
            SpawnClientRPC();
        }

    }

    void OnChangeColor(Color prevColor, Color curColor)
    {
        foreach (var c in carrosserie.GetComponentsInChildren<Renderer>()) 
        {
            c.material.color = curColor;
        }
    }

    public void OnCurrentSpawn(Vector3 previous, Vector3 current)
    {
        transform.position = current;
    }

    [Rpc(SendTo.Server)]
    void ChangeColorRPC()
    {
        colorNetwork.Value = Random.ColorHSV();
    }

    [Rpc(SendTo.Server)]
    void SpawnClientRPC()
    {
        Vector3 posRandom = posVector3[Random.Range(0, posVector3.Count)];
        posVector3.Remove(posRandom);
        posNetwork.Value = posRandom;
    }

    void FixedUpdate()
    {
        if (isDead) { transform.transform.position += new Vector3(0, 1f, 0); return; };

        if (vie.Value <= 0)
        {
            Debug.LogWarning("push");
            isDead = true;  
        }

        if (!IsOwner) return;


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


    //activé seulement si istrigger est a true sur le collider (parechoc)
    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner || isDead) return;
 
        if (other.gameObject.tag == "Player")
        {

            AttackOtherPlayer(other.gameObject.gameObject);
        }

        if (other.gameObject.tag == "Parechoc")
        {
        
            DecrementVie();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
      
    }

    private void AttackOtherPlayer(GameObject otherPlayer)
    {
        PickupController other = otherPlayer.gameObject.GetComponent<PickupController>();
        if (other != null)
        {
            other.DecrementVie();
        };
    }

    

}

