using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBaseState 
{
    public abstract void EnterState(CharacterStateManager character);
    public abstract void UpdateState(CharacterStateManager character);
    public abstract void OnCollisionEnter(CharacterStateManager character, Collision collision);
    public abstract void TakeDamage(CharacterStateManager character, int Damage);

}
