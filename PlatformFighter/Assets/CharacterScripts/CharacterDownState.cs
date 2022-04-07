using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDownState : CharacterBaseState
{
    public float FallSpeed = 2f; 
    private int step = 0;

    public float EndFallTime; 
    public override void EnterState(CharacterStateManager character)
    {
        // Resetting everything 
        character.Arm1.SetActive(true);
        character.Arm2.SetActive(false);
        character.Arm1.transform.localPosition = new Vector3(0, -0.3f, -0.8f);
        character.Arm1.transform.localRotation = Quaternion.Euler(0, 0, 0);
        character.transform.rotation = character.startRotation;
        character.ShowEyebrows(false);
        character.SingleLeg.transform.localPosition = new Vector3(-1.153821f, -2.46f, -1.504989f);
        character.SingleLeg.transform.rotation = Quaternion.Euler(0f, 25f, 0f);

        character.currentEnum = CharacterStateManager.States.CharacterDownState;
        EndFallTime = Time.time + FallSpeed;
        step = 0;
    }
    public override void UpdateState(CharacterStateManager character)
    {
        character.transform.position = new Vector3(character.transform.position.x, 2.5f, character.transform.position.z);
        if(step==0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, 
                                                  Quaternion.Euler(character.startRotation.x - 90, 0, 0), Time.deltaTime * FallSpeed);
            if(Quaternion.Angle(character.transform.rotation, Quaternion.Euler(character.startRotation.x - 90, 0, 0)) < 1){step++;}
        }
        if(step == 1 || Time.time>EndFallTime)
        {
            character.transform.position = new Vector3(character.transform.position.x, 4, -character.transform.position.z);
            character.transform.rotation = character.startRotation;   
            character.SwitchState(character.CharacterMoveState); 
        }
    }
    public override void OnCollisionEnter(CharacterStateManager character, Collision collision){}
    public override void TakeDamage(CharacterStateManager character, int Damage){}
    public override void AIinput (CharacterStateManager character, string input){}
}
