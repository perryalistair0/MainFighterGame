using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCrouchBlockState : CharacterBaseState
{
    float BlockStopTime;
    float BlockDuration = 0.5f;
    public override void EnterState(CharacterStateManager character)
    {
        character.Arm1.SetActive(false);
        character.LeftArm.SetActive(false);
        character.Arm3.SetActive(true);

        BlockStopTime = Time.time + BlockDuration;

    }
    public override void UpdateState(CharacterStateManager character)
    {
        if (Time.time > BlockStopTime)
        {
            character.Arm1.SetActive(true);
            character.LeftArm.SetActive(true);
            character.Arm3.SetActive(false);
            character.SwitchState(character.CharacterCrouchState);
        }
        character.transform.position = new Vector3(character.transform.position.x, 2.5f, character.transform.position.z);
    }
    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {

    }
    public override void TakeDamage(CharacterStateManager character, int Damage)
    {
    }

}
