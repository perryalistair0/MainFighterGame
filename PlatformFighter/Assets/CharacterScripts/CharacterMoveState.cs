using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveState : CharacterBaseState
{

    private Rigidbody rb;
    public int Speed = 10;
    public float PunchSpeed = 0.2f;

    // Controls 
    public string MoveLeft = "a";
    public string MoveRight = "d";
    public string Crouch = "s";
    public string Punch = "j";
    public string Kick = "k";

    public override void EnterState(CharacterStateManager character)
    {
        rb = character.GetRigidbody();

        MoveLeft = character.MoveLeft;
        MoveRight = character.MoveRight;
        Crouch = character.Crouch;
        Punch = character.Punch;
        Kick = character.Kick;
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision) 
    {
        
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (Input.GetKey(MoveLeft))
        {
            rb.velocity = new Vector3(-Speed, 0, 0);
        }
        else if (Input.GetKey(MoveRight))
        {
            rb.velocity = new Vector3(Speed, 0, 0);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        if (Input.GetKeyDown(Punch))
        {
            rb.velocity = Vector3.zero;
            character.SwitchState(character.CharacterPunchState);
        }
        if (Input.GetKeyDown(Crouch))
        {
            rb.velocity = Vector3.zero;
            character.SwitchState(character.CharacterCrouchState);
        }
        if (Input.GetKeyDown(Kick))
        {
            rb.velocity = Vector3.zero;
            character.SwitchState(character.CharacterLegSweepState);
        }
        //character.transform.position = new Vector3(character.transform.position.x, 4.5f, character.transform.position.z);
    }

    public override void TakeDamage(CharacterStateManager character, int Damage)
    {
        // Is blocking
        if((character.IsPlayer1 && Input.GetKey(MoveLeft)) ||
          ((!character.IsPlayer1 && Input.GetKey(MoveRight))))
        {
            character.gameManager.TakeDamage(character.IsPlayer1, Damage/5);
            character.SwitchState(character.CharacterStandlockState);
        }
        // Not blocking 
        else 
        {
            character.gameManager.TakeDamage(character.IsPlayer1, Damage);            
        }
    }
}
