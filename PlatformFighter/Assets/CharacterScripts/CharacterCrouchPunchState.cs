using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCrouchPunchState : CharacterBaseState
{
    public float PunchSpeed = 15f;
    private Transform Arm;
    private int step = 0;
    bool AppliedDamage;

    public Vector3 StartRotation = Vector3.zero;
    public override void EnterState(CharacterStateManager character)
    {
        character.currentEnum = CharacterStateManager.States.CharcterCrouchPunchState;
        AppliedDamage = false; 
        
        character.Leg1.SetActive(false);
        character.Leg2.SetActive(true);
        character.transform.position = new Vector3(character.transform.position.x, 2.1f, character.transform.position.z);

        step = 0;
        Arm = character.GetTransform().Find("Arm1");
        character.ShowEyebrows(true);

        StartRotation = character.transform.rotation.eulerAngles;
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            if (step == 1)
            {
                if (!AppliedDamage)
                {
                    collision.gameObject.GetComponent<CharacterStateManager>().TakeDamage(2);
                }
                AppliedDamage = true; 
            }
        }
    }

    public override void UpdateState(CharacterStateManager character)
    {
        character.transform.position = new Vector3(character.transform.position.x, 2.5f, character.transform.position.z);
      
        if (step == 0)
        {
            character.currentMove = CharacterStateManager.MoveState.Charge;
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                                                            Quaternion.Euler(0, StartRotation.y + 45, 0), Time.deltaTime * PunchSpeed);
            character.Arm1.SetActive(false);
            character.Arm2.SetActive(true);

            if (Quaternion.Angle(character.transform.rotation, Quaternion.Euler(0, StartRotation.y + 45, 0)) < 1) { step++; }
        }
        else if (step != 1)
        {
            character.currentMove = CharacterStateManager.MoveState.Attack;
            character.Arm1.transform.localScale = new Vector3(0.5f, 1.2f, 0.5f);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                                                            Quaternion.Euler(StartRotation), Time.deltaTime * PunchSpeed);
            character.Arm1.SetActive(true); 
            character.Arm2.SetActive(false);
            character.Arm1.transform.localPosition = new Vector3(0.443f, 0.626f, -1.029f);
            character.Arm1.transform.localRotation = Quaternion.Euler(0, 0, 90);


            if (Quaternion.Angle(character.transform.rotation, Quaternion.Euler(StartRotation)) < 10) { step++; }
        }
        else if (step == 2)
        {
            character.currentMove = CharacterStateManager.MoveState.Cooldown;
            character.Arm1.transform.localPosition = new Vector3(0, -0.3f, -0.8f);
            character.Arm1.transform.localRotation = Quaternion.Euler(0, 0, 0);

            character.ShowEyebrows(false);
            character.Arm1.transform.localScale = new Vector3(0.5f, 2.6f, 0.5f);
            character.SwitchState(character.CharacterCrouchState);

        }
    }

    public override void TakeDamage(CharacterStateManager character, int Damage)
    {   
        // Interrupts attack 
        if(step == 0)
        {
            character.Arm1.SetActive(true); 
            character.Arm2.SetActive(false);
            character.transform.rotation = Quaternion.Euler(StartRotation);
            character.gameManager.TakeDamage(character.IsPlayer1, Damage); 
            character.Arm1.transform.localPosition = new Vector3(0, -0.3f, -0.8f);
            character.Arm1.transform.localRotation = Quaternion.Euler(0, 0, 0);

            character.ShowEyebrows(false);
            character.SwitchState(character.CharacterCrouchState);
        }
    }
    public override void AIinput (CharacterStateManager character, string input){}
}
