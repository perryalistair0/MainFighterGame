using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLegsweepState : CharacterBaseState
{
    public float TurnSpeed = 10f;
    public int Step = 0;
    bool AppliedDamage = false;
    public Vector3 StartRotation = Vector3.zero; 
    public override void EnterState(CharacterStateManager character)
    {
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
            Debug.Log("Playre");
            if (Step == 0)
            {
                Debug.Log("Step");
                if (!AppliedDamage)
                {
                    Debug.Log("AppliedDamage ");
                    collision.gameObject.GetComponent<CharacterStateManager>().TakeDamage(15);
                }
                AppliedDamage = true; 
            }
        }
    }

    public override void TakeDamage(CharacterStateManager character, int Damage)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(CharacterStateManager character)
    {
        if(Step == 0)
        {

            character.SingleLeg.transform.localPosition = new Vector3(-0.68f, -1.66f, -1.5f);
            character.SingleLeg.transform.localRotation = Quaternion.Euler(-11.169f, 21.959f, 35.677f);

            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                                                           Quaternion.Euler(0, StartRotation.y -45, 0), Time.deltaTime * TurnSpeed);
            if (Quaternion.Angle(character.transform.rotation, Quaternion.Euler(0, StartRotation.y-45, 0)) < 1) { Step++; }
        }
        if(Step == 1)
        {
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
}
