using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCrouchState : CharacterBaseState
{
    public string Crouch = "s";
    public string Punch = "j";
    public override void EnterState(CharacterStateManager character)
    {
        Crouch = character.Crouch;
        Punch = character.Punch;

        character.Leg1.SetActive(false);
        character.Leg2.SetActive(true);
        character.transform.position = new Vector3(character.transform.position.x, 2.1f, character.transform.position.z);

    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision collision)
    {
        
    }

    public override void TakeDamage(CharacterStateManager character, int Damage)
    {
        character.gameManager.TakeDamage(character.IsPlayer1, Damage/5);
        character.SwitchState(character.CharacterStandlockState);
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if (!Input.GetKey(Crouch))
        {
            character.Leg1.SetActive(true);
            character.Leg2.SetActive(false);
            character.transform.position = new Vector3(character.transform.position.x, 3f, character.transform.position.z);
            character.SwitchState(character.CharacterMoveState);
        }
        if (Input.GetKey(Punch))
        {
            character.SwitchState(character.CharcterCrouchPunchState);
        }
        character.transform.position = new Vector3(character.transform.position.x, 2.5f, character.transform.position.z);
    }
}
