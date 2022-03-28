using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    // Character states
    CharacterBaseState currentState; 
    public CharacterBaseState CharacterMoveState = new CharacterMoveState();
    public CharacterBaseState CharacterPunchState = new CharacterPunchState();
    public CharacterBaseState CharacterLegSweepState = new CharacterLegsweepState();

    public CharacterBaseState CharacterCrouchState = new CharacterCrouchState();
    public CharacterBaseState CharcterCrouchPunchState = new CharacterCrouchPunchState();
    
    // Components 
    private Rigidbody rb;

    // Body parts
    public GameObject Eyebrow1;
    public GameObject Eyebrow2;

    public GameObject Arm1;
    public GameObject Arm2;
    public GameObject Arm3; 

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
    private GameManager gameManager;
    public bool IsPlayer1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ShowEyebrows(false);    
        rb = GetComponent<Rigidbody>();

        currentState = CharacterMoveState;

        currentState.EnterState(this);      
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }
    private void OnCollisionStay(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    public void SwitchState(CharacterBaseState state)
    {
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
        Debug.Log("damage taken");
        gameManager.TakeDamage(IsPlayer1, Damage);
    }
    public Rigidbody GetRigidbody() { return rb; }
    public Transform GetTransform() { return transform; }
    
}
