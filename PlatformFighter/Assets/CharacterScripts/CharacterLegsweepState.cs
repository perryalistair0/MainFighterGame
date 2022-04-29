using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLegsweepState : CharacterBaseState
{
    public float TurnSpeed = 6f;
    public int Step = 0;
    bool AppliedDamage = false;
    public Vector3 StartRotation = Vector3.zero; 
    public override void EnterState(CharacterStateManager character)
    {
        character.currentEnum = CharacterStateManager.States.CharacterLegSweepState;
        // Debug.Log("Current enum" + character.currentEnum);
        AppliedDamage = false;

        character.ShowEyebrows(true);
        Step = 0;

        StartRotation = character.transform.rotation.eulerAngles;
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {
        GameObject other = collision.gameObject;
        if (other.CompareTag("Player"))
        {
            if (Step == 1)
            {
                if (!AppliedDamage)
                {

                    collision.gameObject.GetComponent<CharacterStateManager>().TakeDamage(15);
                }
                AppliedDamage = true; 
            }
        }
    }

    public override void TakeDamage(CharacterStateManager character, int Damage)
    {
        
        if(Step != 2)
        {
            character.SingleLeg.transform.localPosition = new Vector3(-1.153821f, -2.46f, -1.504989f);
            character.SingleLeg.transform.rotation = Quaternion.Euler(0f, 25f, 0f);
            character.transform.rotation = Quaternion.Euler(StartRotation);
            character.gameManager.TakeDamage(character.IsPlayer1, Damage); 

            character.ShowEyebrows(false);
            character.SwitchState(character.CharacterMoveState);
        }
        else if(Damage == 15)
        {
            character.gameManager.TakeDamage(character.IsPlayer1, Damage); 
        }
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if(Step == 0)
        {
            character.currentMove = CharacterStateManager.MoveState.Charge;
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                                                           Quaternion.Euler(0, StartRotation.y + 45, 0), Time.deltaTime * TurnSpeed);
            if (Quaternion.Angle(character.transform.rotation, Quaternion.Euler(0, StartRotation.y + 45, 0)) < 1) { Step++; }
        }
        if(Step == 1)
        {
            character.currentMove = CharacterStateManager.MoveState.Attack;
            //character.SingleLeg.transform.localPosition = new Vector3(-0.68f, -1.66f, -1.5f);
            //character.SingleLeg.transform.localRotation = Quaternion.Euler(-11.169f, 21.959f, 35.677f);
            character.SingleLeg.transform.localPosition = new Vector3(-0.1f, -1.4f, -2.1f);
            character.SingleLeg.transform.localRotation = Quaternion.Euler(1.16f, 38f, 81f);

            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                                                           Quaternion.Euler(0, StartRotation.y -45, 0), Time.deltaTime * TurnSpeed);
            if (Quaternion.Angle(character.transform.rotation, Quaternion.Euler(0, StartRotation.y-45, 0)) < 10) { Step++; }
        }
        if(Step == 2)
        {
            character.currentMove = CharacterStateManager.MoveState.Cooldown;
            character.SingleLeg.transform.localPosition = new Vector3(-1.153821f, -2.46f, -1.504989f);
            character.SingleLeg.transform.rotation = Quaternion.Euler(0f, 25f, 0f);

            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                                                           Quaternion.Euler(StartRotation), Time.deltaTime * TurnSpeed);
            if (Quaternion.Angle(character.transform.rotation, Quaternion.Euler(StartRotation)) < 1)
            {
                character.ShowEyebrows(false);
                character.SwitchState(character.CharacterMoveState);
            }
        }
    }
    public override void AIinput (CharacterStateManager character, string input) {}
}
