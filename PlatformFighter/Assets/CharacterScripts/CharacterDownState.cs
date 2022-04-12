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
