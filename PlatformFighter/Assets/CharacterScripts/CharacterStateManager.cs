using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    // Character state scripts
    public CharacterBaseState currentState; 
  
    public CharacterBaseState CharacterMoveState = new CharacterMoveState();
    public CharacterBaseState CharacterPunchState = new CharacterPunchState();
    public CharacterBaseState CharacterLegSweepState = new CharacterLegsweepState();

    public CharacterBaseState CharacterCrouchState = new CharacterCrouchState();
    public CharacterBaseState CharcterCrouchPunchState = new CharacterCrouchPunchState();
    public CharacterBaseState CharacterStandlockState = new CharacterStandBlockState();
    public CharacterBaseState CharacterCrouchAndBlockState = new CharacterCrouchBlockState();
    public CharacterBaseState CharacterDownState = new CharacterDownState();
    
    // enum of states (for AI)
    public enum States 
    {
        CharacterMoveState,
        CharacterPunchState,
        CharacterLegSweepState,
        CharacterCrouchState,
        CharcterCrouchPunchState,
        CharacterStandlockState,
        CharacterCrouchAndBlockState,
        CharacterDownState,
        CharacterLastState
    }
    
    public States currentEnum;
    public enum MoveState
    {
        neutral, 
        Charge,
        Attack, 
        Cooldown,
        LastState
    }
    
    public MoveState currentMove = MoveState.neutral; 
    // Components 
    private Rigidbody rb;

    // Body parts
    public GameObject Eyebrow1;
    public GameObject Eyebrow2;

    public GameObject Arm1;
    public GameObject Arm2;
    public GameObject Arm3; 
    public GameObject LeftArm; 
    public GameObject Leg1;
    public GameObject Leg2;

    public GameObject SingleLeg;
    
    // Controls 
    public string MoveLeft = "a";
    public string MoveRight = "d";
    public string Crouch = "s";
    public string Punch = "j";
    public string Kick = "k";

    // Other
    public Vector3 StartPosition; 
    public GameManager gameManager;
    public bool IsPlayer1;
    public Vector3 startRotation; 
    void Awake()
    {
        
        StartPosition = transform.position;
        startRotation = transform.localRotation.eulerAngles;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        ShowEyebrows(false);    
        rb = GetComponent<Rigidbody>();
        currentEnum = States.CharacterMoveState;
        currentState = CharacterMoveState;
        
        currentState.EnterState(this);      
    }

    // Update is called once per frame
    void Update()
    {   
        transform.position = new Vector3(transform.position.x, 4.5f, StartPosition.z);
        currentState.UpdateState(this);
    }
    private void OnCollisionStay(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    public void SwitchState(CharacterBaseState state)
    {
        ResetBodyParts();
        currentState = state;
        currentState.EnterState(this);        
    }
    public void ShowEyebrows(bool Show)
    {
        Eyebrow1.SetActive(Show);
        Eyebrow2.SetActive(Show);
    }
    public void TakeDamage(int Damage)
    {        
        currentState.TakeDamage(this, Damage);
        if(Damage==15 && (currentState != CharacterCrouchState && currentState != CharacterCrouchAndBlockState && currentState != CharcterCrouchPunchState))
        {
            SwitchState(CharacterDownState);
        }
    }
    public Rigidbody GetRigidbody() { return rb; }
    public Transform GetTransform() { return transform; }
    public void ResetBodyParts()
    {
        // Resetting everything 
        Arm1.SetActive(true);
        Arm2.SetActive(false);
        Arm3.SetActive(false);
        Arm1.transform.localPosition = new Vector3(0, -0.3f, -0.8f);
        Arm1.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Arm1.transform.localScale = new Vector3(0.5f, 2.6f, 0.5f);
        // Debug.Log("Start rotation 2 " + startRotation);
        transform.localRotation = Quaternion.Euler(startRotation);
        Leg1.SetActive(true);
        Leg2.SetActive(false);
        ShowEyebrows(false);
        SingleLeg.transform.localPosition = new Vector3(-1.153821f, -2.46f, -1.504989f);
        SingleLeg.transform.rotation = Quaternion.Euler(0f, 25f, 0f);
    }
}
